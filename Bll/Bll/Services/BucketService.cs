using Dal;
using Model;
using System.Collections.Generic;

namespace Bll.Services
{
    public class BucketService
    {
        private BucketDAL sJBucketDal = new BucketDAL();

        public List<BucketModel> GetAllBucket()
        {
            return sJBucketDal.GetAllBucket();
        }

        public int BucketModify(BucketModel sJBucket)
        {

            return sJBucketDal.BucketModify(sJBucket);
        }

        public int SyncBucketInfo()
        {
            return sJBucketDal.SyncBucketInfo();
        }

        public int GetNewBucketCount()
        {
            return sJBucketDal.GetNewBucketCount();
        }
    }
}
