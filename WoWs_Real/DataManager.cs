using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WoWs_Real
{
    class DataManager
    {
        // Path
        private static string DocumentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string MyDocument = DocumentFolder + @"\WoWs_Real";
        private static string MyFile = MyDocument + @"\data.txt";
        private static string WOWS = @"\WorldOfWarships.exe";
        private static string LOG = @"\profile\python.log";

        private static string ShipJson = @"";

        // Expected battle count
        private static int battleCount = 1;

        // Regular expression pattern
        private static string dataString = @"Avatar.onEnterWorld([\s\S]*?)BattleLogic";
        private static string playerString = @"Name: (.*?) TeamId: (\d) ShipName: (.*?)_.+";

        // Data index
        public struct DataIndex
        {
            public static int name = 0;
            public static int ship = 1;
            public static int shipId = 0;
            public static int shipName = 1;
        }

        #region Data Validation

        // Check if Data is valid
        public static bool isDataValid()
        {
            return (Directory.Exists(MyDocument) && File.Exists(MyFile));
        }

        // Create new Data
        public static void createData()
        {
            if (!Directory.Exists(MyDocument))
            {
                // Create a new Document Folder
                Directory.CreateDirectory(MyDocument);
            }
            if (!File.Exists(MyFile))
            {
                // Create a Data file
                File.Create(MyFile).Close();
            }
        }

        #endregion

        #region Read Write GamePath

        // Write GamePath to Data
        public static void setGamePath(string path)
        {
            // Double check 
            if (!isDataValid()) createData();
            File.WriteAllText(MyFile, path);
        }

        // Read GamePath to Data
        public static string getGamePath()
        {
            // Double check 
            if (!isDataValid()) createData();
            return File.ReadAllText(MyFile);
        }

        // Read GamePath to Data
        public static string getGameExePath()
        {
            // Double check 
            if (!isDataValid()) createData();
            return File.ReadAllText(MyFile) + WOWS;
        }

        #endregion

        #region Path Validation

        public static bool isPathValid()
        {
            // Check if there is WorldOfWarships.exe inside
            return (File.Exists(getGamePath() + WOWS));
        }

        #endregion

        #region Delete python.log

        public static void deleteLogFile()
        {
            string log = getGamePath() + LOG;
            // See if there is one
            if (File.Exists(log))
            {
                try
                {
                    // Try to delete
                    File.Delete(log);
                }
                catch
                {
                    // Failed, close this software
                    MessageBox.Show(
                        @"WoWs Real could not function properly if you started World of Warships before opening WoWs Real",
                        @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.ExitThread();
                }
            }
        }

        #endregion

        #region Read python.py

        public static string readLogFile()
        {
            string path = getGamePath() + LOG;
            if (path != String.Empty)
            {
                try
                {
                    using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            return String.Empty;
        }

        #endregion

        #region Get Current Player

        public static string getCurrPlayerStr(string data)
        {
            Regex dataRegex = new Regex(dataString);
            MatchCollection dataMatchCollection = dataRegex.Matches(data);
            Console.WriteLine(dataMatchCollection.Count);
            // Only if it is greate than expected value
            if (dataMatchCollection.Count != battleCount) return String.Empty;

            battleCount++;
            // Get string from data
            Console.WriteLine(dataMatchCollection[dataMatchCollection.Count - 1].Groups[1].Value);
            return dataMatchCollection[dataMatchCollection.Count - 1].Groups[1].Value;
        }

        public static string[ , , ] getPlayerInfomation(string data)
        {
            string[ , , ] PlayerInfo = new string[2, 12, 2];

            Regex playerRegex = new Regex(playerString);
            MatchCollection playerMatchCollection = playerRegex.Matches(data);
            int teamOne = -1;
            int teamTwo = -1;
            foreach (Match playerMatch in playerMatchCollection)
            {
                int teamId = 0;
                if (Convert.ToInt16(playerMatch.Groups[2].Value) == 1)
                {
                    teamId = 1;
                    teamOne++;
                    // Team 1
                    PlayerInfo[teamId, teamOne, 0] = playerMatch.Groups[1].Value;
                    PlayerInfo[teamId, teamOne, 1] = playerMatch.Groups[3].Value;
                }
                else
                {
                    teamTwo++;
                    // Team 0
                    PlayerInfo[teamId, teamTwo, 0] = playerMatch.Groups[1].Value;
                    PlayerInfo[teamId, teamTwo, 1] = playerMatch.Groups[3].Value;
                }
            }
            return PlayerInfo;
        }

        #endregion

        #region Get Player Ship Name and ID

        public static void getShipJson()
        {
            string DataFile = Environment.CurrentDirectory + @"\Data\ship.json";
            if (!File.Exists(DataFile))
            {
                // Exit...
                MessageBox.Show(@"Could not find ship.json", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
            else
            {
                // Read it 
                ShipJson = File.ReadAllText(DataFile);
            }
        }

        public static string[] getPlayerShipInfo(string ship)
        {
            string[] ShipInfo = new string[2];
            Regex shipRegex = new Regex(getShipString(ship));
            Match shipMatch = shipRegex.Match(ShipJson);
            ShipInfo[0] = shipMatch.Groups[1].Value;
            ShipInfo[1] = shipMatch.Groups[2].Value;

            return ShipInfo;
        }

        private static string getShipString(string ship)
        {
            return @"""(\d+)"":{""ship_id_str"":""" + ship + @""",""name"":""(.*?)""";
        }

        #endregion
    }
}
