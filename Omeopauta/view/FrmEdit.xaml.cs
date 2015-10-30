using Omeopauta.controller;
using Omeopauta.model;
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
using System.Windows.Shapes;

namespace Omeopauta.view
{
    /// <summary>
    /// Logica di interazione per Edit.xaml
    /// </summary>
    public partial class FrmEdit : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isEdit;
        private AppuntoModel _appunto;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public AppuntoModel Appunto {
            get { return _appunto; }
            set { _appunto = value; }
        }

        public FrmEdit(bool isEdit)
        {
            InitializeComponent();

            this.isEdit = isEdit;
            Appunto = new AppuntoModel("", "", new string[] { });
            this.DataContext = Appunto;

            lblHeader.Content = isEdit ? "Modifica Nota" : "Nuova Nota";
        }

        private void btnUndo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.EditBox.UpdateDocumentBindings();
        }
        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.EditBox.UpdateDocumentBindings();
            DBCtrl.Save(Appunto);
            this.Close();
        }
    }
}
