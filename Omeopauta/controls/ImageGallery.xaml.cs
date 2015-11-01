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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Omeopauta.context;
using System.IO;

namespace Omeopauta.controls
{
    /// <summary>
    /// Logica di interazione per ImageGallery.xaml
    /// </summary>
    public partial class ImageGallery : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSelected = false;
        private DBImage _dbImg;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ImageGallery()
        {
            InitializeComponent();
            IsSelected = false;
        }

        public ImageGallery(DBImage dbImg)
        {
            InitializeComponent();
            this._dbImg = dbImg;
            NotifyPropertyChanged("DBImg");
            NotifyPropertyChanged("Path");
        }

        public void Load()
        {
            NotifyPropertyChanged("DBImg");
            NotifyPropertyChanged("Path");
        }

        public bool IsSelected
        {
            get {
                return _isSelected;
            }
            set {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
                NotifyPropertyChanged("MyBorderBrush");
            }
        }

        public Brush MyBorderBrush {
            get
            {
                var converter = new System.Windows.Media.BrushConverter();
                if (IsSelected)
                    return (Brush)converter.ConvertFromString("#FFEE402A");
                else
                    return Brushes.Transparent;
            }
        }

        public DBImage DBImg
        {
            get {
                return _dbImg;
            }
            set {
                _dbImg = value;
            }
        }

        public string Path {
            get {
                if (DBImg == null) return "";
                return DBImg.AbsPath;
            }
        }

        private void border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                MessageBox.Show("Double Click");
            
            IsSelected = !IsSelected;
        }
        
        internal void DeleteImage()
        {
            string path = Path;
            image.Source = null;
            (new System.Threading.Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        File.Delete(path);
                        Console.WriteLine("File: " + path + " deleted");
                        break;
                    }
                    catch(Exception e) {
                        Console.WriteLine("Wait for " + path);
                    }
                }
            })).Start();
        }
    }
}
