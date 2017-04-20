using System;
using System.IO;
using System.Windows.Forms;

namespace WoWs_Real
{
    class DataManager
    {
        private static string DocumentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string MyDocument = DocumentFolder + @"\WoWs_Real";
        private static string MyFile = MyDocument + @"\data.txt";
        private static string WOWS = @"\WorldOfWarships.exe";
        private static string LOG = @"\profile\python.log";

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

    }
}
