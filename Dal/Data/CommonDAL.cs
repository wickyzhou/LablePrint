using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Data
{
   public class CommonDAL
    {
        public IEnumerable<EnumModel> GetEnumModels()
        {
            DataTable data = SqlHelper.ExecuteDataTable(@" SELECT * FROM SJEnumTable", null);
            return SqlHelper.DataTableToModelList<EnumModel>(data);
        }
    }
}
