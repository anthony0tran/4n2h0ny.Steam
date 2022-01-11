using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using _4n2h0ny.Steam.GUI.EventArguments;
using _4n2h0ny.Steam.GUI.Models;

namespace _4n2h0ny.Steam.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean outputWindowClosed = true;

        public event EventHandler<TaskBarProgressEventArgs>? TaskBarProgressUpdated;

        public event EventHandler<AutomationRunningEventArgs>? AutomationRunning;

        public MainWindow()
        {
            InitializeComponent();

            Title = "4n2h0ny.Steam v" + Globals.version.ToString();

            SetDefaultTxtBoxValues();
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxValidator() && outputWindowClosed)
            {
                WebDriverSingleton webDriverSingleton = new();                

                if (webDriverSingleton.Driver != null)
                {
                    Profile profile = new(webDriverSingleton.Driver);
                    OutputDialog outputDialog = new(profile, this);
                    outputDialog.OutputDialogClosed += OutputDialog_OutputDialogClosed;
                    outputDialog.Show();

                    AutomationRunningEventArgs automationRunningEventArgs = new()
                    {
                        Running = true
                    };

                    OnAutomationRunning(automationRunningEventArgs);

                    outputWindowClosed = false;

                    Comment.TestComment(webDriverSingleton.Driver, profile, commentTemplateTxtBox.Text, defaultCommentTxtBox.Text, outputDialog);

                    webDriverSingleton.DisposeDriver(outputDialog);

                    automationRunningEventArgs.Running = false;
                    OnAutomationRunning(automationRunningEventArgs);
                }
            }
        }

        private void MainWindow_TaskBarProgressUpdated(object? sender, TaskBarProgressEventArgs e)
        {
            TaskbarItemInfo.ProgressState = e.TaskbarItemProgressState;
            TaskbarItemInfo.ProgressValue = e.ProgressValue;
        }

        public virtual void OnTaskbarProgressUpdated(TaskBarProgressEventArgs e)
        {
            TaskBarProgressUpdated?.Invoke(this, e);
        }

        public virtual void OnAutomationRunning(AutomationRunningEventArgs e)
        {
            AutomationRunning?.Invoke(this, e);
        }

        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxValidator() && outputWindowClosed)
            {
                WebDriverSingleton webDriverSingleton = new();
                
                if (webDriverSingleton.Driver != null)
                {
                    Profile profile = new(webDriverSingleton.Driver);
                    TaskBarProgressUpdated += MainWindow_TaskBarProgressUpdated;

                    OutputDialog outputDialog = new(profile, this);
                    outputDialog.OutputDialogClosed += OutputDialog_OutputDialogClosed;
                    outputDialog.Show();

                    AutomationRunningEventArgs automationRunningEventArgs = new()
                    {
                        Running = true
                    };

                    OnAutomationRunning(automationRunningEventArgs);

                    outputWindowClosed = false;

                    // Retrieve the ProfileData of the first steam page that is opened in the browser session.
                    profile.GetMainProfileData(outputDialog);

                    if (profile.ProfileUrls.Count == 0 && profile.ManualProfileUrls.Count == 0)
                    {
                        // Get the urls to the profiles that commented.
                        await profile.GatherProfileUrls(this, outputDialog, int.Parse(maxPageIndexTxtBox.Text));

                        TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                    }
                    else
                    {
                        outputDialog.AppendLogTxtBox($"Skipped profile scraping. {profile.ProfileUrls.Count} profiles and {profile.ManualProfileUrls.Count} manualProfiles found.");
                    }

                    await Comment.CommentAllPages(this, webDriverSingleton.Driver, profile, commentTemplateTxtBox.Text, defaultCommentTxtBox.Text, outputDialog);

                    profile.ProfileUrls = SqliteDataAccess.GetAllUrls();

                    TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;

                    if (Comment.NoFormCounter > 0)
                    {
                        Comment.NoFormCounter = 0;
                    }

                    webDriverSingleton.DisposeDriver(outputDialog);
                    automationRunningEventArgs.Running = false;
                    OnAutomationRunning(automationRunningEventArgs);
                }

            }
        }

        private static void ResetDb()
        {
            List<SteamUrlModel> steamUrlList = SqliteDataAccess.GetAllUrls();
            ObservableCollection<SteamUrlModel> manualProfileList = SqliteDataAccess.GetAllManualUrls();

            if (steamUrlList.Count > 0 || manualProfileList.Count > 0)
            {
                MessageBoxResult ResetDb = MessageBox.Show($"Delete {steamUrlList.Count} profile(s) and {manualProfileList.Count} manualProfile(s) from the database?", "Reset database", MessageBoxButton.YesNo);
                if (ResetDb == MessageBoxResult.Yes)
                {
                    SqliteDataAccess.ResetProfileTable();
                    SqliteDataAccess.ResetManualProfileTable();
                }
            }
            else
            {
                MessageBox.Show("steamUrlList and manualProfileList are empty");
            }
        }

        private void OutputDialog_OutputDialogClosed(object? sender, OutputDialogClosedEventArgs e)
        {
            outputWindowClosed = e.Closed;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            defaultCommentTxtBox.Text = "";
            commentTemplateTxtBox.Text = "";
        }

        private void SetDefaultTxtBoxValues()
        {
            maxPageIndexTxtBox.Text = Globals.MaxPageIndex.ToString();
            defaultCommentTxtBox.Text = Globals.DefaultCommentString.ToString();
            commentTemplateTxtBox.Text = Globals.CommentTemplate.ToString();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool TextBoxValidator()
        {
            if (!String.IsNullOrEmpty(commentTemplateTxtBox.Text) &&
                !String.IsNullOrEmpty(defaultCommentTxtBox.Text) &&
                (!String.IsNullOrEmpty(maxPageIndexTxtBox.Text) &&
                int.Parse(maxPageIndexTxtBox.Text) > 0))
            {
                commentTemplateTxtBox.ClearValue(Border.BorderBrushProperty);
                defaultCommentTxtBox.ClearValue(Border.BorderBrushProperty);
                maxPageIndexTxtBox.ClearValue(Border.BorderBrushProperty);

                return true;
            }
            else
            {
                if (String.IsNullOrEmpty(commentTemplateTxtBox.Text))
                {
                    commentTemplateTxtBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    commentTemplateTxtBox.ClearValue(Border.BorderBrushProperty);
                }

                if (String.IsNullOrEmpty(defaultCommentTxtBox.Text))
                {
                    defaultCommentTxtBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    defaultCommentTxtBox.ClearValue(Border.BorderBrushProperty);
                }

                if (String.IsNullOrEmpty(maxPageIndexTxtBox.Text) || int.Parse(maxPageIndexTxtBox.Text) <= 0)
                {
                    if (int.Parse(maxPageIndexTxtBox.Text) <= 0)
                    {
                        MessageBox.Show("Invalid value for maxPageIndexTxtBox");
                    }

                    maxPageIndexTxtBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    maxPageIndexTxtBox.ClearValue(Border.BorderBrushProperty);
                }

                return false;
            }
        }

        private void ResetDbBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetDb();
        }
    }
}
