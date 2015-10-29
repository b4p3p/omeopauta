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
    /// Logica di interazione per Tag.xaml
    /// </summary>
    public partial class Tag : UserControl
    {
        public static readonly DependencyProperty text = DependencyProperty.Register("Text", typeof(string), typeof(UserControl));

        public Tag()
        {
            InitializeComponent();
        }

        public Tag(string text)
        {
            this.Text = text;
            InitializeComponent();
        }

        public string Text
        {
            get { return this.GetValue(text).ToString().ToUpper(); }
            set{ this.SetValue(text, value.ToUpper()); }
        }
    }
}
