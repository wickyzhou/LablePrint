using Bll.Services;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using Ui.Service;

namespace Ui.Converter
{
    public class ShippingBillUserIdConverter : BaseValueConverter<ShippingBillUserIdConverter>
    {
        public IEnumerable<UserModel> Users { get; set; }

        
        public ShippingBillUserIdConverter()
        {
             Users = new UserService().GetAllUsers();
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

          UserModel User=  (MemoryCache.Default["UserCache"] as UserCacheModel).User;
            if (value == null)
                return null;
            return  Users.FirstOrDefault(x => x.ID == (int)value).UserName;
        }
}
}
