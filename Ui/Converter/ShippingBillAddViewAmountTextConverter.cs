using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Ui.Converter
{
    public class ShippingBillAddViewAmountTextConverter : BaseMultiValueConverter<ShippingBillAddViewAmountTextConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return null;

            if ((int)values[0]==1 || (int)values[0] == 3)
            {
                return "系统计算";
            }
            return   values[1].ToString();
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            //List<object> results = new List<object>();
            //string str = value.ToString();
            //if (str == "系统计算")
            //    results.Add(); 
            return new object[] {1,200 };
        }
    }
}
