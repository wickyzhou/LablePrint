using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;

namespace Ui.MVVM.Service
{
    public class OrganizationService
    {
        public OrganizationEntity GetOrganizationById(int orgId)
        {
            string sql = @"select * from SJOrganization where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<OrganizationEntity>(sql, new { Id = orgId }).FirstOrDefault();
            
            }
        }

        public IList<OrganizationEntity> GetAllOrganizations()
        {
            string sql = @"select * from SJOrganization";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<OrganizationEntity>(sql).ToList();
            }
        }
    }
}
