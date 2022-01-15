using System;
using System.Collections.Generic;
using System.Windows;
using _4n2h0ny.Steam.GUI.EventArguments;
using _4n2h0ny.Steam.GUI.Models;

namespace _4n2h0ny.Steam.GUI
{
    /// <summary>
    /// Interaction logic for ManualProfiles.xaml
    /// </summary>
    public partial class OutputDialog : Window
    {
        public event EventHandler<OutputDialogClosedEventArgs>? OutputDialogClosed;
        private bool AutomationRunning;
        private readonly Profile profile;

        public OutputDialog(Profile profile, MainWindow mainWindow)
        {
            InitializeComponent();
            this.profile = profile;
            mainWindow.AutomationRunning += MainWindow_AutomationRunning;
            UpdateManualProfileListBox(profile);
        }

        private void ExcludeContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (manualProfileListBox.SelectedItem is SteamUrlModel selectedManualProfile)
            {
                SqliteDataAccess.SaveExcludedUrl(selectedManualProfile);
                SqliteDataAccess.DeleteManualUrl(selectedManualProfile);
                UpdateManualProfileListBox(profile);
            }            
        }

        #region ButtonFunctions

        private void NavigateToBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!AutomationRunning)
            {
                if (manualProfileListBox.SelectedItem is SteamUrlModel selectedManualProfile)
                {
                    WebDriverSingleton webDriverSingleton = new();
                    if (webDriverSingleton.Driver != null)
                    {
                        webDriverSingleton.Driver.Navigate().GoToUrl(selectedManualProfile.Url);
                    }
                    webDriverSingleton.DisposeDriver(this);
                }
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!AutomationRunning)
            {
                if (manualProfileListBox.SelectedItem is SteamUrlModel selectedManualProfile)
                {
                    SqliteDataAccess.DeleteManualUrl(selectedManualProfile);
                    UpdateManualProfileListBox(this.profile);
                }
            }
        }

        #endregion ButtonFunctions

        #region EventHandlerFunctions

        private void MainWindow_AutomationRunning(object? sender, AutomationRunningEventArgs e)
        {
            AutomationRunning = e.Running;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            OutputDialogClosedEventArgs outputDialogClosedEventArgs = new()
            {
                Closed = true
            };

            OnOutputDialogClosed(outputDialogClosedEventArgs);
        }

        protected virtual void OnOutputDialogClosed(OutputDialogClosedEventArgs e)
        {
            OutputDialogClosed?.Invoke(this, e);
        }

        #endregion EventHandlerFunctions

        #region UIFunctions

        public void AppendLogTxtBox(string appendString)
        {
            string currentTime = String.Format("{0:HH:mm:ss tt}", DateTime.Now);
            logTxtBox.Text += $"{currentTime}     {appendString}\n";
        }

        public void UpdateManualProfileListBox(Profile profile)
        {
            profile.ManualProfileUrls = SqliteDataAccess.GetAllManualUrls();
            manualProfileListBox.ItemsSource = profile.ManualProfileUrls;
        }

        public void UpdateStatisticsTxtBox(int currentIndex, int profileUrlsCount, int manualProfileCounter)
        {
            double progressPercentage = Math.Round((((double)currentIndex + 1) / (double)profileUrlsCount) * 100, 2);
            string commentProgressString = $"Progress : {currentIndex + 1}/{profileUrlsCount} ({progressPercentage}%)";
            string manualProfileCounterString = $"Manual Profiles: {manualProfileCounter}";
            this.statisticsTxtBlock.Text = $"{commentProgressString}\n{manualProfileCounterString}";
        }

        #endregion UIFunctions
    }
}
