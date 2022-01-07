using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _4n2h0ny.Steam.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly WebDriverSingleton webDriverSingleton = new();
        readonly Profile profile;
        Boolean outputWindowClosed = true;

        public MainWindow()
        {
            InitializeComponent();

            profile = SetProfile();
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxValidator() && outputWindowClosed)
            {
                OutputDialog outputDialog = new();
                outputDialog.OutputDialogClosed += O_OutputDialogClosed;
                outputDialog.Show();
                outputWindowClosed = false;

                Comment.TestComment(webDriverSingleton.Driver, profile, commentTemplateTxtBox.Text, defaultCommentTxtBox.Text, outputDialog);
            }
        }

        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxValidator() && outputWindowClosed)
            {
                OutputDialog outputDialog = new();
                outputDialog.OutputDialogClosed += O_OutputDialogClosed;
                outputDialog.Show();
                outputWindowClosed = false;

                // Retrieve the ProfileData of the first steam page that is opened in the browser session.
                profile.GetMainProfileData(outputDialog);

                // Get the urls to the profiles that commented.
                await profile.GatherProfileUrls(outputDialog, int.Parse(maxPageIndexTxtBox.Text));

                await Comment.CommentAllPages(webDriverSingleton.Driver, profile, commentTemplateTxtBox.Text, defaultCommentTxtBox.Text, outputDialog);
            }
        }

        private void O_OutputDialogClosed(object sender, OutputDialogClosedEventArgs e)
        {
            outputWindowClosed = e.Closed;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            defaultCommentTxtBox.Text = "";
            commentTemplateTxtBox.Text = "";
        }

        private Profile SetProfile()
        {
            maxPageIndexTxtBox.Text = Globals.MaxPageIndex.ToString();
            defaultCommentTxtBox.Text = Globals.DefaultCommentString.ToString();
            commentTemplateTxtBox.Text = Globals.CommentTemplate.ToString();

            return new(webDriverSingleton.Driver);
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
    }
}
