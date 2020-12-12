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
    public class BucketService
    {

        public List<BucketModel> GetLists(string filter)
        {
            string sql = $" select *  from SJPackageBucketInfo where 1=1 {filter}";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<BucketModel>(sql).ToList();
            }
        }

        public int SyncBucketInfo()
        {
            using (var connection = SqlDb.UpdateConnection)
            {
               return connection.Execute("SJSyncBucketInfo", null, null, null, CommandType.StoredProcedure);
            }
        }

        public bool BucketModify(BucketModel model)
        {
            string sql = @" UPDATE SJPackageBucketInfo SET FName=@FName,FWeight=@FWeight,FOffset=@FOffset,FKeyword=@FKeyword WHERE FBucketID=@FBucketID";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql,model)>0;
            }
        }
    }
}
