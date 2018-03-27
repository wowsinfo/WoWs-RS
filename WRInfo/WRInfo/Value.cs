using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRInfo
{
    class Value
    {
        static public string ApplicationID = "e8b46cbc33f242c78725bd6a7562788a";

        static public string PlayerSearch = "https://api.worldofwarships.{0}/wows/account/list/?application_id=" + ApplicationID  + "&search={1}&type=exact&language=en&fields=account_id";
        static public string PlayerShip = "https://api.worldofwarships.{0}/wows/ships/stats/?application_id=" + ApplicationID + "&account_id={1}&language=en&ship_id={2}&fields=pvp.battles%2Cpvp.wins%2Cpvp.losses%2Cpvp.frags%2Cpvp.damage_dealt";
        static public string ServerVersion = "https://api.worldofwarships.{0}/wows/encyclopedia/info/?application_id=" + ApplicationID + "&fields=game_version";
        static public string Warships = "https://api.worldofwarships.{0}/wows/encyclopedia/ships/?application_id=" + ApplicationID + "&fields=name%2Cship_id%2Cship_id_str&language=en";
        static public string PRJson = "https://wows-numbers.com/personal/rating/expected/json/";

        static public string personalRating = "pr.json";
        static public string warshipJson = "warship.json";
    }
}
