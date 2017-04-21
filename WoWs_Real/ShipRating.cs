using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using Newtonsoft.Json;

namespace WoWs_Real
{
    class ShipRating
    {
        private static dynamic ExpectedJson;

        public static void getExpectedJson()
        {
            string DataFile = Environment.CurrentDirectory + @"\Data\expected.json";
            if (!File.Exists(DataFile))
            {
                // Exit...
                MessageBox.Show(@"Could not find expected.json", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
            else
            {
                // Read it 
                ExpectedJson = JsonConvert.DeserializeObject(File.ReadAllText(DataFile));
            }
        }

        public static double getRating(double battle, string shipId, double frag, double winrate, double damage)
        {
            // Visit http://wows-numbers.com/personal/rating for more information
            double rDmg = damage / (Convert.ToDouble(ExpectedJson.data.shipId.average_damage_dealt));
            double rWins = (winrate * 100) / (Convert.ToDouble(ExpectedJson.data.shipId.win_rate));
            double rFrags = frag / (Convert.ToDouble(ExpectedJson.data.shipId.average_frags));

            double nDMG = max(0, (rDmg - 0.4) / (1 - 0.4));
            double nFrags = max(0, (rFrags - 0.1) / (1 - 0.1));
            double nWins = max(0, (rWins - 0.7) / (1 - 0.7));

            return 700 * nDMG + 300 * nFrags + 150 * nWins;
        }

        private static double max(double p0, double p1)
        {
            // Even swift has a max function...
            return p0 >= p1 ? p0 : p1;
        }

        public static string getComment(double rating)
        {
            if (rating < 750) return "Improvement Needed";
            if (rating < 1100) return "Below Average";
            if (rating < 1350) return "Average";
            if (rating < 1550) return "Good";
            if (rating < 1750) return "Very Good";
            if (rating < 2100) return "Great";
            if (rating < 2450) return "Unicum";
            return "Super Unicum";
        }

        public static Color getRatingColorint(double rating)
        {
            if (rating < 750) return Color.FromRgb(254, 14, 0);
            if (rating < 1100) return Color.FromRgb(254, 121, 3);
            if (rating < 1350) return Color.FromRgb(255, 199, 31);
            if (rating < 1550) return Color.FromRgb(68, 179, 0);
            if (rating < 1750) return Color.FromRgb(94, 139, 47);
            if (rating < 2100) return Color.FromRgb(2, 204, 205);
            if (rating < 2450) return Color.FromRgb(208, 66, 243);
            return Color.FromRgb(106, 13, 197);
        }
    }
}
