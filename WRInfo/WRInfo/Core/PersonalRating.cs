using System;
using System.Drawing;

namespace WRInfo.Core
{
    class PersonalRating
    {
        /// <summary>
        /// Calculate personal rating
        /// </summary>
        /// <param name="info"></param>
        /// <param name="shipID"></param>
        /// <param name="json"></param>
        public static void CalPersonalRating(PlayerInfo info, string shipID, dynamic json)
        {
            // Visit https://wows-numbers.com/personal/rating for more explanation
            var ship = json.data[shipID];
            if (ship == null) return;
            var rDmg = info.Avg_damage / (double)ship.average_damage_dealt;
            var rWins = info.Winrate / (double)ship.win_rate;
            var rFrags = info.Avg_frag / (double)ship.average_frags;

            var nDmg = Math.Max(0, (rDmg - 0.4) / (1 - 0.4));
            var nFrags = Math.Max(0, (rFrags - 0.1) / (1 - 0.1));
            var nWins = Math.Max(0, (rWins - 0.7) / (1 - 0.7));

            int PR = (int)(700 * nDmg + 300 * nFrags + 150 * nWins);
            info.AddPrAblityPoint((int)(PR * info.Battle / 100), GetRatingColour(PR));
        }

        private static Color GetRatingColour(int pr)
        {
            if (pr < 750) return Colour.Bad;
            if (pr < 1100) return Colour.BelowAverage;
            if (pr < 1350) return Colour.Average;
            if (pr < 1550) return Colour.Good;
            if (pr < 1750) return Colour.VeryGood;
            if (pr < 2100) return Colour.Great;
            if (pr < 2450) return Colour.Unicum;
            return Colour.SuperUnicum;
        }
    }
}
