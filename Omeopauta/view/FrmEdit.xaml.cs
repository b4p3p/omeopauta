using Omeopauta.context;
using Omeopauta.controller;
using Omeopauta.controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace Omeopauta.view
{
    /// <summary>
    /// Logica di interazione per Edit.xaml
    /// </summary>
    public partial class FrmEdit : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DBAppunto _selectedAppunto;
        private List<DBImage> _listImages = new List<DBImage>();

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public DBAppunto Appunto {
            get { return _selectedAppunto; }
            set { _selectedAppunto = value; }
        }

        public List<DBImage> ListImage
        {
            get { return _listImages; }
            set { _listImages = value; }
        }

        public bool IsEdit { get { return _selectedAppunto != null; } }

        public FrmEdit(DBAppunto appunto)
        {
            InitializeComponent();

            if( appunto == null)
            {
                _selectedAppunto = new DBAppunto();
                lblHeader.Content = "Nuova Nota";
            }
            else
            {
                _selectedAppunto = appunto;
                lblHeader.Content = "Modifica Nota";
            }

            IEnumerable<DBImage> images = Appunto.GetImages();
            if( images != null)
                foreach (DBImage item in images)
                {
                    ImageGallery img = new ImageGallery(item);
                    gallery.Children.Add(img);
                    img.Load();
                }
            this.DataContext = Appunto;
        }

        private void btnUndo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.EditBox.UpdateDocumentBindings();
        }
        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.EditBox.UpdateDocumentBindings();
            Appunto.SimpleText = new TextRange(EditBox.Document.ContentStart, EditBox.Document.ContentEnd).Text;
            DBCtrl.InsertOrUpdate(Appunto);
            DBCtrl.AddImages(ListImage, Appunto);
            this.Close();
        }

        private void btnAddImg_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "All Images|*.jpeg;*.png;*.jpg;*.gif|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (dlg.ShowDialog() == true)
            {
                string[] names = dlg.SafeFileNames;
                string[] pathNames = dlg.FileNames;
                for (int i=0;i< pathNames.Length;i++)
                {
                    DBImage dbImg = new DBImage(Appunto, names[i], pathNames[i]);
                    ListImage.Add(dbImg);
                    gallery.Children.Add(new ImageGallery(dbImg));
                }
                //copia l'immagine
            }
        }

        private void btnDelImg_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            UIElementCollection collections = gallery.Children;
            ImageGallery[] images = (from UIElement img in collections
                                     where ((ImageGallery)img).IsSelected
                                     select (ImageGallery)img).ToArray<ImageGallery>();
            if(images.Length == 0)
            {
                MessageBox.Show("Nessuna immagine selezionata", "Messaggio", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult res =  MessageBox.Show("Eliminare DEFINITIVAMENTE le foto selezionate?", "Conferma", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if ( res== MessageBoxResult.Yes)
            {
                DBCtrl.DeleteImages(images);
                for(var i = 0; i< images.Length; i++)
                {
                    ListImage.Remove(images[i].DBImg);
                    gallery.Children.Remove(images[i]);
                }
                    
            }
        }
    }
}
