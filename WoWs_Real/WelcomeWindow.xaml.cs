﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

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

        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            // Only load game if there is a valid path
            if (DataManager.isPathValid()) Process.Start(DataManager.getGameExePath());
        }
    }
}