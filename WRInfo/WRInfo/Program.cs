using System;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Drawing;
using WRInfo.Core;
using WRInfo.Resouces;
using WRInfo.Properties;
using Console = Colorful.Console;

namespace WRInfo
{
    class Program
    {
        /// <summary>
        /// How many battle user has played
        /// Show some kind reminder for user
        /// </summary>
        static int battleCount = 0;

        static void Main(string[] args)
        {
            AppLaunch();
            
            Console.Read();
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
            if (dataManager.FirstLaunch())
            {
                WelcomeMessage();
                SetupGamePath();
                PullDataFromAPI();
            }
            else
            {
                // Load data
            }
        }

        /// <summary>
        /// Press any key to continue this program
        /// </summary>
        static void AnyKeyToContinue()
        {
            // Waiting for user input to proceed to next level
            Console.Write(strings.anykey_continue);
            Console.ReadKey();
            Console.Clear();
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
                Console.Write(strings.enter_gamepath, Color.White);
                var gamePath = Console.ReadLine();
                Console.WriteLine(strings.validate_gamepath + "<<" + gamePath + ">>\n", Color.White);
                Thread.Sleep(500);

                if (File.Exists(gamePath + "/WorldOfWarships.exe") && File.Exists(gamePath + "/preferences.xml"))
                {
                    Console.WriteLine(strings.path_valid, Colour.WGreen);
                    Settings.Default.GamePath = gamePath;
                    isValid = true;
                    AnyKeyToContinue();
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
            Console.WriteAscii(strings.api, Colour.WOrange);
        }
    }
}
