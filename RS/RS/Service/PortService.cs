using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RS.Service
{
    internal class PortService
    {
        public PortService() { }

        /// <summary>
        /// Adds a new port to the firewall with the specified name and port number. 
        /// This function requires the Admin permission.
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="port">The port number</param>
        /// <exception cref="Exception">This can throw lots of exception, treat any as an error.</exception>
        [Obsolete("Use RegisterPort instead to only grant Admin once")]
        public void AddPortToFirewall(string name, int port)
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

        /// <summary>
        /// Register a port to use only for the current instance.
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="port">The port number</param>
        public void RegisterPort(string name, int port)
        {
            string netshArguments = $"advfirewall firewall add rule name=\"{name}\" dir=in action=allow protocol=TCP localport={port}";
            using (Process netshProcess = new Process())
            {
                netshProcess.StartInfo.FileName = "netsh";
                netshProcess.StartInfo.Arguments = netshArguments;
                netshProcess.StartInfo.Verb = "runas";

                netshProcess.Start();
                netshProcess.WaitForExit();
            }
        }


        /// <summary>
        /// Check if the rule has been added.
        /// </summary>
        /// <param name="ruleName">The name of the firewall rule</param>
        /// <returns>True if the rule is found</returns>
        public bool FirewallRuleExists(string ruleName)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = $"advfirewall firewall show rule name=\"{ruleName}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Check if the rule exists in the output
                return output.Contains(ruleName);
            }
        }

        /// <summary>
        /// Get the IP address of the local machine
        /// </summary>
        /// <returns>The IP address (e.g. 192.168.1.100)</returns>
        public string GetIPAddress()
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
    }
}
