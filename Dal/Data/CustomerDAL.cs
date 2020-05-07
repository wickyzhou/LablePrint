using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Data
{
   public class CustomerDAL
    {
        public IEnumerable<CustomerModel> GetSongJingCustomers()
        {
            DataTable data = SqlHelper.ExecuteDataTable(@" select * from SJCustomerView ", null);
            return SqlHelper.DataTableToModelList<CustomerModel>(data);
        }

        public IEnumerable<CustomerModel> GetSongRunCustomers()
        {
            DataTable data = SqlHelper.ExecuteDataTableSR(@" select * from SRCustomerView  ", null);
            return SqlHelper.DataTableToModelList<CustomerModel>(data);
        }
    }
}
