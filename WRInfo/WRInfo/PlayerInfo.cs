using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;

namespace WRInfo
{
    class PlayerInfo
    {
        private string name;
        private string shipname;

        private double battle;
        private double winrate;
        private double avg_damage;
        private double avg_frag;

        private int ability;
        private Color rating;

        // Get essentail data
        public double Avg_frag { get => avg_frag; }
        public double Avg_damage { get => avg_damage; }
        public double Winrate { get => winrate; }
        public double Battle { get => battle; }
        public int Ability { get => ability; }

        public PlayerInfo(int battle, int win, int frag, int damage)
        {
            this.battle = battle;
            this.winrate = GetRoundValue(win, battle) * 100;
            this.avg_damage = GetRoundValue(damage, battle);
            this.avg_frag = GetRoundValue(frag, battle);
        }

        // For players who hide their stat
        public PlayerInfo(string name, string shipname)
        {
            this.name = name;
            this.shipname = shipname;
        }

        /// <summary>
        /// Adding ability point and pr index
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void AddPrAblityPoint(int value, Color rating)
        {
            this.ability = value;
            this.rating = rating;
        }

        /// <summary>
        /// Add name to playerinfo
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shipName"></param>
        public void AddNames(string name, string shipName)
        {
            this.name = name;
            this.shipname = shipName;
        }

        /// <summary>
        /// Print this player's information
        /// </summary>
        public void ShowPlayer()
        {
            Console.WriteLine("{0, 25} {1, 15} {2, 4} {3, 5}% {4, 8}",
                this.name, this.shipname, this.battle, this.winrate, this.ability);
        }

        public static void ShowTeamOverview(List<PlayerInfo> list)
        {
            list.Sort(delegate (PlayerInfo prev, PlayerInfo next)
            {
                return (prev.ability < next.ability) ? 1 : -1;
            });

            foreach (var player in list)
            {
                player.ShowPlayer();
            }
        }

        /// <summary>
        /// Calculate rounded value with 2 decimals
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double GetRoundValue(int a, int b)
        {
            return Math.Round((double)a * 100 / (double)b) / 100;
        }
    }
}
