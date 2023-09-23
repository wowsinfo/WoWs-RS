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
        private string gamePath = "";

        private PortService portService= new PortService();
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
            howToUseToolStripMenuItem.Text = Properties.strings.how_to_use;

            pathBox.Text = Properties.Settings.Default.path;

            // Get current IP address
            this.IP = GetIPAddress();
            ipLabel.Text = this.IP;

            /*try
            {
                portService.RegisterPort("WoWs RS", port);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Properties.strings.error_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }*/
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
            if (gamePathService.ValidatePath(gamePath))
            {
                try
                {
                    // Make sure replay is on
                    gamePathService.EnableReplay(gamePath);
                    if (localServer == null)
                    {
                        localServer = new LocalServer(gamePath);
                    }

                    /*var address = $"http://{this.IP}:{port}/";
                    // Add the address you want to use
                    listener.Prefixes.Add(address);
                    listener.Start(); // start server (Run application as Administrator!)
                    Console.WriteLine("IP Address -> " + this.IP);
                    // Don't start the game anymore
                    // Process.Start(gamePath + @"\WorldOfWarships.exe");

                    var response = new Thread(ResponseThread);
                    response.Start(); // start the response thread*/
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
            else
            {
                MessageBox.Show(Properties.strings.invalid_path, Properties.strings.error_title,
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

        #region Utils
        private void ResponseThread()
        {
            var rand = new Random();
            while (true)
            {
                Console.WriteLine("Request received");
                var context = listener.GetContext();

                var ARENA = gamePath + @"\replays\tempArenaInfo.json";
                string json = "[]";

                // Grab the file we want and send it
                if (File.Exists(ARENA))
                {
                    var curr = File.GetLastWriteTime(ARENA).ToString();
                    // Get this file and send it as bytes
                    json = File.ReadAllText(ARENA);
                }

                byte[] responseArray = Encoding.UTF8.GetBytes(json); // get the bytes to response
                context.Response.OutputStream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                context.Response.KeepAlive = false; // set the KeepAlive bool to false
                context.Response.Close(); // close the connection
            }
        }

        private string GetIPAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                // It does not have to be valid since there is no real connection
                socket.Connect("8.8.8.8", 65530);
                // This gives the local address that would be used to connect to the specified remote host
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
        #endregion

        private void RS_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener.Close();
            localServer.Stop();
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
