using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Control = System.Windows.Controls.Control;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;

namespace WoWs_Real
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WelcomeWindow: Window
    {

        private static System.Timers.Timer RealTimer;
        private static string[,,] PlayerInformation = new string[2, 24, 5];

        // Data Index
        private static int PlayerName = 0;
        private static int PlayerShipName = 1;
        private static int PlayerBattle = 2;
        private static int PlayerWinrate = 3;
        private static int PlayerRating = 4;

        public WelcomeWindow()
        {
            InitializeComponent();

            // Get Ship Json
            DataManager.getShipJson();

            // Get Expected Json
            ShipRating.getExpectedJson();

            // Check if there is already a document file
            if (!DataManager.isDataValid()) DataManager.createData();
            // Check if gamepath is valid
            if (!DataManager.isPathValid())
            {
                SetupBtn_OnClick(sender: this, e: null);
                Topmost = true;
            }
            else
            {
                // Hide setup button
                SetupBtn.IsEnabled = false;
                // Delete old log
                DataManager.deleteLogFile();
            }

            // Create a timer with 20 minutes interval
            RealTimer = new System.Timers.Timer(20000);
            // Hook up the Elapsed event for the timer. 
            RealTimer.Elapsed += RealTimer_Elapsed;
            // Keep timer Alive
            GC.KeepAlive(RealTimer);
        }

        private void RealTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Factory.StartNew(updatePlayerInformation);
        }


        #region Button clicks

        private void WoWsInfoBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Visit my APP
            Process.Start(@"https://itunes.apple.com/app/id1202750166");
        }

        private void OfficialBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Visit Official
            Process.Start(@"https://worldofwarships.com");
        }

        private void WikiBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Visit Wiki
            Process.Start(@"http://wiki.wargaming.net/en/World_of_Warships");
        }

        private void NumberBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Visit number
            Process.Start(@"http://wows-numbers.com");
        }

        private void SEABtn_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"https://sea-group.org");
        }

        private void ASLAINBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"http://aslain.com/index.php?/topic/2020-0640-aslains-wows-modpack-installer-wpicture-preview");
        }

        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Only load game if there is a valid path
            if (DataManager.isPathValid()) Process.Start(DataManager.getGamePath() + @"\WoWSLauncher.exe");
            // Start timer
            RealTimer.Enabled = true;
            UpdateIndicator.Text = "SYSTEM ONLINE";
        }

        #endregion

        #region GamePath and GameStart

        private void SetupBtn_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog GamePath = new FolderBrowserDialog();
            GamePath.Description = @"Please choose your game directory";
            if (GamePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = GamePath.SelectedPath;
                if (File.Exists(path + @"\WorldOfWarships.exe"))
                {
                    DataManager.setGamePath(path);
                }
                else
                {
                    System.Windows.MessageBox.Show(@"Game Directory is not VALID.", @"Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Windows.Forms.Application.ExitThread();
                }
            }
            // Restart Software
            System.Windows.Forms.Application.Restart();
        }

        #endregion

        #region Timer

        // Do this every 20 second
        private void updatePlayerInformation()
        {
            this.Dispatcher.Invoke(() =>
            {
                UpdateIndicator.Text = "CHECKING FOR UPDATES";
            });
            
            // Check for python.log
            string log = DataManager.readLogFile();
            if (log == String.Empty)
            {
                this.Dispatcher.Invoke(() =>
                {
                    UpdateIndicator.Text = "NO UPDATES";
                });
                Console.WriteLine(@"Log is empty");
                return;
            }
            string dataString = DataManager.getCurrPlayerStr(log);
            if (dataString == String.Empty)
            {
                this.Dispatcher.Invoke(() =>
                {
                    UpdateIndicator.Text = "NO NEW BATTLES";
                });
                
                Console.WriteLine(@"dataString is Empty.");
                return;
            }

            // Get player data from log
            string [,,] info = DataManager.getPlayerInfomation(dataString);

            // Get ship name and id
            int i;
            int team = 0;
            string[] shipInfo;
            string[] api;

            for (i = 0; i < 24; i++)
            {
                this.Dispatcher.Invoke(() =>
                {
                    UpdateIndicator.Text = "UPDATING PLAYER " + i;
                });

                if (i < 12)
                {
                    team = 0;
                    // Team 0
                    shipInfo = DataManager.getPlayerShipInfo(info[team, i, DataManager.DataIndex.ship]);
                    PlayerInformation[0, i, PlayerName] = info[team, i, DataManager.DataIndex.name];
                    PlayerInformation[0, i, PlayerShipName] = shipInfo[DataManager.DataIndex.shipName];
                    // Get data
                    api = API.getPlayerShipInfo(info[team, i, DataManager.DataIndex.name],
                        shipInfo[DataManager.DataIndex.shipId]);
                    // Store data
                    Console.WriteLine(i);
                    if (api.Length == 0)
                    {
                        PlayerInformation[0, i, PlayerBattle] = "";
                        PlayerInformation[0, i, PlayerWinrate] = "";
                    }
                    else
                    {
                        PlayerInformation[0, i, PlayerBattle] = api[API.DataIndex.battle];
                        PlayerInformation[0, i, PlayerWinrate] = (Convert.ToDouble(api[API.DataIndex.winrate]) * 100).ToString("0.##");
                    }
                    
                }
                else
                {
                    team = 1;
                    // Team 1
                    shipInfo = DataManager.getPlayerShipInfo(info[team, i - 12, DataManager.DataIndex.ship]);
                    PlayerInformation[1, i - 12, PlayerName] = info[team, i - 12, DataManager.DataIndex.name];
                    PlayerInformation[1, i - 12, PlayerShipName] = shipInfo[DataManager.DataIndex.shipName];
                    // Get data
                    api = API.getPlayerShipInfo(info[team, i - 12, DataManager.DataIndex.name],
                        shipInfo[DataManager.DataIndex.shipId]);
                    Console.WriteLine(i);
                    if (api.Length == 0)
                    {
                        PlayerInformation[1, i - 12, PlayerBattle] = "";
                        PlayerInformation[1, i - 12, PlayerWinrate] = "";
                    }
                    else
                    {
                        // Store data
                        PlayerInformation[1, i - 12, PlayerBattle] = api[API.DataIndex.battle];
                        PlayerInformation[1, i - 12, PlayerWinrate] = (Convert.ToDouble(api[API.DataIndex.winrate]) * 100).ToString("0.##");
                    } 
                }

                // Skip rating if user hides data
                if (api.Length == 0) continue;
                double rating = ShipRating.getRating(Convert.ToDouble(api[API.DataIndex.battle]), api[API.DataIndex.id],
                    Convert.ToDouble(api[API.DataIndex.frag]), Convert.ToDouble(api[API.DataIndex.winrate]),
                    Convert.ToDouble(api[API.DataIndex.damage]));
                PlayerInformation[team, (i < 12 ? i : i - 12), PlayerRating] = rating.ToString("0.##");
            }

            this.Dispatcher.Invoke(() =>
            {
                this.displayData();
                UpdateIndicator.Text = "DATA UPDATED";
                Console.WriteLine(@"Data updated");
            });

        }

        #endregion

        #region Display Data

        private void displayData()
        {
            int i;
            for (i = 0; i < 24; i++)
            {
                Label playerLabel = (Label) FindName(getLabelName(i));
                Label playerRatingLabel = (Label) FindName(getRatingLabelName(i));

                if (i < 12)
                {
                    // Team 0
                    if (playerLabel != null) playerLabel.Content = getLabelString(0, i);
                    if (playerRatingLabel != null)
                    {
                        string battle = PlayerInformation[0, i, PlayerBattle];
                        if (battle == String.Empty)
                        {
                            playerRatingLabel.Content = "HIDDEN";
                        }
                        else
                        {
                            playerRatingLabel.Content = ShipRating.getComment(Convert.ToDouble(PlayerInformation[0, i, PlayerRating]));
                            playerRatingLabel.Foreground =
                                ShipRating.getRatingColor(Convert.ToDouble(PlayerInformation[0, i, PlayerRating]));
                        }
                    }
                }
                else
                {
                    // Team 1
                    if (playerLabel != null) playerLabel.Content = getLabelString(1, i);
                    if (playerRatingLabel != null)
                    {
                        string battle = PlayerInformation[1, i - 12, PlayerBattle];
                        if (battle == String.Empty)
                        {
                            playerRatingLabel.Content = "HIDDEN";
                        }
                        else
                        {
                            playerRatingLabel.Content = ShipRating.getComment(Convert.ToDouble(PlayerInformation[1, i - 12, PlayerRating]));
                            playerRatingLabel.Foreground =
                                ShipRating.getRatingColor(Convert.ToDouble(PlayerInformation[1, i - 12, PlayerRating]));
                        }
                    }
                }
            }
        }

        private string getLabelName(int i)
        {
            int j = i;
            return @"Player" + (j + 1) + @"Label";
        }

        private string getRatingLabelName(int i)
        {
            int j = i;
            return @"Player" + (j + 1) + @"RatingLabel";
        }

        private string getLabelString(int team, int index)
        {
            if (index < 12)
            {
                // Team 0
                string name = PlayerInformation[team, index, PlayerName];
                string ship = PlayerInformation[team, index, PlayerShipName];
                string battle = PlayerInformation[team, index, PlayerBattle];
                string winrate = PlayerInformation[team, index, PlayerWinrate];
                if (battle == "" || winrate == "") return ship + "   " + name;
                return ship + "   " + name + "   " + battle + " | " + winrate + "%";
            }
            else
            {
                // Team 1
                string name = PlayerInformation[team, index - 12, PlayerName];
                string ship = PlayerInformation[team, index - 12, PlayerShipName];
                string battle = PlayerInformation[team, index - 12, PlayerBattle];
                string winrate = PlayerInformation[team, index - 12, PlayerWinrate];
                if (battle == "" || winrate == "") return name + "   " + ship;
                return winrate + "% | " + battle + "   " + name + "  " + ship;
            }
        }

        #endregion
    }
}
