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
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool _formActive = true;
        public bool FormActive
        {
            get { return _formActive; }
            set {
                _formActive = value;
                NotifyPropertyChanged("FormActive");
            } }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VisibleTag = new ObservableCollection<Tag>( DBCtrl.GetTags("") );
            NotifyPropertyChanged("VisibleTag");

            txtSearchBox.delegateLateTextChange = new SearchTextBox.TextChangeDelegate(TextChange);
        }

        private List<Tag> lstTag = new List<Tag>();

        public ObservableCollection<Tag> VisibleTag { get; set; }

        private void TextChange(string newValue)
        {
            VisibleTag = new ObservableCollection<Tag>(DBCtrl.GetTags(newValue));
            NotifyPropertyChanged("VisibleTag");
        }
        private void btnEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void btnAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrmEdit frm = new FrmEdit();
            frm.Owner = this;

            FormActive = false;
            frm.ShowDialog();
            FormActive = true;
        }
    }
}
