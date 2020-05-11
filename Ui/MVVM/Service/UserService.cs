using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;

namespace Ui.MVVM.Service
{
    public class UserService
    {
        public bool Insert(UserEntity user)
        {
            string sql = @"insert into SJUser(LoginName,Password,UserName,OrgId,SuperAdmin) values(@LoginName,@Password,@UserName,@OrgId,@SuperAdmin)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, user) > 0;
            }
        }

        public bool Update(UserEntity user)
        {
            string sql = @"update SJUser set  Password=@Password,UserName=@UserName,OrgId=@OrgId,SuperAdmin=@SuperAdmin where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, user) > 0;
            }
        }

        public bool Delete(int id)
        {
            string sql = @"delete from SJUser where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }

        public IList<UserEntity> GetAllUsers()
        {
            string sql = @"select * from SJUser";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<UserEntity>(sql).ToList();
            }
        }
    }
}
