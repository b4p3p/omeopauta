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
        private List<DBAppunto> _selectedAppunti = new List<DBAppunto>();

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();   
            txtSearchBox.delegateLateTextChange = new SearchTextBox.TextChangeDelegate(TextChange);
            SetEnableButtons();
        }

        private void RefreshData()
        {
            VisibleTags = controls.Tag.FromArray(DBCtrl.GetTags(""));
            NotifyPropertyChanged("VisibleTags");
            ListaAppunti = new ObservableCollection<DBAppunto>(DBCtrl.GetAppunti(""));
        }

        /// <summary>
        /// Flag per controllare quando si apre una nuova finestra
        /// </summary>
        public bool FormActive
        {
            get { return _formActive; }
            set {
                _formActive = value;
                SetEnableButtons();
            }
        }

        private void SetEnableButtons()
        {
            btnAdd.IsEnabled = _formActive ? true : false;
            btnEdit.IsEnabled = _formActive && _selectedAppunti.Count == 1 ? true : false;
        }

        public ObservableCollection<Tag> VisibleTags { get; set; }

        public ObservableCollection<DBAppunto> ListaAppunti {
            get { return _listaAppunti; }
            set { _listaAppunti = value; NotifyPropertyChanged("ListaAppunti"); }
        }

        public List<DBAppunto> SelectedAppunti {
            get { return _selectedAppunti; }
            set {
                _selectedAppunti = value;
                SetEnableButtons();
                NotifyPropertyChanged("CountSelected");
            }
        }

        public int CountSelected { get {
                return SelectedAppunti.Count;
        } }

        private void TextChange(string newValue)
        {
            VisibleTags = controls.Tag.FromArray(DBCtrl.GetTags(newValue));
            NotifyPropertyChanged("VisibleTags");
        }

        private void btnDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Rimuovere completamente le note selezionate?", "Conferma operazione", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
            if( res == MessageBoxResult.Yes)
            {
                DBCtrl.DeleteAppunto(SelectedAppunti);
                RefreshData();
            }
        }

        private void btnEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedAppunti.Count > 0)
                openEdit(SelectedAppunti[0]);
        }
        private void btnAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            openEdit(null);
        }

        private void lstDati_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                SelectedAppunti.Add((DBAppunto)item);
            }
            foreach (var item in e.RemovedItems)
            {
                SelectedAppunti.Remove((DBAppunto)item);
            }
            SetEnableButtons();
            NotifyPropertyChanged("SelectedAppunti");
            NotifyPropertyChanged("CountSelected");
        }

        private void lstDati_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as DBAppunto;
            openEdit(item);
        }

        private void openEdit(DBAppunto selected)
        {
            if (SelectedAppunti.Count != 1 && selected != null) return;

            FrmEdit frm = new FrmEdit(selected);
            frm.Owner = this;

            FormActive = false;
            frm.ShowDialog();
            FormActive = true;

            RefreshData();
        }
    }
}
