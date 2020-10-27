using Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ui.Converter
{
    public class WMDeliveryButtonVisibilityConverter : BaseMultiValueConverter<WMDeliveryButtonVisibilityConverter>
    {

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {   
            if(values[0] == null || values[1]==null)
                return Visibility.Hidden;

            int sourceStockId = (values[0] as BatchBomRequestSummaryModel).StockId;
            int targetStockId = System.Convert.ToInt32((values[1] as Button).Tag);

            if (sourceStockId == targetStockId)
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
