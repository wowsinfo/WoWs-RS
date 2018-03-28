using System;
using System.Threading;
using System.IO;
using System.Drawing;
using WRInfo.Core;
using WRInfo.Resouces;
using WRInfo.Properties;
using Console = Colorful.Console;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WRInfo
{
    class Program
    {
        static int battleCount = 0;
        // Colour list
        static Color[] list = new Color[] { Colour.WBlue, Colour.WGreen, Colour.WOrange, Colour.WPurple, Colour.WRed,
               Colour.WYellow, Color.White, Color.Turquoise, Color.Silver, Color.Orchid, Color.Brown, Color.Azure,
               Color.Pink, Color.Teal, Color.Violet, Color.Lime };

        // Name List
        static List<string> nameList = new List<string> { };
        // Team 0 List
        static List<PlayerInfo> team0List = new List<PlayerInfo> { };
        // Team 1 List
        static List<PlayerInfo> team1List = new List<PlayerInfo> { };

        static string shipJson;
        static dynamic prJson;

        static void Main(string[] args)
        {
            AppLaunch();
            // Save data before app closed
            Settings.Default.Save();
        }

        /// <summary>
        /// Show some animations with fake loading and real update checking
        /// - Checking for saved game directory if no data ask for user
        /// - Checking for saved api data if one is missing, exit
        /// - Checking for WoWs update, if there is one update everything
        /// - Greating user
        /// </summary>
        private static void AppLaunch()
        {
            DataManager dataManager = new DataManager(Directory.GetCurrentDirectory());
            var first = dataManager.FirstLaunch();
            if (first) Reset();

            if (!first)
            {
                // Checking for update and local data
            }

            // Show Menu
            ShowMenu();
            var input = "";
            while (input != "exit")
            {
                Console.Write("> ");
                input = Console.ReadLine();
                switch (input)
                {
                    case "start": StartWRInfo();  break;
                    case "clear": Console.Clear(); break;
                    case "menu": ShowMenu(); break;
                    case "gamestart": StartGame(); break;
                    case "reset": Reset(); break;
                    case "server": PullDataFromAPI(); break;
                    case "path": SetupGamePath(); break;
                    case "language": LanguageSelection(); break;
                    case "help": ShowUsage(); break;
                    case "wows":
                        Process.Start(String.Format(Value.WoWs, Settings.Default.APIDomain)); break;
                    case "facebook": Process.Start(Value.Facebook); break;
                    case "number": Process.Start(Value.Number); break;
                    case "github": Process.Start(Value.Github); break;
                }
            }
        }
        
        #region Menu
        /// <summary>
        /// Show Main menu
        /// </summary>
        private static void ShowMenu()
        {
            Console.Clear();
            Random rnd = new Random();
            Console.WriteAscii(strings.wrinfo, list[rnd.Next(0, list.Length)]);
            Console.WriteLine("Version " + Value.VERSION);
        }

        /// <summary>
        /// Show all possible commands
        /// </summary>
        private static void ShowUsage()
        {
            Console.WriteLine("start\t");
            Console.WriteLine("gamestart\t");
            Console.WriteLine("help\t");
            Console.WriteLine("menu\t");
            Console.WriteLine("language\t");
            Console.WriteLine("server\t");
            Console.WriteLine("path\t");
            Console.WriteLine("clear\t");
            Console.WriteLine("reset\t");
            Console.WriteLine("resetall\t");
            Console.WriteLine("exit\t");

            Console.WriteLine("\n" + strings.website_link);
            Console.WriteLine("wows\t");
            Console.WriteLine("facebook\t");
            Console.WriteLine("number\t");
            Console.WriteLine("github\t");
        }

        /// <summary>
        /// Start World of Warships
        /// </summary>
        private static void StartGame()
        {
            var gamepath = Settings.Default.GamePath;
            Process.Start(gamepath + "/WorldOfWarships.exe");
        }

        /// <summary>
        /// Start parsing data and presenting useful information
        /// </summary>
        private static void StartWRInfo()
        {
            Console.Clear();
            // Remove extra log
            RemoveExtraLog();

            var currPath = Directory.GetCurrentDirectory() + "/data/";
            shipJson = File.ReadAllText(currPath + Value.warshipJson);
            prJson = JsonConvert.DeserializeObject(File.ReadAllText(currPath + Value.personalRating));
            // Best loop ever created
            while (true)
            {
                // Starting parse data from python.log
                var gamepath = Settings.Default.GamePath;
                var logpath = gamepath + "/profile/python.log";
                if (File.Exists(logpath))
                {
                    string python_log = File.ReadAllText(logpath);
                    Regex BattleRegex = new Regex(Value.BattleRegex);
                    MatchCollection Battles = BattleRegex.Matches(python_log);
                    var count = Battles.Count;
                    // No battle is found, Waah
                    if (count == 0) break;
                    if (count != battleCount)
                    {
                        // This is the last battle that we care about
                        python_log = Battles[count - 1].Value;
                        if (battleCount == 0)
                        {
                            // Since this program just opened, it is not sure that if user just played a battle
                            Regex DateRegex = new Regex(Value.DateRegex);
                            Match Date = DateRegex.Match(python_log);
                            var date = String.Join("-", Date.Groups[1].Value.Split('_'));
                            var time = Date.Groups[2].Value;
                            DateTime lastBattledDate = DateTime.Parse(date + " " + time);
                            // Checking date difference in seconds if it is less than 5 mins consider a battle
                            var difference = (DateTime.Now - lastBattledDate).TotalSeconds;

                            // TODO: CHANGE THIS BACK HENRY DONT FORGET
                            if (difference > 300)
                            {
                                CheckPlayerInfo(python_log);
                            }
                        }
                        else
                        {
                            CheckPlayerInfo(python_log);
                        }
                        battleCount = count;
                    }

                    // Rest for 20 seconds WRInfo >_<
                    GC.Collect();
                    Thread.Sleep(20000);
                }
                else
                {
                    // Never play this game or you deleted it just to break this program
                    Console.WriteLine("-_-\n" + strings.no_log);
                    Environment.Exit(1);
                }
            }
        }

        private static void CheckPlayerInfo(string log)
        {
            Console.WriteAscii(strings.player);
            Regex PlayerRegex = new Regex(Value.PlayerRegex);
            MatchCollection Players = PlayerRegex.Matches(log);

            foreach (Match player in Players)
            {
                var name = player.Groups[1].Value;
                var team = player.Groups[2].Value;
                var ship = player.Groups[3].Value;

                // Add reminder for the people you met today
                if (nameList.Contains(name))
                {
                    Console.Write(name + "  ......  !! ");
                }   
                else
                {
                    Console.Write(name);
                    nameList.Add(name);
                }

                // Wah, my user meet me. What should I do??
                if (name == Value.MYNAME) Console.WriteLine(strings.developer);
                else Console.WriteLine();

                // Get ship name and ship id
                Regex ShipRegex = new Regex("\"(\\d+?)\":{\"ship_id_str\":\"" + ship + "\",\"name\":\"(.+?)\"}");
                Match Ship = ShipRegex.Match(shipJson);
                var shipID = Ship.Groups[1].Value;
                var shipName = Ship.Groups[2].Value;

                // Search for player stat
                PlayerInfo info = API.GetPlayerInfo(name, shipID);
                if (info == null)
                {
                    info = new PlayerInfo(name, shipName);
                }
                else
                {
                    info.AddNames(name, shipName);
                    // Calculate personal rating
                    PersonalRating.CalPersonalRating(info, shipID, prJson);
                }

                // Insert into team
                if (team == "0") team0List.Add(info);
                else team1List.Add(info);

                // Guard for wargaming server
                Thread.Sleep(10);
            }

            // Show Team overview
            Console.WriteAscii(strings.team0);
            PlayerInfo.ShowTeamOverview(team0List);
            Console.WriteAscii(strings.team1);
            PlayerInfo.ShowTeamOverview(team1List);
        }

        /// <summary>
        /// Remove python_1.log and so on because they are useless
        /// </summary>
        private static void RemoveExtraLog()
        {
            // TODO: I dont have any extra log nooooow
        }

        /// <summary>
        /// Reset or setup everything
        /// </summary>
        private static void Reset()
        {
            // Reset or setup data
            Console.Clear();
            WelcomeMessage();

            LanguageSelection();
            AnyKeyToContinue();

            SetupGamePath();
            AnyKeyToContinue();

            PullDataFromAPI();
            ShowMenu();
        }
        #endregion

        #region Functiions
        /// <summary>
        /// Press any key to continue this program
        /// </summary>
        private static void AnyKeyToContinue()
        {
            // Waiting for user input to proceed to next level
            Console.Write("> ");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Choose user's perfered language (English, Chinese or Japanese)
        /// </summary>
        private static void LanguageSelection()
        {
            Console.WriteAscii(strings.language, Colour.WYellow);
            Console.WriteLine(strings.only_threelanguages, Color.White);
            Console.Write("1. English\n2. 简体中文\n3. 日本語\n> ");
            var language = Console.ReadLine();
        }

        /// <summary>
        /// Welcome messages and some warnings
        /// </summary>
        private static void WelcomeMessage()
        {
            Console.WriteAscii(strings.welcome, Colour.WBlue);
            Console.WriteLine(strings.introduction, Color.White);
            Console.WriteLine("\n" + strings.agreement + "\n", Color.Red);
            AnyKeyToContinue();
        }

        /// <summary>
        /// Ask user for their game path and save it locally
        /// </summary>
        private static void SetupGamePath()
        {
            var isValid = false;
            Console.WriteAscii(strings.gamepath, Colour.WRed);
            while (!isValid)
            {
                Console.Write("> ");
                var gamePath = Console.ReadLine();
                Console.WriteLine(strings.validate_gamepath + "<<" + gamePath + ">>\n");
                Thread.Sleep(500);
                var preferencePath = gamePath + "/preferences.xml";
                if (File.Exists(gamePath + "/WorldOfWarships.exe") && File.Exists(preferencePath))
                {
                    Console.WriteLine(strings.path_valid, Colour.WGreen);
                    Settings.Default.GamePath = gamePath;
                    isValid = true;

                    var info = DataManager.LoadNameAndServer(preferencePath);
                    Console.WriteAscii("Hi " + info.Name, Color.White);
                }
                else
                {
                    Console.WriteLine(strings.path_notvalid, Colour.WYellow);
                }
            }
        }

        /// <summary>
        /// Download data into /data folder and this is gonna take forever
        /// </summary>
        private static void PullDataFromAPI()
        {
            var isValid = false;
            Console.WriteAscii(strings.api);

            while (!isValid)
            {
                Console.Write("1. RU\t2. EU\t3. NA\t4. ASIA\n> ", Color.White);

                try
                {
                    var server = Convert.ToInt16(Console.ReadLine());
                    if (server < 1 || server > 4)
                    {
                        // Not valid
                        Console.WriteLine(strings.server_notvalid + "\n", Colour.WYellow);
                    }
                    else
                    {
                        // Setup domain
                        var domain = "asia";
                        switch (server)
                        {
                            case 1: domain = "ru"; break;
                            case 2: domain = "eu"; break;
                            case 3: domain = "com"; break;
                        }
                        Settings.Default.APIDomain = domain;
                        isValid = true;
                    }
                }
                catch
                {
                    // Not valid
                    Console.WriteLine(strings.server_notvalid + " -_-\n", Colour.WRed);
                }
            }

            // Reset version and start downloading data
            Console.WriteLine("\n" + strings.pull_api, Color.White);
            Settings.Default.GameVersion = "";
            API.PullData();
        }
    }
    #endregion
}
