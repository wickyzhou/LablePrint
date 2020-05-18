using Common;
using Dal;
using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;

namespace Bll.Services
{
    public class LabelPrintService
    {

        private readonly LabelPrintDAL dal = new LabelPrintDAL();
        private readonly UserModel user;
        private readonly DateTime date;

        public LabelPrintService(DateTime date,UserModel user)
        {
            this.user = user;
            this.date = date;
        }

        public LabelPrintService()
        {

        }

        #region 主页面操作
        // 按日期查询历史打印数据
        public List<LabelPrintHistoryModel> GetAllLabelPrintHistoryDataByDate(DateTime date, int userId)
        {
            return dal.GetLabelPrintHistoryDataByDate(date, userId).ToList();
        }

        // 按日期获取当前打印数据
        public IEnumerable<LabelPrintCurrencyModel> GetCurrencyPrintingDataByDate(DateTime date)
        {
            return dal.GetCurrencyPrintingDataByDate(date).OrderByDescending(m => m.CreateTime).ThenBy(m => m.BatchNo);
        }

        // 获取当前用户所有未打印的数据
        public IEnumerable<LabelPrintCurrencyModel> GetCurrentPrintData(int userID)
        {
            return dal.GetPrintResultRecord(date, user.ID, "未打印");
        }

        //单次添加用户未打印排重
        public string AddCurrentPrintData(List<int> list, DateTime date)
        {
            if (list.Count > 0)
            {
                List<int> uniqueKey = dal.GetNotPrintedUniqueKey(user.ID,date);
                List<int> addIds = list.FindAll(x => !uniqueKey.Contains(x));
                if (addIds.Count == 0)
                {
                    return "添加成功，没有符合查询方案条件的【 新数据 】\r\n";
                }
                string ids = string.Join(",", addIds);
                int result = dal.AddCurrentPrintData(ids, user.ID,date);
                return $"成功添加{result}条数据 \r\n";
            }
            return "没有符合查询方案条件的数据 \r\n";
        }


        //批添加用户所有数据排重
        public string AddCurrentPrintDataBatch(List<int> list,DateTime date)
        {
            if (list.Count > 0)
            {
                List<int> uniqueKey = dal.GetUserUniqueKey(user.ID, date);
                List<int> addIds = list.FindAll(x => !uniqueKey.Contains(x));
                if (addIds.Count == 0)
                {
                    return "添加成功，没有符合查询方案条件的【 新数据 】\r\n";
                }
                string ids = string.Join(",", addIds);
                int result = dal.AddCurrentPrintData(ids, user.ID, date);
                return $"成功添加{result}条数据 \r\n";
            }
            return "没有符合查询方案条件的数据 \r\n";
        }






        public int ModifyHistoryDataByID(int id, int count,string seq)
        {
            return dal.ModifyLastPrintBucketByID(id, count, seq);
        }

        public string AddQuerySchemaEntry(QuerySchemaEntryModel model)
        {
            return dal.AddQuerySchemaEntry(model) == 1 ? null : "插入失败";
        }

        public IEnumerable<QuerySchemaModel> GetQuerySchemaModelByUserId(int userId)
        {
            return dal.GetQuerySchemaModelByUserId(userId);
        }

        public string IfEntryExists(int userId, QuerySchemaEntryModel inputModel)
        {
            var entries = dal.GetSchemaEntryByUserIdAndType(userId,inputModel.IsConditionOut);
            var item = entries.FirstOrDefault(m => m.OrgId == inputModel.OrgId && m.Label == inputModel.Label && m.BatchNo == inputModel.BatchNo && m.ProductionModel == inputModel.ProductionModel && m.BtnN == inputModel.SchemaId);
            if (item != null)
            {
                return "条件已存在";
            }
            return null;
        }

        //获取最新日期的打印数据重新生成
        public void ReGenPrintData()
        {
            dal.ReGenPrintData();
        }

        //获取用户模板查询明细
        public List<QuerySchemaEntryModel> GetQuerySchemaEntryBySchemaId(int schemaId)
        {
           return dal.GetQuerySchemaEntryBySchemaId(schemaId).ToList(); 
        }

        public void BatchInsertSchema(List<QuerySchemaModel> lists,string destinationTableName)
        {
                DataTable dt = ConvertHelper.ToDataTable<QuerySchemaModel>(lists);
                dal.BatchInsertSchema(dt, destinationTableName);  
        }


        // 记录打印参数
        public string SavePrintSchemaParameter(PrintSchemaParameterModel model)
        {
            var count= dal.SavePrintSchemaParameter(model);
            if (count != 1)
                return "保存打印参数失败";
            return null;
        }


        public PrintSchemaParameterModel GetLastPrintSchemaParameter(int userId,int schemaId)
        {
           return dal.GetPrintSchemaParameter(userId, schemaId).OrderByDescending(m => m.Id).FirstOrDefault();
        }

        #endregion

        #region 打印记录界面
        public int DeletePrintLog(List<int> list)
        {

            return dal.DeletePrintLog(string.Join(",", list));
        }


