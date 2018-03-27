using System;
using System.Dynamic;
using System.IO;
using System.Xml;

namespace WRInfo.Core
{
    class DataManager
    {
        private string dir = "";
        private string targetDir = "";

        /// <summary>
        /// Receive a path, usually curr path
        /// </summary>
        /// <param name="path"></param>
        public DataManager(string dir)
        {
            this.dir = dir;
            this.targetDir = dir + "/data/";
        }

        /// <summary>
        /// Check if this is first launch by finding info.json inside data folder
        /// </summary>
        /// <returns></returns>
        public bool FirstLaunch()
        {
            if (!Directory.Exists(this.targetDir))
            {
                // Create a data folder
                Directory.CreateDirectory(this.targetDir);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Loading user's name and current from preferences.xml
        /// </summary>
        /// <param name="path"></param>
        public static Info LoadNameAndServer(string path)
        {
            // Getting user server and name
            var perference = new XmlDocument();
            perference.Load(path);
            var credential = perference.GetElementsByTagName("net_credentials");
            var nodes = credential[0].ChildNodes;
            var name = nodes[0].ChildNodes[2].InnerText.Replace("\t", string.Empty);
            var server = nodes[3].InnerText.Replace("\t", string.Empty);
            return new Info(name, server); ;
        }

        /// <summary>
        /// Download a file with name to path
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void SaveJsonToPathWithName(string data, string path)
        {
            File.WriteAllText(path, data);
        }
    }
}
