using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ui.Converter
{
    public class OilSampleFlowExpressPrintContentConverter : BaseValueConverter<OilSampleFlowExpressPrintContentConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "打印"; 
            return (int)value > 0 ? "打印" : "已打印";
        }
    }
}
