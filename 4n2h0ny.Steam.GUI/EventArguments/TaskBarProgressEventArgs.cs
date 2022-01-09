using System;
using System.Windows.Shell;

namespace _4n2h0ny.Steam.GUI.EventArguments
{
    public class TaskBarProgressEventArgs: EventArgs
    {
        public TaskbarItemProgressState TaskbarItemProgressState { get; set; }
        public double ProgressValue { get; set; }
    }
}
