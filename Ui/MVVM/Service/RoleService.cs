using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;

namespace Ui.MVVM.Service
{
    public class RoleService
    {
        public bool Insert(RoleEntity role)
        {
            string sql = @"insert into SJRole(ID,Name) values(@ID,@Name)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, role) > 0;
            }
        }

        public bool Update(RoleEntity role)
        {
            string sql = @"update SJRole set Name=@Name  where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, role) > 0;
            }
        }

        public bool Delete(int id)
        {
            string sql = @"delete from SJRole where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }

        public IList<RoleEntity> GetAllRoles()
        {
            string sql = @"select * from SJRole";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<RoleEntity>(sql).ToList();
            }
        }
    }
}
