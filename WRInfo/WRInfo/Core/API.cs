using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WRInfo.Properties;
using WRInfo.Resouces;
using WRInfo.Core;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WRInfo
{
    class API
    {
        private static string CurrDir = Directory.GetCurrentDirectory();

        /// <summary>
        /// Download and format data properly and save it under data folder
        /// </summary>
        public static void PullData()
        {
            if (CheckGameUpdate())
            {
                GetWarships();
                GetPersonalRating();
            }
        }

        /// <summary>
        /// Request and save curr game version for future update checking
        /// </summary>
        private static bool CheckGameUpdate()
        {
            var hasUpdate = false;
            // Checking for update
            using (var client = new WebClient())
            {
                // In case something fails, make a backup first
                try
                {
                    var data = client.DownloadString(String.Format(Value.ServerVersion, Settings.Default.APIDomain));
                    dynamic json = JsonConvert.DeserializeObject(data);
                    if (json.status == "ok")
                    {
                        // Data is all right
                        var version = (String)json.data.game_version;
                        var currVersion = Settings.Default.GameVersion;
                        if (currVersion != version)
                        {
                            // Update version
                            Settings.Default.GameVersion = version;
                            hasUpdate = true;
                            Console.WriteLine((currVersion == "" ? "NULL" : currVersion) + " -> " + version);
                        }
                        else
                        {
                            Console.WriteLine(strings.no_update);
                        }
                    }
                }
                catch (Exception e)
                {
                    FatalError(e, "API::CheckGameUpdate");
                }
            }
            return hasUpdate;
        }

        /// <summary>
        /// Download warships data x 3
        /// </summary>
        private static void GetWarships()
        {
            Console.WriteLine(strings.download_warships);
            using (var client = new WebClient())
            {
                try
                {
                    var api = String.Format(Value.Warships, Settings.Default.APIDomain);
                    var data = client.DownloadString(api);
                    Console.WriteLine("P1...");

                    dynamic json = JsonConvert.DeserializeObject(data);
                    if (json.status == "ok")
                    {
                        JObject warship = JObject.Parse(data);
                        var pageCount = (int) json.meta.page_total;
                        for (var i = 2; i <= pageCount; i++)
                        {
                            data = client.DownloadString(api + "&page_no=" + i);
                            warship.Merge(JObject.Parse(data), new JsonMergeSettings
                            {
                                // Avoid duplicates
                                MergeArrayHandling = MergeArrayHandling.Union
                            });
                            Console.WriteLine("P" + i + "...");
                        }
                        DataManager.SaveJsonToPathWithName(warship.ToString(), CurrDir + "/" + Value.warshipJson);
                        Console.WriteLine(strings.complete);
                    }
                }
                catch (Exception e)
                {
                    FatalError(e, "API::GetWarships");
                }
            }
        }

        /// <summary>
        /// Download personal rating json
        /// </summary>
        private static void GetPersonalRating()
        {
            Console.WriteLine(strings.download_pr);
            using (var client = new WebClient())
            {
                try
                {
                    var data = client.DownloadString(Value.PRJson);
                    DataManager.SaveJsonToPathWithName(data, CurrDir + "/" + Value.personalRating);
                    Console.WriteLine(strings.complete);
                }
                catch (Exception e)
                {
                    FatalError(e, "API::GetPersonalRating");
                }
            }
        }

        /// <summary>
        /// Deal with exception. A message will be print and application will be closed
        /// </summary>
        /// <param name="e"></param>
        private static void FatalError(Exception e, string funcName)
        {
            Console.WriteLine(funcName + "\n" + e.Message);
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
