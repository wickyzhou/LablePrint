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
            return ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private static string GetConnectionStringSR()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionSR"].ToString();
        }


        private static string GetConnectionStringOa()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionOa"].ToString();
        }

        public static IDbConnection UpdateConnection => new SqlServerConnection(GetConnectionString());

        public static IDbConnection UpdateConnectionSR => new SqlServerConnection(GetConnectionStringSR());

        public static IDbConnection UpdateConnectionOa => new SqlServerConnection(GetConnectionStringOa());

    }
}
