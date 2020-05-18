
using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Services
{
   public class QuerySchemaCurrentStatusService
    {
        private readonly QuerySchemaCurrentStatusDAL dal = new QuerySchemaCurrentStatusDAL();

        public List<QuerySchemaCurrentStatusModel> GetUserPageSizeList(int userId,DateTime productionDate)
        {
            return dal.GetUserPageSizeList(userId, productionDate).ToList();
        }

        public List<QuerySchemaCurrentStatusModel> GetUserSchemaList(int userId, DateTime productionDate)
        {
            return dal.GetUserSchemaList(userId, productionDate).ToList();
        }
    }
}
