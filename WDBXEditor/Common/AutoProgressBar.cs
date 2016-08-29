using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDBXEditor.Common
{
    class AutoProgressBar : ProgressBar
    {
        private BackgroundWorker bgw = new BackgroundWorker();

        public void Start(int increment = 3)
        {
            if (bgw.IsBusy) return;

            this.Style = ProgressBarStyle.Continuous;
            this.Value = 0;
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.WorkerReportsProgress = true;
            bgw.WorkerSupportsCancellation = true;
            bgw.RunWorkerAsync(increment);
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            int inc = (int)e.Argument;
            int i = 0;

            while (!bgw.CancellationPending)
            {
                System.Threading.Thread.Sleep(250);
                int percent = i;

                if (percent > 100)
                {
                    percent = 100;
                    i = 0;
                }
                else
                    i += inc;

                bgw.ReportProgress(percent);
            }
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate { Value = e.ProgressPercentage; });
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Task.Run(() => ClearValue());
        }

        public void Stop()
        {
            if (bgw.IsBusy)
                bgw.CancelAsync();

            Task.Run(() => ClearValue());
        }

        private async Task ClearValue()
        {
            await Task.Factory.StartNew(() =>
            {
                while (bgw.CancellationPending || this.Value != 0)
                {
                    this.Invoke((MethodInvoker)delegate { this.Value = 0; });
                    Task.Delay(50);
                }
            });
        }
    }
}
