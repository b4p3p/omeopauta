using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace FsWpfControls.FsRichTextBox
{
    [ValueConversion(typeof(string), typeof(FlowDocument))]
    public class StringFlowDocumentToString : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts from XAML markup to a WPF FlowDocument.
        /// </summary>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            /* See http://stackoverflow.com/questions/897505/getting-a-flowdocument-from-a-xaml-template-file */

            var flowDocument = new FlowDocument();
            //string v = string.IsNullOrEmpty((string)value) ? "<FlowDocument AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph></Paragraph></FlowDocument>" 
            //                                               : (string)value;
            if (value != null && !string.IsNullOrEmpty((string)value))
            {
                var xamlText = (string)value;
                flowDocument = (FlowDocument)XamlReader.Parse((string)value);
            }

            // Set return value
            return flowDocument;
        }

        /// <summary>
        /// Converts from a WPF FlowDocument to a XAML markup string.
        /// </summary>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            FlowDocument flowDocument = (FlowDocument)XamlReader.Parse((string)value);
            string text = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd).Text;
            text = text.Replace("\r\n", " ");
            return text;
        }

        #endregion
    }
}
