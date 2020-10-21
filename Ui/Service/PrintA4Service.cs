
using Dapper;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Ui.Helper;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class PrintA4Service
    {
        public List<LabelPrintHistoryModel>  GetHistoryLists(string filter)
        {
            string sql = @" select ID,ProductiveTaskListID,WorkNo,T.BatchNo,ProductionModel,ProductionName,ProductionDate,ExpirationDate,ExpirationMonth,OrgID,Label,OrgCode,OrgBillNo,Package,
                              RoughWeight,NetWeight	,CheckNo,1 as PrintCount,ModifyTime,TwoDimensionCode,SelectCount,SelectTotalCount,SpecialRequest,
                              BucketCount,CaseName,RowHashValue,Status,Seq,SafeCode,FItemID,SpecificationValue,TwoDimensionCode1,TwoDimensionCode2,TwoDimensionCode3,TwoDimensionCode4,
                              Seq2678,IsPassed,SampleOilPrintCount,SampleOilPrintArea,SampleOilPrintProductionName,RowQuantity	,LastPrintTime,PrintedCount
                            from ( select *,ROW_NUMBER()over(partition by batchno order by ProductionDate,BatchNo,ID) rnk from SJLabelPrintHistory where 1=1 " + filter+@") as t
                                left join(select BatchNo, sum(PrintBucket) PrintedCount, MAX(PrintTime) LastPrintTime from SJLabelPrintA4Log group by  BatchNo) b on t.BatchNo = b.BatchNo
                            where rnk = 1";
         //   string sql = $" select * from SJLabelPrintHistoryView  where 1=1  {filter}";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<LabelPrintHistoryModel>(sql).ToList();
            }
        }

        public DataTable GetExportData1(string dataGridName,int userId,string filter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            DataTable dataTable = SqlHelper.ExecuteDataTable(@" select ColumnFieldName ,ColumnHeaderName from SJDataGridUserCustom where IsDownLoad = 1 and DataGridName = @DataGridName and UserId = @UserId       order by columnorder ;"
                    , new SqlParameter[] { new SqlParameter("@DataGridName", dataGridName), new SqlParameter("@UserId", userId) });
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                stringBuilder.Append($",{dataTable.Rows[i][0]} as [{dataTable.Rows[i][1]}]");
            }

            string fields = stringBuilder.ToString().Substring(1);
            string sql = $"  select {fields} from SJLabelPrintHistoryView  where 1=1 {filter} ";

            return SqlHelper.ExecuteDataTable(sql,null);
        }
    }
}
