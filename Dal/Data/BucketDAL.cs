
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Dal
{
    public class BucketDAL
    {
        public List<BucketModel> GetAllBucket()
        {
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT * FROM  SJPackageBucketInfo ORDER  BY FBucketID DESC ", null);
            return SqlHelper.DataTableToModelList<BucketModel>(data);
        }
            

        public int BucketModify(BucketModel bucket)
        {
            return SqlHelper.ExecuteNonQuery(" UPDATE SJPackageBucketInfo SET FName=@FName,FWeight=@FWeight,FOffset=@FOffset,FKeyword=@FKeyword WHERE FBucketID=@FBucketID"
                    , new SqlParameter[] { new SqlParameter ("@FName",SqlHelper.SqlNull(bucket.FName ))
                                            ,new SqlParameter ("@FWeight", SqlHelper.SqlNull(bucket.FWeight))
                                            ,new SqlParameter ("@FOffset",SqlHelper.SqlNull(bucket.FOffset))
                                            ,new SqlParameter ("@FBucketID", bucket.FBucketID)
                                            ,new SqlParameter ("@FKeyword", SqlHelper.SqlNull(bucket.FKeyword) )
                    });
        }

        public int SyncBucketInfo()
        {
            return SqlHelper.ExecuteNonQuery(@" INSERT INTO SJPackageBucketInfo(FBucketID,FName,FWeight,FOffset,FKeyword)
                                                SELECT FInterID, FName, NULL, NULL,NULL FROM t_SubMessage 
                                                WHERE FTypeID = 10004  AND NOT EXISTS(SELECT 1 FROM SJPackageBucketInfo WHERE FBucketID = t_SubMessage.FInterID)",null);
        }

        public int GetNewBucketCount()
        {
            return (int)SqlHelper.ExecuteScalar(@"  SELECT Count(1) FROM t_SubMessage WHERE FTypeID = 10004  AND NOT EXISTS(SELECT 1 FROM SJPackageBucketInfo WHERE FBucketID = t_SubMessage.FInterID) ");
        }
    }
}
