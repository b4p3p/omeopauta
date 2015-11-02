using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Logica di interazione per FrmShowImage.xaml
    /// </summary>
    public partial class FrmShowImage : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public FrmShowImage(string posizione, string nome)
        {
            InitializeComponent();

            this.Posizione = posizione;
            this.Nome = nome;
            NotifyPropertyChanged("Posizione");
            NotifyPropertyChanged("Nome");
        }

        public string Nome { get; set; }
        public string Posizione { get; set; }

        private void btnFolder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DirectoryInfo dir = Directory.GetParent(Posizione);
            Process.Start("explorer.exe", @dir.FullName);
        }
        private void btnImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", @Posizione);
        }
    }
}
