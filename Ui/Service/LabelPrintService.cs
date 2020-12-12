using Dapper;
using Model;
using System.Collections.Generic;
using System.Linq;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class LabelPrintService
    {
        public List<PrintCommonAdjustmentModel> GetLists(string filter)
        {
            string sql = $" select a.* from SJPrintCommonAdjustment a join SJEnumTable b on a.TypeId = b.Id  where 1=1 {filter}";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<PrintCommonAdjustmentModel>(sql).ToList();
            }
        }

        // 存储过程
        /* connection.Query<SysUser, SysUserInfo, SysUser>(
                     Constant.ProcGetList,
                     (u, ui) =>
                         {
                             u.UserInfo = ui;
                             return u;
                         },
                         p,splitOn:"SysId",
                     commandType: CommandType.StoredProcedure); */


        public bool DeletePrintCommonAdjustment(int id)
        {
            string sql = $" delete from SJPrintCommonAdjustment where Id = @Id ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql,new { Id = id })>0;
            }
        }

        public bool DeletePrintCommonAdjustment(string ids)
        {
            string sql = $" delete from SJPrintCommonAdjustment where Id in ({ids}) ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }
    }
}
