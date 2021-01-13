using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class AdminService
    {
        public List<ActionOperationLogModel> GetActionOperationLogLists()
        {
            string sql = @" select * from SJActionOperationLog; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ActionOperationLogModel>(sql).ToList();
            }
        }

        public List<MainMenuModel> GetMainMenuLists(UserModel user )
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", user.ID, DbType.Int32, ParameterDirection.Input);
            dp.Add("@SuperAdmin", user.SuperAdmin, DbType.Boolean, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
              return  connection.Query<MainMenuModel>("SJGetUserPagesProc", dp, null,true, null, CommandType.StoredProcedure).ToList();
            }
        }

        public UserInformationModel GetUserIndexPage(UserModel user)
        {
            string sql = @" select * from SJUserInfomation where UserId = @UserId; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return  connection.Query<UserInformationModel>(sql,new { UserId =user.ID}).FirstOrDefault();
              
            }
        }
    }
}
