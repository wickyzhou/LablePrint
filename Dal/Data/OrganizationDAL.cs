using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data
{
   public class OrganizationDAL
    {
        public IEnumerable<OrganizationModel> GetOrganization()
        {
            DataTable data = SqlHelper.ExecuteDataTable(@" select * from SJOrganization ", null);
            return SqlHelper.DataTableToModelList<OrganizationModel>(data);
        }

        public IEnumerable<OrganizationModel> GetOrganization(int orgId)
        {
            DataTable data = SqlHelper.ExecuteDataTable(@" select * from SJOrganization where id=@Id ", new SqlParameter[] { new SqlParameter("@Id",orgId) }) ;
            return SqlHelper.DataTableToModelList<OrganizationModel>(data);
        }
    }
}
