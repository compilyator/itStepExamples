using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PicsManual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string folderName = "Pictures";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            var fileNames = Directory.GetFiles(folderName).Select(fullName => Path.GetFileName(fullName));
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var fileName in fileNames)
            {
                var path = Path.Combine(baseDir, folderName, fileName);
                var uri = new Uri(path);
                var image = new Image {Source = new BitmapImage(uri)};
                PicsListView.Items.Add(image);
            }
        }
    }
}
