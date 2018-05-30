using System;
using System.Threading;
using System.IO;
using System.Drawing;
using WRInfo.Resouces;
using WRInfo.Properties;
using Console = Colorful.Console;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;

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
            // Update language
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Language);
            AppLaunch();
            Settings.Default.Save();
        }

        /// <summary>
        /// Save data before software closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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
                API.PullData();

                // Reset if data is damaged
                if (dataManager.NeedReset()) PullDataFromAPI();

                // Check for program update
                DataManager.CheckForGithubUpdate();

                // Check for game path
                if (Settings.Default.GamePath == "") SetupGamePath();
            }

            // Show Menu
            ShowMenu();
            var input = "";
            while (input != "exit")
            {
                Console.Write("WRInfo> ");
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
                    default:
                        ShowWebsite(input); break;
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
            Console.WriteLine(Settings.Default.GamePath);
            Console.WriteLine("v" + Value.VERSION);

            Console.WriteLine("\n" + strings.wows_info);
            Console.WriteLine("\n" + strings.enter_help);
        }

        /// <summary>
        /// Show website here
        /// </summary>
        /// <param name="input"></param>
        private static void ShowWebsite(string input)
        {
            var url = "";
            var domain = Settings.Default.APIDomain;
            switch (input)
            {
                case "wows": url = String.Format(Value.WoWs, domain); break;
                case "facebook": url = Value.Facebook; break;
                case "wows number":
                    if (domain == "eu") domain = "";
                    url = String.Format(Value.Number, domain + "."); break;
                case "wows today": url = String.Format(Value.Today, domain); break;
                case "player ranking": url = Value.PlayerRanking; break;
                case "sea group": url = Value.SeaGroup; break;
                case "daily bounce": url = Value.DailyBounce; break;
                case "github": url = Value.Github; break;
            }
            // Open browser
            if (url != "") Process.Start(url);
        }

        /// <summary>
        /// Show all possible commands
        /// </summary>
        private static void ShowUsage()
        {
            Console.WriteLine("start\t\t" + strings.help_start);
            Console.WriteLine("gamestart\t" + strings.help_gamestart);
            Console.WriteLine("help\t\t" + strings.help_help);
            Console.WriteLine("menu\t\t" + strings.help_menu);
            Console.WriteLine("language\t" + strings.help_language);
            Console.WriteLine("server\t\t" + strings.help_server);
            Console.WriteLine("path\t\t" + strings.help_path);
            Console.WriteLine("clear\t\t" + strings.help_clear);
            Console.WriteLine("reset\t\t" + strings.help_reset);
            Console.WriteLine("exit\t\t" + strings.help_exit);

            Console.WriteLine("\n" + strings.website_link);
            Console.WriteLine("wows\t\t");
            Console.WriteLine("facebook\t\t");
            Console.WriteLine("wows number\t\t", Color.LightBlue);
            Console.WriteLine("wows today\t\t", Color.Red);
            Console.WriteLine("player ranking\t\t");
            Console.WriteLine("sea group\t\t");
            Console.WriteLine("daily bounce\t\t");
            Console.WriteLine("github\t\t", Color.LightPink);
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
                    string python_log = DataManager.ReadLogFile(logpath);
                    Regex BattleRegex = new Regex(Value.BattleRegex);
                    MatchCollection Battles = BattleRegex.Matches(python_log);

                    var count = Battles.Count;
                    if (count == 0)
                    {
                        // No battle is found, Waah
                        Console.Clear();
                        Console.Write("Standby ...");
                    }
                    else if (count != battleCount)
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

                            if (difference > 600)
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
                }
                else
                {
                    // Never play this game or you deleted it just to break this program
                    Console.Clear();
                    Console.Write(strings.no_log);
                }

                // Rest for 15 seconds WRInfo >_<
                GC.Collect();
                Thread.Sleep(15000);
            }
        }

        private static void CheckPlayerInfo(string log)
        {
            Console.Clear();
            team0List = new List<PlayerInfo> { };
            team1List = new List<PlayerInfo> { };
            Console.WriteAscii(strings.player, Colour.WYellow);
            Regex PlayerRegex = new Regex(Value.PlayerRegex);
            MatchCollection Players = PlayerRegex.Matches(log);

            foreach (Match player in Players)
            {
                var name = player.Groups[1].Value;
                var team = player.Groups[2].Value;
                var ship = player.Groups[3].Value;

                // Add reminder for the people you met today
                var metBefore = false;
                if (nameList.Contains(name))
                {
                    Console.Write(name + "  ......  !! ", Color.Gray);
                    metBefore = true;
                }   
                else
                {
                    Console.Write(name, Color.Gray);
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

                if (metBefore) info.AddWeMeetAgain();

                // Insert into team
                if (team == "0") team0List.Add(info);
                else team1List.Add(info);

                // Guard for wargaming server
                Thread.Sleep(10);
            }

            // Show Team overview
            PlayerInfo.ShowTeamOverview(team0List, team1List);
        }

        /// <summary>
        /// Remove python_1.log and so on because they are useless
        /// </summary>
        private static void RemoveExtraLog()
        {
            var logPath = Settings.Default.GamePath + "/profile/";
            var i = 1;
            while (true)
            {
                var file = logPath + "python_" + i++ + ".log";
                if (File.Exists(file))
                {
                    // Remove this file
                    File.Delete(file);
                }
                else break;
            }
        }

        /// <summary>
        /// Reset or setup everything
        /// </summary>
        private static void Reset()
        {
            // Reset or setup data
            Console.Clear();

            LanguageSelection();
            WelcomeMessage();
            SetupGamePath();
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
            Console.Write("WRInfo> ");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Choose user's perfered language (English, Chinese or Japanese)
        /// </summary>
        private static void LanguageSelection()
        {
            var isValid = false;
            Console.WriteAscii(strings.language, Colour.WYellow);
            while (!isValid)
            {
                Console.WriteLine(strings.only_threelanguages, Color.White);
                Console.Write("1. English\n2. 简体中文\n3. 日本語\nWRInfo> ");
                try
                {
                    var selection = Convert.ToInt16(Console.ReadLine());
                    if (selection < 1 || selection > 4)
                    {
                        // Not valid
                        Console.WriteLine(strings.server_notvalid + "\n", Colour.WYellow);
                    }
                    else
                    {
                        // Setup domain
                        var lang = "en";
                        switch (selection)
                        {
                            case 1: lang = "en"; break;
                            case 2: lang = "zh"; break;
                            case 3: lang = "ja"; break;
                        }
                        Settings.Default.Language = lang;
                        Settings.Default.Save();
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
                        isValid = true;
                    }
                }
                catch
                {
                    // Not valid
                    Console.WriteLine(strings.server_notvalid + " -_-\n", Colour.WRed);
                }
            }
            
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
            Console.WriteLine(strings.enter_gamepath);
            // Open my computer for user
            Process.Start("explorer.exe", "");
            while (!isValid)
            {
                Console.Write("WRInfo> ");
                var gamePath = Console.ReadLine();
                Console.WriteLine(strings.validate_gamepath + "<<" + gamePath + ">>\n");
                Thread.Sleep(500);
                var preferencePath = gamePath + "/preferences.xml";
                if (File.Exists(gamePath + "/WorldOfWarships.exe") && File.Exists(preferencePath))
                {
                    Console.WriteLine(strings.path_valid);
                    Settings.Default.GamePath = gamePath;
                    Settings.Default.Save();
                    isValid = true;
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
            Console.WriteAscii(strings.api, Color.White);

            while (!isValid)
            {
                Console.Write("1. RU\t2. EU\t3. NA\t4. ASIA\nWRINfo> ", Color.White);

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
