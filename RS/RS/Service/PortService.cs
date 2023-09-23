using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            string netshArguments = $"advfirewall firewall add rule name=\"{name} {port}\" dir=in action=allow protocol=TCP localport={port}";
            Console.WriteLine(netshArguments);
            using (Process netshProcess = new Process())
            {
                netshProcess.StartInfo.FileName = "netsh";
                netshProcess.StartInfo.Arguments = netshArguments;
                netshProcess.StartInfo.UseShellExecute = true;

                netshProcess.Start();
                netshProcess.WaitForExit();
            }
        }
    }
}
