using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using Ui.MVVM.Common;
using Ui.ViewModel;

namespace Ui.Service
{
    public class ConsignmentBillService
    {
        public IList<ConsignmentBillModel> GetAllConsignmentBills(int userId)
        {
            string sql = @"select a.*,isnull(SelectedStatus,0) SelectedStatus,@UserId UserId from SJConsignmentBill a  left join SJConsignmentBillUserCurrencyOperation b  on a.BillNo=b.BillNo and b.UserId=@UserId and b.InsEntryStatus=0  order by a.BillNo ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillModel>(sql, new { UserId = userId }).ToList();
            }
        }

        public IList<ConsignmentBillModel> GetAllConsignmentBills(int userId, string filter)
        {
            string sql = @"select a.*,isnull(SelectedStatus,0) SelectedStatus,@UserId UserId from SJConsignmentBill a  left join SJConsignmentBillUserCurrencyOperation b  on a.BillNo=b.BillNo and b.UserId=@UserId and b.InsEntryStatus=0 where 1=1  " + filter + " order by  a.BillNo";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillModel>(sql, new { UserId = userId }).ToList();
            }
        }


        public bool UpdateConsignmentBill(ConsignmentBillModel consignmentBill)
        {

            string sql = @" update SJConsignmentBill set CurrencyQuantity=@CurrencyQuantity,UndoQuantity=@UndoQuantity,TotalQuantity=@TotalQuantity  where InterId=@InterId ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, consignmentBill) > 0;
            }
        }

        public bool AddConsignmentBill(ConsignmentBillModel consignmentBill)
        {

            string sql = @" insert into SJConsignmentBillUserCurrencyOperation(BillNo,UserId,SelectedStatus) values(@BillNo,@UserId,@SelectedStatus);";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, consignmentBill) > 0;
            }
        }

        public bool DeleteConsignmentBill(ConsignmentBillModel consignmentBill)
        {

            string sql = @"  delete from  SJConsignmentBillUserCurrencyOperation where   BillNo=@BillNo and UserId=@UserId and Eid=-1;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, consignmentBill) > 0;
            }
        }


        public IList<ConsignmentBillEntryModel> GetAllConsignmentBillEntryById(int interId)
        {
            string sql = @" select *  from SJConsignmentBillEntry   where InterId=@InterId ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillEntryModel>(sql, new { InterID = interId }).ToList();
            }
        }

        public bool UpdateConsignmentBillEntry(ConsignmentBillEntryModel entry)
        {
            string sql = @" update SJConsignmentBillEntry set ECurrencyQuantity=@ECurrencyQuantity,IsChecked=@IsChecked  where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entry) > 0;
            }
        }

        public bool ClearSelectedConsignmentBills(string ids)
        {
            string sql = @" delete from  SJConsignmentBillUserCurrencyOperation  where BillNo in (" + ids + ") and EId=-1";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }

        public string MergeConsignmentBill(int userId, string ids, string billNos)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ConsignmentIds", ids, DbType.String, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@BillNos", billNos, DbType.String, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString( connection.ExecuteScalar("SJGenShippingBillProc", dp, null, null, CommandType.StoredProcedure));
            }
        }

        public bool SyncConsignmentBill(DateTime beginDate,DateTime endDate)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BeginDate", beginDate, DbType.Date, ParameterDirection.Input);
            dp.Add("@EndDate", endDate, DbType.Date, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJGenConsignmentBillProc", dp, null, null, CommandType.StoredProcedure) > 0;
                //return connection.Execute(sql) > 0;
            }
        }

        public string GetConsignmentBillLockOwner(string billNo, int userId)
        {
            string sql = @" select (select top 1 '【单据号:'+ BillNo+'】正在被用户：【'+UserName+'】编辑' from SJUser where ID=a.UserId) UserName  from  SJConsignmentBillUserCurrencyOperation a where BillNo=@BillNo and UserId <> @UserId and SelectedStatus>0 ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { BillNo = billNo, UserId = userId }, null, null, null));
            }
        }

        public string GetConsignmentBillLockOwner(int userId, string billNos)
        {
            string sql = @" select (select top 1 '【单据号:'+ BillNo+'】正在被用户：【'+UserName+'】编辑' from SJUser where ID=a.UserId) UserName  from  SJConsignmentBillUserCurrencyOperation a where BillNo in(" + billNos + ") and UserId <> @UserId  and SelectedStatus>0 ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { UserId = userId }, null, null, null));
            }
        }

        public string GetConsignmentBillLock(string billNos)
        {
            string sql = @" select (select top 1 '【单据号:'+ BillNo+'】 正在被用户：【'+UserName+'】锁定。请取消该单据选择状态，重试' from SJUser where ID=a.UserId) UserName  from  SJConsignmentBillUserCurrencyOperation a where BillNo in(" + billNos + ")  and SelectedStatus>0  ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, null, null, null, null));
            }
        }


        public bool AddUserCurrencyOperation(int userId, string billNos)
        {
            string sql = @"  insert into SJConsignmentBillUserCurrencyOperation (BillNo,UserId,SelectedStatus) select  BillNo,@UserId,case when UndoQuantity=CurrencyQuantity then 2 else 1 end from  SJConsignmentBill  where BillNo in (" + billNos + ")";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { UserId = userId }) > 0;
            }
        }

        public bool RemoveUserCurrencyOperation(string billNos)
        {
            string sql = @"  delete from  SJConsignmentBillUserCurrencyOperation  where BillNo in (" + billNos + ") and EId=-1";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }

        public IList<ConsignmentBillModel> GetUserSelectedConsignmentBill(int userId)
        {
            string sql = @" select *  from SJConsignmentBillUserCurrencyOperation   where UserId=@UserId and SelectedStatus>0 ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillModel>(sql, new { UserId = userId }).ToList();
            }
        }

        public int AddConsignmentBillEntry(ConsignmentBillEntryModel entry,int userId)
        {

            string sql = @" insert into  SJConsignmentBillEntry(InterId,EntryId,ItemId,ItemName,CaseId,CaseName,BrandId,BrandName,IsChecked,ETotalQuantity,EUndoQuantity,ECurrencyQuantity,IsSystem)
                            values(@InterId,@EntryId,@ItemId,@ItemName,@CaseId,@CaseName,@BrandId,@BrandName,@IsChecked,@ECurrencyQuantity,@ECurrencyQuantity,@ECurrencyQuantity,@IsSystem);Set @Id=SCOPE_IDENTITY();
                            insert into SJConsignmentBillUserCurrencyOperation (BillNo,UserId,InsEntryStatus,EId,InterId) select  BillNo," + userId.ToString() + @",1,@Id,InterId   from SJConsignmentBill where InterId=@InterId ;  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@InterId", entry.InterId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@EntryId", entry.EntryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@ItemId", entry.ItemId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@ItemName", entry.ItemName, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@CaseId", entry.CaseId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@CaseName", entry.CaseName, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@BrandId", entry.BrandId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@BrandName", entry.BrandName, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@IsChecked", entry.IsChecked, dbType: DbType.Boolean, direction: ParameterDirection.Input);
                p.Add("@ECurrencyQuantity", entry.ECurrencyQuantity, dbType: DbType.Single, direction: ParameterDirection.Input);
                p.Add("@IsSystem", entry.IsSystem, dbType: DbType.Boolean, direction: ParameterDirection.Input);
                connection.Execute(sql, p);
                return p.Get<int>("@Id");
            }
        }

        public bool DeleteConsignmentBillEntry(int id)
        {
            string sql = @" delete from SJConsignmentBillUserCurrencyOperation where EId=@Id ; delete from  SJConsignmentBillEntry where Id=@Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }

        public string GetConsignmentBillNosByShippingBillId(int id)
        {
            string sql = @" select stuff((select ','''+ConsignmentBillNo+'''' from SJShippingBillDetailLog where MainId = @Id for xml path('')),1,1,'') as name";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { Id = id }));
            }
        }

    }
}
