using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Data;

namespace Ui.Converter
{
    public class AMainMenuFolderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var open = (bool)value;
            if(open)
                return HttpUtility.HtmlDecode("&#xe502;");
            return HttpUtility.HtmlDecode("&#xe62d;");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
