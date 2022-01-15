using _4n2h0ny.Steam.GUI.Models;
using System;
using System.Windows;

namespace _4n2h0ny.Steam.GUI.Views
{
    /// <summary>
    /// Interaction logic for BlacklistDialog.xaml
    /// </summary>
    public partial class ExclusionListDialog : Window
    {
        private Profile? ProfileInstance { get; set; }

        public ExclusionListDialog(Profile? profile)
        {
            InitializeComponent();
            if (profile != null)
            {
                ProfileInstance = profile;
                profile.ExcludedProfileUrls = SqliteDataAccess.GetAllExcludedUrls();
                this.exclusionListBox.ItemsSource = profile.ExcludedProfileUrls;
            }
        }

        private void AddExclusionProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            UrlInputDialog urlInputDialog = new();
            urlInputDialog.ShowDialog();

            SteamUrlModel urlInput = urlInputDialog.UrlInput;

            if (!String.IsNullOrEmpty(urlInput.Url) && urlInputDialog.SaveUrl)
            {
                SqliteDataAccess.SaveExcludedUrl(urlInput);
                UpdateExcusionListBox(this.ProfileInstance);
            }
        }

        private void DeleteExclusionProfileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (exclusionListBox.SelectedItem is SteamUrlModel selectedExcludedProfile)
            {
                SqliteDataAccess.DeleteExcludedUrl(selectedExcludedProfile);
                UpdateExcusionListBox(this.ProfileInstance);
            }
        }

        private void UpdateExcusionListBox(Profile? profile)
        {
            if (profile != null)
            {
                profile.ExcludedProfileUrls = SqliteDataAccess.GetAllExcludedUrls();
                this.exclusionListBox.ItemsSource = profile.ExcludedProfileUrls;
            }
        }
    }
}
