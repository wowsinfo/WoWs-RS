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
using NetFwTypeLib;

namespace RS
{
    public partial class RS : Form
    {
        static HttpListener listener = new HttpListener();

        private string IP = "0.0.0.0";
        private static int port = 8605;
        private string gamePath = "";

        public RS()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.lang);
            pathLabel.Text = Properties.strings.path;
            startBtn.Text = Properties.strings.start;
            aboutMenu.Text = Properties.strings.about;
            checkForUpdateToolStripMenuItem.Text = Properties.strings.check_for_update;
            languageToolStripMenuItem.Text = Properties.strings.language;

            pathBox.Text = Properties.Settings.Default.path;

            // Get current IP address
            this.IP = GetIPAddress();
            ipLabel.Text = this.IP;

            AddPortToFirewall("WoWs RS", port);
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
            if (ValidatePath())
            {
                // Make sure replay is on
                EnableReplay();

                var address = $"http://{this.IP}:{port}/";
                // Add the address you want to use
                listener.Prefixes.Add(address);
                listener.Start(); // start server (Run application as Administrator!)
                Console.WriteLine("IP Address -> " + this.IP);
                // Start the game
                Process.Start(gamePath + @"\WorldOfWarships.exe");

                var response = new Thread(ResponseThread);
                response.Start(); // start the response thread

                ipLabel.ForeColor = Color.Green;
                startBtn.Enabled = false;
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

        private bool ValidatePath()
        {
            // Check if we have a game path
            gamePath = pathBox.Text;
            Console.WriteLine("-> " + gamePath);
            if (File.Exists(gamePath + @"\WorldOfWarships.exe"))
            {
                Properties.Settings.Default.path = gamePath;
                Properties.Settings.Default.Save();
                return true;
            }
            return false;
        }

        private void EnableReplay()
        {
            string path = gamePath + @"\preferences.xml";
            var replay = false;
            var scriptLine = 0;

            // XmlDocument is so bad... I have to change it manually
            var xml = File.ReadAllLines(path);
            for (var i = 0; i < xml.Length; i++)
            {
                var line = xml[i];
                if (line.Contains("<isReplayEnabled>"))
                {
                    // Replay is enabled
                    replay = true;
                    break;
                }
                else if (line.Contains("</scriptsPreferences>"))
                {
                    // Record this line to add our script later
                    scriptLine = i;
                    break;
                }
            }

            if (!replay)
            {
                // Enable replay
                xml[scriptLine] = "		<isReplayEnabled>	true	</isReplayEnabled>\n" + xml[scriptLine];
                File.WriteAllLines(path, xml);
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

        private void AddPortToFirewall(string name, int port)
        {
            try
            {
                Type TicfMgr = Type.GetTypeFromProgID("HNetCfg.FwMgr");
                INetFwMgr icfMgr = (INetFwMgr)Activator.CreateInstance(TicfMgr);

                // add a new port
                Type TportClass = Type.GetTypeFromProgID("HNetCfg.FWOpenPort");
                INetFwOpenPort portClass = (INetFwOpenPort)Activator.CreateInstance(TportClass);

                // Get the current profile
                INetFwProfile profile = icfMgr.LocalPolicy.CurrentProfile;

                // Set the port properties
                portClass.Scope = NetFwTypeLib.NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
                portClass.Enabled = true;
                portClass.Protocol = NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                // WoWs Info - 8605
                portClass.Name = name;
                portClass.Port = port;

                // Add the port to the ICF Permissions List
                profile.GloballyOpenPorts.Add(portClass);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Properties.strings.error_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
        }
        #endregion

        private void RS_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener.Close();
            Application.ExitThread();
        }
    }
}
