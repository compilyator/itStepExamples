using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace PicsBinding
{
    public class PicturesViewModel
    {
        private string folderName = "Pictures";

        public ObservableCollection<Uri> FileNames { get; set; }

        public ICommand GetFileNames { get; set; }

        public PicturesViewModel()
        {
            GetFileNames = new RelayCommand(o => LoadPictures());
            FileNames = new ObservableCollection<Uri>();
        }

        private void LoadPictures()
        {
            var fileNames = Directory.GetFiles(folderName).Select(fullName => Path.GetFileName(fullName));
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var fileName in fileNames)
            {
                var path = Path.Combine(baseDir, folderName, fileName);
                FileNames.Add(new Uri(path));
            }
        }
    }
}