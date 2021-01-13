using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Data;

namespace Ui.Converter
{
    public class AMainMenuIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return HttpUtility.HtmlDecode("&#xe67e;"); 

            return HttpUtility.HtmlDecode(System.Convert.ToString(value)); 

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
