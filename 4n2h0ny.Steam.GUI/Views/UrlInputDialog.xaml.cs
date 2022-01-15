using _4n2h0ny.Steam.GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _4n2h0ny.Steam.GUI.Views
{
    /// <summary>
    /// Interaction logic for UrlInputDialog.xaml
    /// </summary>
    public partial class UrlInputDialog : Window
    {
        public SteamUrlModel UrlInput { get; set; } = new();
        public bool SaveUrl { get; set; } = false;

        public UrlInputDialog()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputField())
            {
                this.UrlInput.Url = urlTxtBox.Text;
                this.SaveUrl = true;
                this.Close();
            }
        }

        private bool ValidateInputField()
        {
            if (String.IsNullOrEmpty(urlTxtBox.Text))
            {
                urlTxtBox.BorderBrush = Brushes.Red;
                return false;
            }

            urlTxtBox.ClearValue(Border.BorderBrushProperty);
            return true;
        }
    }
}
