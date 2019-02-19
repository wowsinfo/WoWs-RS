using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using NetFwTypeLib;

namespace RS
{
    public partial class RS : Form
    {
        private string IP = "0.0.0.0";
        private static int port = 8605;
        private string gamePath = "";

        public RS()
        {
            InitializeComponent();

            // Deal with game path
            if (ValidatePath())
            {
                // Make sure replay is on
                EnableReplay();
            }

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

        private void startBtn_Click(object sender, EventArgs e)
        {
            ipLabel.ForeColor = Color.Green;
        }
        #endregion

        #region Utils
        private bool ValidatePath()
        {
            // Check if we have a game path
            gamePath = Properties.Settings.Default.path;
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
                MessageBox.Show("Error", "Failed to add port to firewall. This is the error message.\n" + e.Message);
                Application.ExitThread();
            }
        }
        #endregion
    }
}
