using System.Diagnostics;
using System.Windows;

namespace WoWs_Real
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WelcomeWindow: Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
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

        #endregion

    }
}
