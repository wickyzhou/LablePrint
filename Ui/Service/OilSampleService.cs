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
    public class OilSampleService
    {   
        // 默认是待办
        public IList<OilSampleFlowModel> GetOilSampleFlow()
        {
            string sql = @" select *  from SROilSampleFlowView  order by title  desc ;";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleFlowModel>(sql).ToList();
            }
        }

        // 已办流程
        public IList<OilSampleFlowModel> GetOilSampleDealedFlow()
        {
            string sql = @" select distinct b.ID Id,a.SUBJECT+cast(b.ID as nvarchar(50)) Title 
                                    ,isnull((select max(PrintedCount) PrintCount from SROilSampleFlowPrintLog where TypeId=3 and FormmainId=b.ID),0)  ExpressPrintedCount
                            from CTP_AFFAIR a 
                            join formmain_1796 b on a.FORM_RECORDID=b.ID
                            where  a.NODE_NAME='松润总经办-人事专员' 
                                     and a.TEMPLETE_ID=4081300307003984148 -- 样油样板申请表I
                                     and a.state = 4 --待办
                                     and b.field0002=2612005562338331437 -- 样油
                                     and b.start_date>='2020-06-01'
                            order by title desc ; ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleFlowModel>(sql).ToList();
            }
        }





        public IList<OilSampleEntryModel> GetOilSampleEntries(decimal formmainId)
        {
            string sql = @" select Id,FormmainId,EntryId,ProductionName,ProductionModel,RoughWeight,WeightPerBucket,TotalWeight,ExpirationMonth,CheckNo,ProductionDate,BatchNo,
                                        PrintTotalCount, isnull(PrintedCount,0) PrintedCount,PrintTotalCount-isnull(PrintedCount,0) CurrencyPrintCount
                            from(	
                                    select *,ceiling(TotalWeight/WeightPerBucket ) PrintTotalCount
	                                        ,(select max(PrintedCount)  from SROilSampleFlowPrintLog where FormsonId=a.id) PrintedCount
                                    from SROilSamplePrintView a where  FormmainId=@FormmainId  ) 
                            as t
                            order by EntryId";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleEntryModel>(sql, new { FormmainId = formmainId }).ToList();
            }
        }

        public IList<OilSampleFlowPrintLogModel> GetOilSampleFlowLog()
        {
            string sql = @" select top 200 A.Id,FormmainId,isnull(A.Title,(select top 1 B.SUBJECT from CTP_AFFAIR  b WITH(NOLOCK) where TEMPLETE_ID=4081300307003984148 and FORM_RECORDID=A.FormmainId))Title,TypeId,TypeDesc,PrintCount,CreateTime,BatchNo,PrintedCount,FormsonId,EntryId 
                            from SROilSampleFlowPrintLog a 
                            order by A.Id desc ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleFlowPrintLogModel>(sql).ToList();
            }
        }

        public bool DeleteOilSampleFlowLog(decimal id)
        {
            string sql = @" delete from SROilSampleFlowPrintLog where Id=@Id ; ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }

        public int InsertOilSampleFlowLog(OilSampleFlowPrintLogModel model)
        {
            string sql = @" insert into SROilSampleFlowPrintLog(FormsonId,FormmainId,Title,TypeId,TypeDesc,PrintCount,BatchNo,PrintedCount,EntryId) values(@FormsonId,@FormmainId,@Title,@TypeId,@TypeDesc,@PrintCount,@BatchNo,@PrintedCount,@EntryId) ; select SCOPE_IDENTITY() as Id; ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return Convert.ToInt32(connection.ExecuteScalar(sql, model));
            }
        }

        public bool InsertOilSampleFlowLog2(OilSampleFlowPrintLogModel model)
        {
            string sql = @" insert into SROilSampleFlowPrintLog(FormsonId,FormmainId,Title,TypeId,TypeDesc,PrintCount,BatchNo,PrintedCount,EntryId) values(@FormsonId,@FormmainId,@Title,@TypeId,@TypeDesc,@PrintCount,@BatchNo,@PrintedCount,@EntryId) ; ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Execute(sql, model) > 0;
            }
        }


        public bool UpdateOilSampleFlowLog(OilSampleFlowPrintLogModel model)
        {
            string sql = @" update SROilSampleFlowPrintLog set  PrintedCount= @PrintedCount where Id=@Id ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool UpdateOilSampleFlowLog(decimal formsonId)
        {
            string sql = @" update SROilSampleFlowPrintLog set  PrintedCount= 0 where FormsonId=@FormsonId ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Execute(sql, new { FormsonId = formsonId }) > 0;
            }
        }


        public OilSampleExpressPrintModel GetExpressPrintData(decimal id)
        {
            string sql = @" select SendName,SendPhone,SendCompanyName,SendAddress,ContractMan,ContractPhone,ContractCompanyName,ContractAddress from  SRExpressPrintView where Id=@Id ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleExpressPrintModel>(sql, new { Id = id }).FirstOrDefault();
            }
        }

        public OilSampleExpressPrintModel GetOilSamplePrintData(decimal formsonId)
        {
            string sql = @" select *from SROilSamplePrintView where Id=@FormsonId ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleExpressPrintModel>(sql, new { FormsonId = formsonId }).FirstOrDefault();
            }
        }


        public string GetOilSampleEntryBatchNo(decimal formsonId)
        {
            string sql = @" select BatchNo from SROilSampleFlowPrintLog where FormsonId=@FormsonId ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { FormsonId = formsonId }));
            }
        }
    }
}
