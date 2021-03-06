﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Ui.Converter
{
    public class OilSampleTemplatePage4BackgroundConverter : BaseValueConverter<OilSampleTemplatePage4BackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {   
            if(value==null)
                return new SolidColorBrush(Colors.LightGray);

            int status = int.Parse(value.ToString().Substring(0,1));
            if (status==4 )
                    return new SolidColorBrush((Color)Application.Current.Resources["GenericRedColor"]);

            return new SolidColorBrush(Colors.LightGray);
        }
    }
}
