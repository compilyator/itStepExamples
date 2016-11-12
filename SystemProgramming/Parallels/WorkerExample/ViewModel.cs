namespace WorkerExample
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    using WorkerExample.Annotations;

    public class ViewModel: INotifyPropertyChanged
    {
        private int sum;

        private int progress;

        private int max;

        private BackgroundWorker worker;

        public ViewModel()
        {
            this.worker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            this.worker.DoWork += StartWork;
            this.worker.ProgressChanged += DoProgress;
            this.worker.RunWorkerCompleted += ShowResult;
        }

        private void ShowResult(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Cancelled ? "Aborted" : "Completed!");
            this.OnPropertyChanged(nameof(IsStarted));
            this.OnPropertyChanged(nameof(IsStoped));
        }

        private void DoProgress(object sender, ProgressChangedEventArgs e)
        {
            this.Progress = e.ProgressPercentage;
            this.Sum = (int)e.UserState;
        }

        private void StartWork(object sender, DoWorkEventArgs e)
        {
            var localWorker = sender as BackgroundWorker;
            if (localWorker == null) return;

            var localMax = (int)e.Argument;
            int localSum = 0;
            for (int i = 1; i <= localMax; i++)
            {
                if (localWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                localSum += i;
                localWorker.ReportProgress(i, localSum);
                Thread.Sleep(100);
            }

            e.Result = localSum;
        }

        public bool IsStarted => this.worker.IsBusy;

        public bool IsStoped => !this.worker.IsBusy;

        public int Sum
        {
            get
            {
                return this.sum;
            }

            set
            {
                if (value == this.sum) return;
                this.sum = value;
                this.OnPropertyChanged();
            }
        }

        public int Progress
        {
            get
            {
                return this.progress;
            }

            set
            {
                if (value == this.progress) return;
                this.progress = value;
                this.OnPropertyChanged();
            }
        }

        public int Max
        {
            get
            {
                return this.max;
            }

            set
            {
                if (value == this.max) return;
                this.max = value;
                this.OnPropertyChanged();
            }
        }

        public void Start()
        {
            this.worker.RunWorkerAsync(Max);
            this.OnPropertyChanged(nameof(IsStarted));
            this.OnPropertyChanged(nameof(IsStoped));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Stop()
        {
            this.worker.CancelAsync();
        }
    }
}