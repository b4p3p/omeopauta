using Omeopauta.context;
using Omeopauta.controller;
using Omeopauta.controls;
using Omeopauta.view;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Omeopauta
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _formActive = true;
        private ObservableCollection<DBAppunto> _listaAppunti;

        private List<Tag> lstTag = new List<Tag>();

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow() { InitializeComponent(); }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VisibleTags = controls.Tag.FromArray(DBCtrl.GetTags(""));
            NotifyPropertyChanged("VisibleTags");

            ListaAppunti = new ObservableCollection<DBAppunto>(DBCtrl.GetAppunti(""));

            txtSearchBox.delegateLateTextChange = new SearchTextBox.TextChangeDelegate(TextChange);
        }

        public bool FormActive
        {
            get { return _formActive; }
            set {
                _formActive = value;
                NotifyPropertyChanged("FormActive");
            }
        }

        public ObservableCollection<Tag> VisibleTags { get; set; }

        public ObservableCollection<DBAppunto> ListaAppunti {
            get { return _listaAppunti; }
            set { _listaAppunti = value; NotifyPropertyChanged("ListaAppunti"); }
        }

        private void TextChange(string newValue)
        {
            VisibleTags = controls.Tag.FromArray(DBCtrl.GetTags(newValue));
            NotifyPropertyChanged("VisibleTags");
        }
        private void btnEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void btnAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrmEdit frm = new FrmEdit(false);
            frm.Owner = this;

            FormActive = false;
            frm.ShowDialog();
            FormActive = true;

            ListaAppunti = new ObservableCollection<DBAppunto>(DBCtrl.GetAppunti(""));
        }
    }
}
