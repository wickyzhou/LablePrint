using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ui.Service;

namespace Ui.Converter
{
    public class WMDeliveryButtonVisibilityConverter : BaseMultiValueConverter<WMDeliveryButtonVisibilityConverter>
    {
        public WMDeliveryButtonVisibilityConverter()
        {
            Lists = new CommonService().GetDeliveryStock();
        }
        public List<DeliveryStockModel> Lists { get; set; }


        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {   
            if(values[0] == null || values[1]==null)
                return Visibility.Hidden;

            int targetStockId  = (values[0] as BatchBomRequestSummaryModel).StockId;
            int sourceStockId = System.Convert.ToInt32((values[1] as Button).Tag);

            if (!Lists.Exists(x=>x.StockId== sourceStockId && x.TargetStockId== targetStockId))
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
