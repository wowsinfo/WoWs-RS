using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RS.Service
{
    internal class LocalServer
    {
        private TcpListener listener = null;
        private Thread response = null;
        private readonly string gamepath;

        public LocalServer(string gamePath) {
            gamepath = gamePath;
        }

        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            response = new Thread(ResponseThread);
            response.Start(); // start the response thread
        }

        public void Stop()
        {
            if (listener != null)
            {
                listener.Stop();
                listener = null;

                response.Join();
                response = null;
            }
        }

        private void ResponseThread()
        {
            var rand = new Random();
            while (true)
            {
                Console.WriteLine("Request received");

                var ARENA = gamepath + @"\replays\tempArenaInfo.json";
                string json = "[]";

                // Grab the file we want and send it
                if (File.Exists(ARENA))
                {
                    /*var curr = File.GetLastWriteTime(ARENA).ToString();*/
                    // Get this file and send it as bytes
                    json = File.ReadAllText(ARENA);
                    Console.WriteLine("Reading the Arena file");
                }

                // return
                try
                {
                    var client = listener.AcceptTcpClient();
                    var stream = client.GetStream();
                    byte[] responseArray = Encoding.UTF8.GetBytes(json); // get the bytes to response
                    stream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                    stream.Flush();
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }
    }
}
