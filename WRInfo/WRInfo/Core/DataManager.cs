using System;
using System.IO;

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
            if (!Directory.Exists(this.targetDir)) return true;
            else return false;
        }
    }
}
