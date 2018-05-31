using Colorful;
using System;
using System.Collections.Generic;
using System.Drawing;
using WRInfo.Resouces;
using Console = Colorful.Console;

namespace WRInfo
{
    class PlayerInfo
    {
        private string name;
        private string shipname;
        private string clanName;

        private double battle;
        private double winrate;
        private double avg_damage;
        private double avg_frag;
        private double win;

        private int ability;
        private Color rating = Color.Gray;
        private Color WeMeetAgain = Color.White;

        // Get essentail data
        public double Avg_frag { get => avg_frag; }
        public double Avg_damage { get => avg_damage; }
        public double Winrate { get => winrate; }
        public double Battle { get => battle; }

        public PlayerInfo(int battle, int win, int frag, int damage)
        {
            this.battle = battle;
            this.win = win;
            this.winrate = GetWinRate(win, battle);
            this.avg_damage = GetRoundValue(damage, battle);
            this.avg_frag = GetRoundValue(frag, battle);
        }

        public PlayerInfo(int battle, int win, int frag, int damage, String clan)
        {
            this.battle = battle;
            this.win = win;
            this.winrate = GetWinRate(win, battle);
            this.avg_damage = GetRoundValue(damage, battle);
            this.avg_frag = GetRoundValue(frag, battle);
            this.clanName = clan;
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
            var clanTag = this.clanName.Length < 2 ? "" : String.Format("[{0}]", this.clanName);
            this.name = clanTag + name;
            this.shipname = shipName;
        }

        /// <summary>
        /// Indicate player you meet today
        /// </summary>
        public void AddWeMeetAgain()
        {
            this.WeMeetAgain = Colour.WYellow;
        }

        /// <summary>
        /// Print this player's information
        /// </summary>
        public void ShowPlayer(bool team0)
        {
            if (team0)
            {
                Console.Write("{0, 25}  ", this.name, this.WeMeetAgain);
                Console.Write("{0, 20}  ", this.shipname, this.rating);
                Console.Write("{0, 5}  {1, 4}%  {2, 6}", this.battle, this.winrate, this.ability, Color.White);
            }
            else
            {
                Console.Write("{0, -6}  %{1, -4}  {2, -5}", this.ability, this.winrate, this.battle, Color.White);
                Console.Write("  {0, -20}", this.shipname, this.rating);
                Console.Write("  {0, -25}", this.name, this.WeMeetAgain);
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
            int team0Count = team0.Count - 2;
            int team1Count = team1.Count - 2;
            double team0Win = 0, team0Total = 0, team0AP = 0;
            double team1Win = 0, team1Total = 0, team1AP = 0;
            if (team0Count == team1Count)
            {
                Console.WriteAscii("Team 0       Team 1", Colour.WBlue);
                // What each value means?
                Console.WriteLine("{0} - {1} - {2} - {3} - {4}\n", strings.player_name, strings.ship_name,
                    strings.battle_count, strings.win_rate, strings.ability_point);
                for (var i = 0; i < team0.Count; i++)
                {
                    var player0 = team0[i];
                    var player1 = team1[i];

                    // Ignore the best / worst player and get average of other players
                    if (i > 0 && i < team0.Count - 1)
                    { 
                        if (player0.battle == 0)
                        {
                            // Ignore player 0
                            CalAvgData(ref team1Win, ref team1Total, ref team1AP, player1);
                            team0Count--;
                        }
                        else if (player1.battle == 0)
                        {
                            // Ignore player 1
                            CalAvgData(ref team0Win, ref team0Total, ref team0AP, player0);
                            team1Count--;
                        }
                        else
                        {
                            // Add both players
                            CalAvgData(ref team1Win, ref team1Total, ref team1AP, player1);
                            CalAvgData(ref team0Win, ref team0Total, ref team0AP, player0);
                        }
                    }

                    player0.ShowPlayer(true);
                    Console.Write(" | ", Color.White);
                    player1.ShowPlayer(false);
                    Console.WriteLine("\n");
                }
                // Print extra information
                Console.WriteLine("Team 0 - {0, 3}% - {1}",
                    GetWinRate(team0Win, team0Total), 
                    GetRoundValue(team0AP, team0Count), Colour.WYellow);
                Console.WriteLine("Team 1 - {0, 3}% - {1}",
                    GetWinRate(team1Win, team1Total), 
                    GetRoundValue(team1AP, team1Count), Colour.WYellow);
            } 
            else
            {
                // Training room or Operation
                Console.WriteAscii("? ? ?", Colour.WBlue);
                ShowTeamOverviewSmall(team0);
                Console.WriteLine("");
                ShowTeamOverviewSmall(team1);
            }

            // Show what colour means
            Console.WriteLine("\nPersonal Rating\n" + strings.colour_meaning + "\n");
            Console.WriteLine(strings.grey_player, Color.Gray);
        }

        private static void ShowTeamOverviewSmall(List<PlayerInfo> list)
        {
            list.Sort(delegate (PlayerInfo prev, PlayerInfo next)
            {
                return (prev.ability < next.ability) ? 1 : -1;
            });
            foreach (var player in list)
            {
                player.ShowPlayer(true);
                Console.WriteLine("\n");
            }
        }

        /// <summary>
        /// Calculate team average data
        /// </summary>
        /// <param name="Win"></param>
        /// <param name="Total"></param>
        /// <param name="AP"></param>
        /// <param name="Player"></param>
        private static void CalAvgData(ref double Win, ref double Total, ref double AP, PlayerInfo Player)
        {
            Win += Player.win;
            Total += Player.battle;
            AP += Player.ability;
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

        private static double GetWinRate(double win, double battle)
        {
            return Math.Round(win * 1000 / battle) / 10;
        }
    }
}
