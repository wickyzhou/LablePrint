
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Ui.MVVM.Entity;
using Ui.MVVM.Service;

namespace Ui.MVVM.Converter
{
    public class OrganizationConverter : IValueConverter
    {

        public IList<OrganizationEntity> Org { get; set; }

        public OrganizationConverter()
        {
            Org = new OrganizationService().GetAllOrganizations();
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Org.FirstOrDefault(m=>m.Id==(int)value).ShortName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
