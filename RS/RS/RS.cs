using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using RS.Service;

namespace RS
{
    public partial class RS : Form
    {
        private HttpListener listener = new HttpListener();

        private string IP = "0.0.0.0";
        private static int port = 8605;
        private readonly string ruleName = "WoWs RS";
        private string gamePath = "";

        private PortService portService = new PortService();
        private GamePathService gamePathService = new GamePathService();
        private LocalServer localServer = null;

        public RS()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.lang);
            pathLabel.Text = Properties.strings.path;
            startBtn.Text = Properties.strings.start;
            aboutMenu.Text = Properties.strings.about;
            checkForUpdateToolStripMenuItem.Text = Properties.strings.check_for_update;
            languageToolStripMenuItem.Text = Properties.strings.language;

            pathBox.Text = GetSavedPath();

            // Get current IP address
            IP = portService.GetIPAddress();
            ipLabel.Text = IP;
        }

        private string GetSavedPath()
        {
            var saved_path = Properties.Settings.Default.path;
            if (saved_path.Trim() == "")
            {
                try
                {
                    var found_path = gamePathService.LocalGamePath();
                    if (found_path != null)
                    {
                        saved_path = found_path;

                    }
                }
                catch
                {
                    // Failed, it is fine
                }
            }
            return saved_path;
        }

        #region Button Clicks
        private void githubMenu_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/HenryQuan/WoWs-RS");
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/HenryQuan/WoWs-RS/releases/latest");
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            // Deal with game path
            gamePath = pathBox.Text;
            if (!gamePathService.ValidatePath(gamePath))
            {
                MessageBox.Show(Properties.strings.invalid_path, Properties.strings.error_title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // save the game path
            Properties.Settings.Default.path = gamePath;
            Properties.Settings.Default.Save();

            try
            {
                // Make sure replay is on
                gamePathService.EnableReplay(gamePath);
                if (localServer == null)
                {
                    localServer = new LocalServer(gamePath);
                }

                // Request the Admin access
                if (!portService.FirewallRuleExists(ruleName))
                {
                    var result = MessageBox.Show(Properties.strings.why_admin, "WoWs RS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        portService.RegisterPort(ruleName, port);
                    }
                    else
                    {
                        Application.ExitThread();
                        return;
                    }
                }

                localServer.Start(port);
                ipLabel.ForeColor = Color.Green;
                startBtn.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.strings.invalid_path + "\n\n" + ex.Message, Properties.strings.error_title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.lang = "en";
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        private void chineseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.lang = "zh";
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        private void japaneseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.lang = "ja";
            Properties.Settings.Default.Save();
            Application.Restart();
        }

        #endregion

        private void RS_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener.Close();
            if (localServer != null)
            {
                localServer.Stop();
            }
            Application.ExitThread();
        }

        private void AboutGamePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string content = Properties.strings.how_to_use;
            content += "\nD:\\Games\\World_of_Warships\n";
            content += "D:\\Games\\Steam\\steamapps\\common\\World_of_Warships";
            MessageBox.Show(content, Properties.strings.about_path);
        }
    }
}
