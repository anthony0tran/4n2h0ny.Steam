using System;
using System.Windows;
using _4n2h0ny.Steam.GUI.EventArguments;

namespace _4n2h0ny.Steam.GUI
{
    /// <summary>
    /// Interaction logic for ManualProfiles.xaml
    /// </summary>
    public partial class OutputDialog : Window
    {
        public event EventHandler<OutputDialogClosedEventArgs>? OutputDialogClosed;

        public OutputDialog()
        {
            InitializeComponent();
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

        public void AppendLogTxtBox(string appendString)
        {
            logTxtBox.Text += appendString + "\n";
        }
    }
}
