using System;
using System.IO;

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
            if (File.Exists(gamePath + @"\preferences.xml"))
            {
                // Support steam or non steam
                Properties.Settings.Default.path = gamePath;
                Properties.Settings.Default.Save();
                return true;
            }

            return false;
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
    }
}
