using Dal;
using Model;
using System;
using System.Collections.Generic;

namespace Bll.Services
{
    public class ProductiveTaskListService
    {
        private readonly ProductiveTaskListDAL dal=new ProductiveTaskListDAL();

        public List<ProductiveTaskListModel> GetAllProductiveTaskList(DateTime productionDate, string type)
        {
            return dal.GetAllProductiveTaskList(productionDate, type);
        }
        
        public List<ProductiveTaskListModel> GetAllProductiveTaskListByDate(DateTime date)
        {
            return dal.GetAllProductiveTaskListByDate(date);
        }

        public object SyncProductiveTaskList(DateTime productionDate)
        {
            return dal.SyncProductiveTaskList(productionDate);
        }

        public int ModifyProductiveTaskList(ProductiveTaskListModel model)
        {
            return dal.ModifyProductiveTaskList(model);
        }

        public List<ProductionClassModel> GetAllType()
        {
            return dal.GetAllType();
        }

        public object AuditProductiveTaskList(DateTime productionDate)
        {
            return dal.AuditProductiveTaskList(productionDate);
        }

        public void ClearIncrement()
        {
             dal.ClearIncrement();
        }

        public string ModifyBillDateMonthly(string batches,DateTime productionDate,DateTime newDate,int userId)
        {
           int count= Convert.ToInt32(dal.ModifyBillDateMonthly(batches, productionDate, newDate, userId));
            if (count > 0)
                return $" 成功修改【 {count} 】条记录";
            return null;
        }

        public string VerifyICMOOrder(DateTime productionDate)
        {
            return dal.VerifyICMOOrder(productionDate);
        }
    }
}
