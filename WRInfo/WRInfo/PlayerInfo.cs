using Colorful;
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
        private Color rating = Color.Gray;

        // Get essentail data
        public double Avg_frag { get => avg_frag; }
        public double Avg_damage { get => avg_damage; }
        public double Winrate { get => winrate; }
        public double Battle { get => battle; }
        public int Ability { get => ability; }

        public PlayerInfo(int battle, int win, int frag, int damage)
        {
            this.battle = battle;
            this.winrate = GetWinRate(win, battle);
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
        public void ShowPlayer(bool team0)
        {
            if (team0)
            {
                Console.Write("{0, 25}  ", this.name, Color.White);
                Console.Write("{0, 15}  ", this.shipname, this.rating);
                Console.Write("{0, 5}  {1, 4}%  {2, 6}", this.battle, this.winrate, this.ability, Color.White);
            }
            else
            {
                Console.Write("{0, -6}  %{1, -4}  {2, -5}", this.ability, this.winrate, this.battle, Color.White);
                Console.Write("  {0, -15}", this.shipname, this.rating);
                Console.Write("  {0, -25}", this.name, Color.White);
            }
        }

        /// <summary>
        /// Sort list and print out all players
        /// </summary>
        /// <param name="list"></param>
        public static void ShowTeamOverview(List<PlayerInfo> team0, List<PlayerInfo> team1)
        {
            team0.Sort(delegate (PlayerInfo prev, PlayerInfo next)
            {
                return (prev.ability < next.ability) ? 1 : -1;
            });
            team1.Sort(delegate (PlayerInfo prev, PlayerInfo next)
            {
                return (prev.ability < next.ability) ? 1 : -1;
            });

            // Calculate team 0 and team 1 average win rate and ability point
            int team0Count = team0.Count;
            int team1Count = team1.Count;
            double team0Win = 0, team1Win = 0, team0AP = 0, team1AP = 0;
            if (team0Count == team1Count)
            {
                // This is normal mode
                for (var i = 0; i < team0.Count; i++)
                {
                    var player0 = team0[i];
                    var player1 = team1[i];

                    // This player hides stat
                    if (player0.ability == 0) team0Count--;
                    else if (player1.ability == 0) team1Count--;
                    else
                    {
                        team0Win += player0.Winrate;
                        team1Win += player1.Winrate;
                        team0AP += player0.Ability;
                        team1AP += player1.Ability;
                    }

                    player0.ShowPlayer(true);
                    Console.Write(" | ", Color.White);
                    player1.ShowPlayer(false);
                    Console.WriteLine("\n");
                }
                // Print extra information
                Console.WriteLine("Team 0 - {0, 4}% - {1}", 
                    GetRoundValue(team0Win, team0Count), GetRoundValue(team0AP, team0Count), Colour.WYellow);
                Console.WriteLine("Team 1 - {0, 4}% - {1}", 
                    GetRoundValue(team1Win, team0Count), GetRoundValue(team1AP, team0Count), Colour.WYellow);
            } 
            else
            {
                // Training room or Operation
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

        private static double GetRoundValue(double a, double b)
        {
            return Math.Round(a * 100 / b) / 100;
        }

        /// <summary>
        /// Calculate winrate with 1 decimal
        /// </summary>
        /// <param name="win"></param>
        /// <param name="battle"></param>
        /// <returns></returns>
        private static double GetWinRate(int win, int battle)
        {
            return Math.Round((double)win * 1000 / (double)battle) / 10;
        }
    }
}
