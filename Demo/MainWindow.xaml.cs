using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using GeekJ.FolderTreeControl.Model;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // finished as a result of cancellation, which can only happen if the selection changed, so start over
                worker.RunWorkerAsync(folderTree.Selection);
            }
            status.Dispatcher.Invoke(new Action(delegate()
            {
                status.Text = string.Empty;
            }));
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            FolderTreeSelection selection = (FolderTreeSelection)e.Argument;
            foreach (var folder in selection.SelectedFolders)
            {
                // use the dispatcher to set the status text since we're in a different thread
                status.Dispatcher.Invoke(new Action(delegate()
                {
                    status.Text = folder.FullName;
                }));

                // check the cancel flag after each iteration
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void FolderTreeSelection_Changed(object sender, EventArgs e)
        {
            // this will be triggered every time the selection changes
            if (worker.IsBusy)
            {
                // if the background process is already running, cancel it since the selection is different
                worker.CancelAsync();
            }
            else
            {
                // otherwise, start the background process
                worker.RunWorkerAsync(sender);
            }
        }
    }
}
