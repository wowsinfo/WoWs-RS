using System.Net;
using System;
using System.Threading;
using System.Text;
using NetFwTypeLib;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;

namespace winserver
{
    class Program
    {
        static HttpListener listener = new HttpListener();

        static string lastEdited = "";
        static string gamePath = "";
        static int battle = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Github: https://github.com/HenryQuan/winserver");

            // Deal with game path
            ValidatePath();
            // Make sure replay is on
            EnableReplay();

            int port = 8605;
            string ip = GetIPAddress();

            // Add port first for foreign access
            AddPortToFirewall("WoWs Info", port);

            var address = $"http://{ip}:{port}/";
            // Add the address you want to use
            listener.Prefixes.Add(address);
            listener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("IP Address -> " + ip);
            // Start the game
            Process.Start(gamePath + @"\WorldOfWarships.exe");

            var response = new Thread(ResponseThread);
            response.Start(); // start the response thread
        }

        /// <summary>
        /// Valid game path and always ask user to add it if not valid
        /// </summary>
        static void ValidatePath()
        {
            // Check if we have a game path
            gamePath = Properties.Settings.Default.path;
            Console.WriteLine("-> " + gamePath);
            if (gamePath == "")
            {
                var path = "";
                while (!File.Exists(path + @"\WorldOfWarships.exe"))
                {
                    Console.Write("Paste your game path here: ");
                    path = Console.ReadLine();
                }
                // Update path
                gamePath = path;
                Properties.Settings.Default.path = path;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Check if replay is enabled and enable it if not
        /// </summary>
        static void EnableReplay()
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

        /// <summary>
        /// Responce to request
        /// Reference: https://www.codeproject.com/Tips/485182/Create-a-local-server-in-Csharp
        /// </summary>
        static void ResponseThread()
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

                    if (lastEdited == "" || lastEdited != curr)
                    {
                        battle++;
                        Console.WriteLine("Battle " + battle);
                        // Remind user to get a break if played fo too long
                        if (battle % 6 == 0)
                        {
                            Console.WriteLine("Maybe it is time to take a break");
                        }

                        lastEdited = curr;
                        // Get this file and send it as bytes
                        json = File.ReadAllText(ARENA);
                    }
                }
                byte[] responseArray = Encoding.UTF8.GetBytes(json); // get the bytes to response
                context.Response.OutputStream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                context.Response.KeepAlive = false; // set the KeepAlive bool to false
                context.Response.Close(); // close the connection
            }
        }

        /// <summary>
        /// Get your local ip address
        /// Reference: https://stackoverflow.com/questions/6803073/get-local-ip-address
        /// </summary>
        /// <returns>local ip</returns>
        static string GetIPAddress()
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

        /// <summary>
        /// Add port to windows firewall
        /// Reference: https://social.msdn.microsoft.com/Forums/vstudio/en-US/a3e390d1-4383-4f23-bad9-b725bef33499/add-firewall-rule-programatically?forum=wcf
        /// </summary>
        static void AddPortToFirewall(string name, int port)
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
                Console.WriteLine("Failed to add port to firewall. This is the error message.\n");
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease feel free to open an issue to discuss this it with me.");
                Process.Start("https://github.com/HenryQuan/winserver");
            }
            
        }
    }
}
