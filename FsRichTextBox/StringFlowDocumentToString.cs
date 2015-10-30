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
            /* This converter does not insert returns or indentation into the XAML. If you need to 
             * indent the XAML in a text box, see http://www.knowdotnet.com/articles/indentxml.html */

            // Exit if FlowDocument is null
            if (value == null) return string.Empty;

            // Get flow document from value passed in
            var flowDocument = (FlowDocument)value;

            // Convert to XAML and return
            return XamlWriter.Save(flowDocument);
        }

        #endregion
    }
}
