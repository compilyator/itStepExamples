namespace WorkerExample
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public new ViewModel DataContext
        {
            get
            {
                return base.DataContext as ViewModel;
            }

            set
            {
                base.DataContext = value;
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            this.DataContext.Start();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            this.DataContext.Stop();
        }
    }
}