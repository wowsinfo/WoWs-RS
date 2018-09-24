using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWs_RS.Core
{
    /// <summary>
    /// DataManager deals with data
    /// </summary>
    class DataManager
    {
        public DataManager()
        {

        }

        #region Data Validation
        /// <summary>
        /// check for all saved data
        /// </summary>
        public void ValidateData()
        {
            CheckPath();
            CheckDomain();
            CheckVersion();
            CheckLanguage();
        }

        /// <summary>
        /// Check for saved path and whether it is valid
        /// If path is lost, ask for user to enter it again
        /// </summary>
        private void CheckPath()
        {
        }

        /// <summary>
        /// Check for saved version and if it does not exist, get it from server
        /// </summary>
        private void CheckVersion()
        {
        }

        /// <summary>
        /// Check for current language. Set it as ENGLISH if it is not found
        /// </summary>
        private void CheckLanguage()
        {
        }

        /// <summary>
        /// Check for current domain. Get it from engine.xml if not found
        /// </summary>
        private void CheckDomain()
        {
        }
        #endregion

        #region Downloading Data / Update
        /// <summary>
        /// Pull data from Wargaming API according to domain
        /// </summary>
        public void PullData()
        {

        }

        /// <summary>
        /// Check for new version for WoWs RS
        /// </summary>
        public void GithubUpdate()
        {

        }

        /// <summary>
        /// Save data to a certain path
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        private void SaveData(string data, string path)
        {

        }
        #endregion

        #region Parsing Data
        public static string ReadArenaInfo()
        {
            return null;
        }
        #endregion
    }
}
