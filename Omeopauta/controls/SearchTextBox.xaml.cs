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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Omeopauta.controls
{
    /// <summary>
    /// Logica di interazione per SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl
    {
        public String SearchText { get; set; }
        public delegate void TextChangeDelegate(string newValue);
        public TextChangeDelegate delegateLateTextChange = null;

        public SearchTextBox()
        {
            InitializeComponent();
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && delegateLateTextChange != null)
                delegateLateTextChange(SearchText);
        }
        private void btnSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (delegateLateTextChange != null)
                delegateLateTextChange(SearchText);
        }

        private void btnDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchText = "";
            if (delegateLateTextChange != null)
                delegateLateTextChange(SearchText);

        }
    }
}
