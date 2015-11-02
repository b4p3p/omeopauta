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
            txtSearchBox.delegateLateTextChange = new SearchTextBox.TextChangeDelegate(txtTag_Change);
            txtSearchBoxAppunti.delegateLateTextChange = new SearchTextBox.TextChangeDelegate(txtAppunti_Change);
            SetEnableButtons();
        }

        #region Counter

        public int CountAllTags { get; set; }
        public int CountFilterTags { get; set; }
        public int CountAllAppunti {get; set;}
        public int CountFilterAppunti {
            get { return _countFilterAppunti; }
            set {
                _countFilterAppunti = value;
                NotifyPropertyChanged("CountFilterAppunti");
            }}
        private int _countFilterAppunti = 0;
        public int CountSelected
        {
            get
            {
                return SelectedAppunti.Count;
            }
        }

        #endregion Counter

        public string SelectedFilterTag {
            get {
                return _selectedFilterTag;
            }
            set {
                _selectedFilterTag = value;
                NotifyPropertyChanged("SelectedFilterTag");
            }
        }
        private string _selectedFilterTag = "";

        private void RefreshData()
        {
            CountAllTags = RefreshTag("");
            CountFilterTags = CountAllTags;
            NotifyPropertyChanged("CountAllTags");
            NotifyPropertyChanged("CountFilterTags");

            ListaAppunti = new ObservableCollection<DBAppunto>(DBCtrl.GetAppunti(""));
            CountAllAppunti = ListaAppunti.Count;
            CountFilterAppunti = CountAllAppunti;
            NotifyPropertyChanged("CountAllAppunti");

            SelectedFilterTag = "";
        }

        private int RefreshTag(string query)
        {
            VisibleTags = controls.Tag.FromArray(
                DBCtrl.GetTags(query),
                new Tag.TagSelectedDelegate(tag_Click));
            NotifyPropertyChanged("VisibleTags");
            return VisibleTags.Count;
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

        #region Events

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

        private void tag_Click(string tag)
        {
            SelectedFilterTag = tag;
            ListaAppunti = new ObservableCollection<DBAppunto>(DBCtrl.GetAppuntiByTag(SelectedFilterTag));
            CountFilterAppunti = ListaAppunti.Count;
        }

        private void txtTag_Change(string newValue)
        {
            CountFilterTags = RefreshTag(newValue);
            NotifyPropertyChanged("CountFilterTags");
        }

        private void txtAppunti_Change(string filter)
        {
            ListaAppunti = new ObservableCollection<DBAppunto>(DBCtrl.GetAppunti(filter));
            CountFilterAppunti = ListaAppunti.Count;
            SelectedFilterTag = "";
        }

        private void btnClearFilterTag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedFilterTag = "";
            ListaAppunti = new ObservableCollection<DBAppunto>(DBCtrl.GetAppunti(""));
            CountFilterAppunti = ListaAppunti.Count;
        }

        #endregion Events

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
