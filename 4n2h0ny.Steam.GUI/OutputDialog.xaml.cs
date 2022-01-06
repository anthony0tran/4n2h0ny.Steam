using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace _4n2h0ny.Steam.GUI
{
    /// <summary>
    /// Interaction logic for ManualProfiles.xaml
    /// </summary>
    public partial class OutputDialog : Window
    {
        public OutputDialog()
        {
            InitializeComponent();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            OutputDialogClosedEventArgs args = new()
            {
                Closed = true
            };

            OnOutputDialogClosed(args);
        }

        protected virtual void OnOutputDialogClosed(OutputDialogClosedEventArgs e)
        {
            OutputDialogClosed?.Invoke(this, e);
        }

        public event EventHandler<OutputDialogClosedEventArgs> OutputDialogClosed;

        public void AppendLogTxtBox(string appendString)
        {
            logTxtBox.Text += appendString + "\n";
        }
    }

    public class OutputLog
    {
        public string OutputLogString { get; set; }
    }

    public class OutputDialogClosedEventArgs : EventArgs
    {
        public bool Closed { get; set; }
    }
}
