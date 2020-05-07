using Dal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data
{
    public class UserInformationDAL
    {
        public object GetDefaultPage(int userId)
        {
            return SqlHelper.ExecuteScalar("  SELECT DefaultPage FROM SJUserInfomation where UserId=@UserId ;", new SqlParameter[] { new SqlParameter("@UserId ", userId) });
        }
    }
}
