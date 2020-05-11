using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;

namespace Ui.MVVM.Service
{
    public class UserRoleService
    {
        public bool Insert(UserRoleEntity userRole)
        {
            string sql = @"insert into SJUserRole(UserId,RoleId) values(@UserId,@RoleId)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, userRole) > 0;
            }
        }

        public bool Update(UserRoleEntity userRole)
        {
            string sql = @"update SJUserRole set UserId=@UserId,RoleId=@RoleId where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, userRole) > 0;
            }
        }

        public bool Delete(Guid id)
        {
            string sql = @"delete from SJUserRole where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }

        public IList<UserRoleEntity> GetAllUserRoles()
        {
            string sql = @" select a.Id,a.UserId,b.UserName,a.RoleId,c.Name RoleName from SJUserRole a join SJUser b on a.UserId=b.ID join  SJRole c on a.RoleId=c.ID;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<UserRoleEntity>(sql).ToList();
            }
        }

        public IList<UserRoleEntity> GetUserRoleByUserId(int userId)
        {
            string sql = @" select a.Id,a.UserId,b.UserName,a.RoleId,c.Name RoleName from SJUserRole a join SJUser b on a.UserId=b.ID join  SJRole c on a.RoleId=c.ID where a.userId=@UserId;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<UserRoleEntity>(sql, new { UserId = userId }).ToList();
            }
        }
    }
}
