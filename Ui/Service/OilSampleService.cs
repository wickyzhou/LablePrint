using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class OilSampleService
    {
        public IList<OilSampleFlowModel> GetOilSampleFlow()
        {
            string sql = @" select *  from SROilSampleFlowView  order by title  ;";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleFlowModel>(sql).ToList();
            }
        }

        public IList<ExpressPrintModel> GetExpressPrintData(double id)
        {
            string sql = @"select * from SRExpressPrintView where Id=@Id ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<ExpressPrintModel>(sql,new {Id=id}).ToList();
            }
        }

        public IList<OilSampleEntryModel> GetOilSampleEntries(double id)
        {
            string sql = @" select *,(select sum(PrintWeight)  from SROilSampleFlowPrintLog where FormmainId=@Id) PrintedWeight
                            from SROilSamplePrintView where  FormmainId=@Id  order by EntryId";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleEntryModel>(sql, new { Id = id }).ToList();
            }
        }

        public IList<OilSampleFlowPrintLogModel> GetOilSampleFlowLog()
        {
            string sql = @" select top 100 * from SROilSampleFlowPrintLog order by Id desc";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleFlowPrintLogModel>(sql).ToList();
            }
        }

        public bool DeleteOilSampleFlowLog(double id)
        {
            string sql = @" delete from SROilSampleFlowPrintLog where Id=@Id ; ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Execute(sql, new { Id=id}) > 0;
            }
        }

        public bool InsertOilSampleFlowLog(OilSampleFlowPrintLogModel model)
        {
            string sql = @" insert into SROilSampleFlowPrintLog(FormmainId,Title,TypeId,TypeDesc,PrintCount,PrintWeight,BatchNo) values(@FormmainId,@Title,@TypeId,@TypeDesc,@PrintCount,@PrintWeight,@BatchNo) ; ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        //public bool UpdateOilSampleEntry(OilSampleEntryModel model)
        //{
        //    string sql = @"  ; ";
        //    using (var connection = SqlDb.UpdateConnectionOa)
        //    {
        //        return connection.Execute(sql, model) > 0;
        //    }
        //}
    }
}
