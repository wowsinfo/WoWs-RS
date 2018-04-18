namespace WRInfo
{
    class Value
    {
        static public string VERSION = "1.0.3.1";
        static public string ApplicationID = "e8b46cbc33f242c78725bd6a7562788a";

        static public string PlayerSearch = "https://api.worldofwarships.{0}/wows/account/list/?application_id=" + ApplicationID  + "&search={1}&type=exact&language=en&fields=account_id";
        static public string PlayerShip = "https://api.worldofwarships.{0}/wows/ships/stats/?application_id=" + ApplicationID + "&account_id={1}&language=en&ship_id={2}&fields=pvp.battles%2Cpvp.wins%2Cpvp.frags%2Cpvp.damage_dealt";
        static public string ServerVersion = "https://api.worldofwarships.{0}/wows/encyclopedia/info/?application_id=" + ApplicationID + "&fields=game_version";
        static public string Warships = "https://api.worldofwarships.{0}/wows/encyclopedia/ships/?application_id=" + ApplicationID + "&fields=name%2Cship_id_str&language=en";
        static public string PRJson = "https://wows-numbers.com/personal/rating/expected/json/";

        static public string personalRating = "pr.json";
        static public string warshipJson = "warship.json";

        static public string Github = "https://github.com/HenryQuan/WRInfo-Console-Edition";
        static public string Facebook = "https://www.facebook.com/wowsdevblog/";
        static public string WoWs = "https://worldofwarships.{0}/";
        static public string Number = "https://{0}wows-numbers.com/";
        static public string Today = "https://{0}.warships.today/";
        static public string SeaGroup = "https://sea-group.org/";
        static public string PlayerRanking = "http://maplesyrup.sweet.coocan.jp/wows/ranking/index.html";
        static public string DailyBounce = "https://thedailybounce.net/category/world-of-warships/";

        // Regular expressions
        static public string BattleRegex = @"Avatar.onEnterWorld((.|\n)*?)BattleLogic";
        static public string MapRegex = @"onGeometryMapped\(\) MapName: \d*_(.*)\n";
        static public string PlayerRegex = @"player: Id: \d+ Name: (.+?) TeamId: (\d) ShipName: (\w+?)_\w+";
        static public string DateRegex = @"\[S] \[(.*)?\s(\d*:\d*:\d*)]";
        static public string VersionRegex = @"<span class=""css-truncate-target"">(.+?)<\/span>";
        static public string DownloadRegex = @"<a href=""\/HenryQuan\/WRInfo-Console-Edition\/releases\/download\/(.+?)""";

        // Developer
        static public string MYNAME = "HenryQuan";
        static public string GithubRelease = "https://github.com/HenryQuan/WRInfo-Console-Edition/releases/latest";
        static public string DownloadLink = "https://github.com/HenryQuan/WRInfo-Console-Edition/releases/download/";
    }
}
