using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;

namespace RS.Service
{
    internal class LocalServer
    {
        private HttpListener listener = null;
        private Thread response = null;
        private readonly string gamepath;

        private bool isGettingData = false;

        public LocalServer(string gamePath)
        {
            gamepath = gamePath;
        }

        public void Start(int port)
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://+:{port}/");
            listener.Start();

            response = new Thread(ResponseThread);
            response.Start(); // start the response thread
        }

        public void Stop()
        {
            if (listener != null)
            {
                listener.Close();
                listener = null;
            }

            if (response != null)
            {
                response.Join();
                response = null;
            }
        }

        private void ResponseThread()
        {
            while (true)
            {
                if (isGettingData)
                {
                    return;
                }

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
                    isGettingData = true;
                }

                try
                {
                    var context = listener.GetContext();
                    byte[] responseArray = Encoding.UTF8.GetBytes(json); // get the bytes to response
                    context.Response.OutputStream.Write(responseArray, 0, responseArray.Length); // write bytes to the output stream
                    context.Response.KeepAlive = false; // set the KeepAlive bool to false
                    context.Response.Close(); // close the connection
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                isGettingData = false;
            }
        }
    }
}
