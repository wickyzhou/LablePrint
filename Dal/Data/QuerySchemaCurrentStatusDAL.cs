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
   public class QuerySchemaCurrentStatusDAL
    {
        public IEnumerable<QuerySchemaCurrentStatusModel> GetUserPageSizeList(int userId, DateTime productionDate)
        {
            DataTable data = SqlHelper.ExecuteDataTable(" select PageSize,max(PageSizeStatus) PageSizeStatus  from SJUserQuerySchemaCurrentStatus where UserId=@UserId and ProductionDate=@ProductionDate group by PageSize "
                , new SqlParameter[] { new SqlParameter("@UserId", userId), new SqlParameter("@ProductionDate", productionDate) }

                );
            return SqlHelper.DataTableToModelList<QuerySchemaCurrentStatusModel>(data);
        }

        public IEnumerable<QuerySchemaCurrentStatusModel> GetUserSchemaList(int userId, DateTime productionDate)
        {
            DataTable data = SqlHelper.ExecuteDataTable(" select SchemaId,max(SchemaStatus) SchemaStatus from SJUserQuerySchemaCurrentStatus where UserId=@UserId and ProductionDate=@ProductionDate group by SchemaId"
                , new SqlParameter[] { new SqlParameter("@UserId", userId), new SqlParameter("@ProductionDate", productionDate) }

                );
            return SqlHelper.DataTableToModelList<QuerySchemaCurrentStatusModel>(data);
        }
    }
}
