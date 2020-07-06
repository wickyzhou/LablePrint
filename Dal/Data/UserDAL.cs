using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class UserDAL
    {
        public IEnumerable<UserModel> GetAllUsers()
        {
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT * FROM  SJUser ;", null);
            return SqlHelper.DataTableToModelList<UserModel>(data);
        }

        public int RecordLoginLog(int userId, string hostName)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(" Insert into SJUserLoginLog(UserID,LoginTime,LogoutTime,LoginHostName) Values(@UserId,default,NULL,@LoginHostName); select SCOPE_IDENTITY() as r "
                , new SqlParameter[] { new SqlParameter("@UserId", userId), new SqlParameter("@LoginHostName", hostName) }
                ));
        }

        public void RecordLogoutLog(int id, bool isSystemLogout)
        {
            Task.Factory.StartNew(() =>
            {
               SqlHelper.ExecuteNonQuery(" update SJUserLoginLog set IsSystemLogout=@IsSystemLogout,LogoutTime=GETDATE() where ID=@ID "
                                                , new SqlParameter[] { new SqlParameter("@ID", id), new SqlParameter("@IsSystemLogout", isSystemLogout) });
            });
            // Insert into SJUserLoginLog(UserID,LoginTime,LogoutTime,IsSystemLogout) Values(@ID,NULL,default,@IsSystemLogout)

        }

        public int ModifyUserPassword(int userId, string ps)
        {
            return SqlHelper.ExecuteNonQuery(" update SJUser set Password =@Password  where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", userId), new SqlParameter("@Password", ps) });
        }
    }
}
