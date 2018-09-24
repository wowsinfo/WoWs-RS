using System.Windows;
using WoWs_RS.Core;

namespace WoWs_RS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Always check for Path, Domain, Language and Version
            // If they are lost, we have to get it again
            var manager = new DataManager();
            manager.checkPath();
        }
    }
}
