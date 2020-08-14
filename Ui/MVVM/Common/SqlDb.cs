using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Ui.MVVM.Common
{
    public class SqlDb
    {
        private static string GetConnectionString()
        {
            return "Password=K3admin;Persist Security Info=True;User ID=sa;Initial Catalog=AIS20200115215325;Data Source=192.168.2.150;Pooling=true;Max Pool Size=40000;Min Pool Size=0;";  //ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private static string GetConnectionStringSR()
        {
            return "Password=K3admin;Persist Security Info=True;User ID=sa;Initial Catalog=AIS20170824161725;Data Source=192.168.2.150;Pooling=true;Max Pool Size=40000;Min Pool Size=0;"; //ConfigurationManager.ConnectionStrings["ConnectionSR"].ToString();
        }


        private static string GetConnectionStringOa()
        {
            return "Password=sokan,123;Persist Security Info=True;User ID=oa;Initial Catalog=v3x;Data Source=192.168.1.240;Pooling=true;Max Pool Size=40000;Min Pool Size=0;"; //ConfigurationManager.ConnectionStrings["ConnectionOa"].ToString();
        }

        public static IDbConnection UpdateConnection => new SqlServerConnection(GetConnectionString());

        public static IDbConnection UpdateConnectionSR => new SqlServerConnection(GetConnectionStringSR());

        public static IDbConnection UpdateConnectionOa => new SqlServerConnection(GetConnectionStringOa());

    }
}
