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
    public class UserDAL
    {
        public IEnumerable<UserModel> GetAllUsers()
        {
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT * FROM  SJUser ;", null);
            return SqlHelper.DataTableToModelList<UserModel>(data);
        }

        public int RecordLoginLog(int id)
        {
            return SqlHelper.ExecuteNonQuery(" Insert into SJUserLoginLog(UserID,LoginTime,LogoutTime) Values(@ID,default,NULL)", new SqlParameter[] { new SqlParameter("@ID",id)}); 
        }

        public int RecordLogoutLog(int id)
        {
            return SqlHelper.ExecuteNonQuery(" Insert into SJUserLoginLog(UserID,LoginTime,LogoutTime) Values(@ID,NULL,default)", new SqlParameter[] { new SqlParameter("@ID", id) });
        }

        public int ModifyUserPassword(int userId,string ps)
        {
            return SqlHelper.ExecuteNonQuery(" update SJUser set Password =@Password  where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", userId) , new SqlParameter("@Password", ps) });
        }
    }
}
