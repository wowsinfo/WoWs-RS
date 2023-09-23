using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;

namespace RS.Service
{
    internal class GamePathService
    {
        public GamePathService() { }

        /// <summary>
        /// Validate if the input game path is valid by finding the perferences.xml
        /// </summary>
        /// <param name="gamePath">The game path from the TextBox</param>
        /// <returns>A boolean vakue indicating whther the path is valid</returns>
        public bool ValidatePath(string gamePath)
        {
            // Check if we have a game path
            Console.WriteLine("-> " + gamePath);
            // Only check for perferences.xml (it should work for both steam and normal version)
            return File.Exists(gamePath + @"\preferences.xml");
        }

        /// <summary>
        /// Enable the replay feature of the game. 
        /// Call <seealso cref="ValidatePath(string)"/> first to valid the path.
        /// </summary>
        /// <param name="gamePath">The game path from the TextBox</param>
        public void EnableReplay(string gamePath)
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
        /// Search through the Registry and find the first valid game path
        /// </summary>
        /// <exception cref="Exception">This function can throw in case something is wrong</exception>
        /// <returns>The game path if found</returns>
        public string LocalGamePath()
        {
            RegistryKey currentUser = Registry.CurrentUser;
            RegistryKey uninstall = currentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\");
            string[] publishers = { "Wargaming.net", "Wargaming Group Limited" }; // we don't support the CN or RU server for now

            // loop through all folders
            foreach (var key in uninstall.GetSubKeyNames())
            {
                var folder = uninstall.OpenSubKey(key);
                var publisher = folder.GetValue("Publisher").ToString();
                if (publishers.Contains(publisher))
                {
                    return folder.GetValue("InstallLocation").ToString();
                }
            }

            return null;
        }
    }
}
