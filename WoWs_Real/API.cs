using System;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace WoWs_Real
{
    class API
    {
        public struct DataIndex
        {
            public static int battle = 0;
            public static int id = 1;
            public static int winrate = 2;
            public static int frag = 3;
            public static int damage = 4;
        }

        // Regular Expression Stirng
        private static string getIdString(string name)
        {
            return @"""nickname"":""" + name + @""",""account_id"":(\d*)";
        }

        // Make PlayerAPI string
        private static string getPlayerAPI(string name)
        {
            return @"https://api.worldofwarships.asia/wows/account/list/?application_id=e8b46cbc33f242c78725bd6a7562788a&search=" + name;
        }

        // Make PlayerShipAPI string
        private static string getPlayerShipAPI(string player, string ship)
        {
            return @"https://api.worldofwarships.asia/wows/ships/stats/?application_id=e8b46cbc33f242c78725bd6a7562788a&account_id=" + player + @"&ship_id=" + ship + @"&fields=pvp.battles%2Cpvp.damage_dealt%2Cpvp.frags%2Cpvp.wins";
        }

        // Get Player ID from API
        private static string getPlayerId(string name)
        {
            string dataJson;
            using (var web = new WebClient())
            {
                web.Proxy = null;
                dataJson = web.DownloadString(getPlayerAPI(name));
            }

            if (dataJson.Contains(@"""status"":""ok"""))
            {
                // This is valid
                Regex idRegex = new Regex(getIdString(name));
                Match idMatch = idRegex.Match(dataJson);
                return idMatch.Groups[1].Value;
            }
            return String.Empty;
        }

        public static string[] getPlayerShipInfo(string name, string ship)
        {
            dynamic dataJson;
            string dataString = @"";
            string[] ShipInfo = new string[5];
            string playerId = @"";

            using (var web = new WebClient())
            {
                web.Proxy = null;
                playerId = getPlayerId(name);
                dataString = web.DownloadString(getPlayerShipAPI(playerId, ship));
            }

            if (dataString.Contains(@"battles"))
            {
                // This is valid
                dataJson = JObject.Parse(dataString);
                ShipInfo[0] = dataJson["data"][playerId][0]["pvp"]["battles"];
                ShipInfo[1] = ship;
                double winrate = Convert.ToDouble(dataJson["data"][playerId][0]["pvp"]["wins"]) / Convert.ToDouble(dataJson["data"][playerId][0]["pvp"]["battles"]);
                ShipInfo[2] = winrate.ToString();
                ShipInfo[3] = dataJson["data"][playerId][0]["pvp"]["frags"];
                ShipInfo[4] = dataJson["data"][playerId][0]["pvp"]["damage_dealt"];
                return ShipInfo;
            }
            return new string[0];
        }
    }
}
