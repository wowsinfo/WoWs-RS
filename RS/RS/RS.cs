using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace RS
{
    public partial class RS : Form
    {
        private string IP = "0.0.0.0";
        private static int port = 8605; 

        public RS()
        {
            InitializeComponent();

            // Get current IP address
            this.IP = GetIPAddress();
            ipLabel.Text = this.IP;
        }

        private void githubMenu_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/HenryQuan/WoWs-RS");
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

        private void startBtn_Click(object sender, EventArgs e)
        {
            ipLabel.ForeColor = Color.Green;
        }
    }
}
