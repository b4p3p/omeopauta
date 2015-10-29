using System;
using System.Collections.Generic;
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
    public partial class FrmEdit : Window
    {
        public FrmEdit()
        {
            InitializeComponent();
        }

        private void btnUndo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
