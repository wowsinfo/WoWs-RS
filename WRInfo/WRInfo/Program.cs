using System;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Drawing;
using WRInfo.Core;
using WRInfo.Resouces;
using WRInfo.Properties;
using Console = Colorful.Console;
using System.Diagnostics;

namespace WRInfo
{
    class Program
    {
        /// <summary>
        /// How many battle user has played
        /// Show some kind reminder for user
        /// </summary>
        static int battleCount = 0;
        static Color[] list = new Color[] { Colour.WBlue, Colour.WGreen, Colour.WOrange, Colour.WPurple, Colour.WRed,
               Colour.WYellow, Color.White, Color.Turquoise, Color.Silver, Color.Orchid, Color.Brown, Color.Azure,
               Color.Pink, Color.Teal, Color.Violet, Color.Lime };

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
        static void AppLaunch()
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
                    case "clear": Console.Clear(); break;
                    case "menu": ShowMenu(); break;
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
        static void ShowMenu()
        {
            Console.Clear();
            Random rnd = new Random();
            Console.WriteAscii(strings.wrinfo, list[rnd.Next(0, list.Length)]);
            Console.WriteLine("Version " + Value.VERSION);
        }

        /// <summary>
        /// Show all possible commands
        /// </summary>
        static void ShowUsage()
        {
            Console.WriteLine("start (sec)\t");
            Console.WriteLine("help\t");
            Console.WriteLine("menu\t");
            Console.WriteLine("language\t");
            Console.WriteLine("server\t");
            Console.WriteLine("path\t");
            Console.WriteLine("clear\t");
            Console.WriteLine("reset\t");
            Console.WriteLine("resetall\t");
            Console.WriteLine("exit\t");
            Console.WriteLine("wows\t");
            Console.WriteLine("facebook\t");
            Console.WriteLine("number\t");
            Console.WriteLine("github\t");
        }

        /// <summary>
        /// Reset or setup everything
        /// </summary>
        static void Reset()
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
        static void AnyKeyToContinue()
        {
            // Waiting for user input to proceed to next level
            Console.Write("> ");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Choose user's perfered language (English, Chinese or Japanese)
        /// </summary>
        static void LanguageSelection()
        {
            Console.WriteAscii(strings.language, Colour.WYellow);
            Console.WriteLine(strings.only_threelanguages, Color.White);
            Console.Write("1. English\n2. 简体中文\n3. 日本語\n> ");
            var language = Console.ReadLine();
        }

        /// <summary>
        /// Welcome messages and some warnings
        /// </summary>
        static void WelcomeMessage()
        {
            Console.WriteAscii(strings.welcome, Colour.WBlue);
            Console.WriteLine(strings.introduction, Color.White);
            Console.WriteLine("\n" + strings.agreement + "\n", Color.Red);
            AnyKeyToContinue();
        }

        /// <summary>
        /// Ask user for their game path and save it locally
        /// </summary>
        static void SetupGamePath()
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
        static void PullDataFromAPI()
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

            // Start downloading data
            Console.WriteLine("\n" + strings.pull_api, Color.White);
            API.PullData();
        }
    }
    #endregion
}