        public string DeleteCurrentPrintData(int id)
        {
            int count = dal.DeleteCurrentPrintData(id);
            if (count == 1)
                return "成功删除一条数据";
            return null;
        }

        public string BatchDeleteCurrentPrintData(List<int> list)
        {
            int count = dal.BatchDeleteCurrentPrintData(string.Join(",", list));
            return count>0?$"成功删除{count}条数据":null;
        }

        public string BatchDeleteCurrentPrintData(DateTime date, int userId)
        {
            int count = dal.BatchDeleteCurrentPrintData(date, userId);
            if (count > 0)
            {
                return $"成功删除{count}条数据";
            }
            return null;
        }

        public IEnumerable<LabelPrintCurrencyModel> GetUserData(DateTime date,int userId,string status)
        {
            if (status=="未打印")
                return dal.GetPrintResultRecord(date, userId, status).OrderBy(m=>m.PrintOrder);
            return dal.GetPrintResultRecord(date, userId, status);
        }

        public void ModifyCurrency(int id,int printOrder)
        {
            dal.ModifyCurrency(id, printOrder);
        }

        #endregion

        #region 通用调整界面

        public IEnumerable<LabelPrintCommonAdjustmentModel> GetAllLabelPrintCommonAdjustment()
        {
            return dal.GetAllLabelPrintCommonAdjustment();
        }

        public string DeleteCommonAdjustment(int id)
        {
            int r = dal.DeleteCommonAdjustment(id);
            if (r == 1)
                return "删除成功";
            else
                return "删除失败";

        }

        public int AddCommonAdjustment(LabelPrintCommonAdjustmentModel model)
        {   
             return Convert.ToInt32(dal.AddCommonAdjustment(model));
        }

        public int UpdateCommonAdjustment(LabelPrintCommonAdjustmentModel model)
        {
            return dal.UpdateCommonAdjustment(model);
        }
        #endregion

        #region 特殊要求界面
        public IEnumerable<LabelPrintSpecialRequestModel> GetAllSpecialRequest()
        {
            return dal.GetAllSpecialRequest().OrderByDescending(m => m.IdentityType)
                                                            .ThenByDescending(m => m.RequestSeq)
                                                            .ThenByDescending(m => m.OrgID)
                                                            .ThenByDescending(m => m.Label);
        }

        public string AddRequestName(CbRequestNameModel model)
        {
            var enums = new CommonDAL().GetEnumModels();
            var entry = enums.Where(m=>(m.ItemSeq==model.RequestSeq || m.ItemValue==model.RequestName)&& m.GroupSeq==2);
            if (entry.Count() > 0)
            {
                return " 此记录已存在，请重新打开界面";
            }
            else
            {
                int count =  dal.AddEnumModel(new EnumModel { 
                    ItemSeq= model.RequestSeq,
                    ItemValue= model.RequestName,
                    GroupSeq = 2,
                    GroupName= "客户特殊要求标签名",
                    ParentGroupSeq=0
                });
                if(count == 1)
                return null;
                return "新增失败";
            }
        
        }

        public string UpdateLabelPrintSpecialRequestModel(int id,string value,string label)
        {
            //if (string.IsNullOrEmpty(value))
            //{
            //    value = " ";
            //}
            int count = dal.UpdateLabelPrintSpecialRequestModel(id,value, label);
            if (count == 1)
                return null;
            return "更新失败";
        }

        public int AddSpecialRequest(LabelPrintSpecialRequestModel model)
        {
            return Convert.ToInt32(dal.AddSpecialRequest(model));
        }

        public string DeleteSpecialRequest(int id)
        {
            int count= dal.DeleteSpecialRequest(id);
            if (count == 1)
                return null;
            return "删除失败";
        }
        #endregion

        #region 方案配置界面

        public IEnumerable<QuerySchemaModel> GetAllSchemaName(int userId)
        {
            return dal.GetAllSchemaName(userId).OrderBy(m=>m.SchemaSeq);
        }

        public int AddQuerySchemaName(QuerySchemaModel model)
        {
            return Convert.ToInt32(dal.AddQuerySchemaName(model));
    
        }

        public string UpdateQuerySchemaName(int id,string name)
        {
            int count = dal.UpdateQuerySchemaName(id,name);
            if (count == 1)
                return null;
            return "更新失败";
        }

        public string DeleteQuerySchema(int id)
        {
            int count = dal.DeleteQuerySchema(id);
            if (count>0)
                return null;
            return "删除失败";
        }

        public IEnumerable<QuerySchemaModel>  GetMySchema(int userId)
        {
            return dal.GetMySchema(userId);
        }

        public IEnumerable<QuerySchemaConfigModel> GetSchemaEntryByUserId(int userId)
        {
            return dal.GetSchemaEntryByUserId(userId).OrderBy(m => m.SchemaSeq);
        }

        public string DeleteQuerySchemaEntry(int entryId)
        {
            int count = dal.DeleteQuerySchemaEntry(entryId);
            if (count == 1)
            {
                return null;
            }
            return "删除失败";
        }
        #endregion


      
    }
}
