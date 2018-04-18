using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using WRInfo.Resouces;

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
            this.targetDir = dir + "\\data\\";
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
        /// Check if any local data are damaged
        /// </summary>
        /// <returns></returns>
        public bool NeedReset()
        {
            // If a essential file is not found
            if (!File.Exists(this.targetDir + Value.personalRating) || !File.Exists(this.targetDir + Value.warshipJson))
                return true;
            else return false;
        }

        /// <summary>
        /// Whether a new version is available
        /// </summary>
        public static void CheckForGithubUpdate()
        {
            try
            {
                using (var client = new WebClient())
                {
                    string Github = client.DownloadString(Value.GithubRelease);
                    Regex VersionRegex = new Regex(Value.VersionRegex);
                    Match Version = VersionRegex.Match(Github);
                    var GithubVersion = Version.Groups[1].Value;
                    if (String.Compare(GithubVersion, Value.VERSION) > 0)
                    {
                        // Update available
                        Regex DownloadRegex = new Regex(Value.DownloadRegex);
                        Match Download = DownloadRegex.Match(Github);
                        Process.Start(Value.DownloadLink + Download.Groups[1].Value);
                        Console.WriteLine(strings.has_update + GithubVersion);
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DataManager::CheckForGithubUpdate\n" + e.Message);
                Console.ReadKey();
            }
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

        /// <summary>
        /// Read log file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadLogFile(string path)
        {
            var log = "";
            try
            {
                using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    log = new StreamReader(stream).ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DataManager::ReadLogFile\n" + e);
                Console.ReadKey();
                Environment.Exit(1);
            }
            return log;
        }
    }
}
