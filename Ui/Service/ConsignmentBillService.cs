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
            string sql = @"select a.*,isnull(SelectedStatus,0) SelectedStatus,@UserId UserId from SJConsignmentBill a  left join SJConsignmentBillUserCurrencyOperation b  on a.BillNo=b.BillNo and b.UserId=@UserId  order by InterId desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillModel>(sql, new { UserId = userId }).ToList();
            }
        }

        public IList<ConsignmentBillModel> GetAllConsignmentBills(int userId, string filter)
        {
            string sql = @"select a.*,isnull(SelectedStatus,0) SelectedStatus,@UserId UserId from SJConsignmentBill a  left join SJConsignmentBillUserCurrencyOperation b  on a.BillNo=b.BillNo and b.UserId=@UserId  where 1=1  " + filter + " order by InterId desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillModel>(sql, new { UserId = userId }).ToList();
            }
        }


        public bool UpdateConsignmentBill(ConsignmentBillModel consignmentBill)
        {

            string sql = @" update SJConsignmentBill set CurrencyQuatity=@CurrencyQuatity  where InterId=@InterId ;";
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

            string sql = @"  delete from  SJConsignmentBillUserCurrencyOperation where   BillNo=@BillNo and UserId=@UserId;";
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
            string sql = @" update SJConsignmentBillEntry set ECurrencyQuatity=@ECurrencyQuatity,IsChecked=@IsChecked  where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entry) > 0;
            }
        }

        public bool ClearSelectedConsignmentBills(string ids)
        {
            string sql = @" delete from  SJConsignmentBillUserCurrencyOperation  where BillNo in (" + ids + ")";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }

        public bool MergeConsignmentBill(int userId, string ids, string billNos)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ConsignmentIds", ids, DbType.String, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@BillNos", billNos, DbType.String, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJGenShippingBillProc", dp, null, null, CommandType.StoredProcedure) > 0;
                //return connection.Execute(sql) > 0;
            }
        }

        public bool SyncConsignmentBill()
        {
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJGenConsignmentBillProc", null, null, null, CommandType.StoredProcedure) > 0;
                //return connection.Execute(sql) > 0;
            }
        }

        public string GetConsignmentBillLockOwner(string billNo, int userId)
        {
            string sql = @" select (select top 1 '【单据号:'+ BillNo+'】正在被用户：【'+UserName+'】编辑' from SJUser where ID=a.UserId) UserName  from  SJConsignmentBillUserCurrencyOperation a where BillNo=@BillNo and UserId <> @UserId ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { BillNo = billNo, UserId = userId }, null, null, null));
            }
        }

        public string GetConsignmentBillLockOwner(int userId, string billNos)
        {
            string sql = @" select (select top 1 '【单据号:'+ BillNo+'】正在被用户：【'+UserName+'】编辑' from SJUser where ID=a.UserId) UserName  from  SJConsignmentBillUserCurrencyOperation a where BillNo in(" + billNos + ") and UserId <> @UserId ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { UserId = userId }, null, null, null));
            }
        }

        public bool AddUserCurrencyOperation(int userId, string billNos)
        {
            string sql = @"  insert into SJConsignmentBillUserCurrencyOperation (BillNo,UserId,SelectedStatus) select  BillNo,@UserId,case when UndoQuatity=CurrencyQuatity then 2 else 1 end from  SJConsignmentBill  where BillNo in (" + billNos + ")";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { UserId = userId }) > 0;
            }
        }
        public bool RemoveUserCurrencyOperation(string billNos)
        {
            string sql = @"  delete from  SJConsignmentBillUserCurrencyOperation  where BillNo in (" + billNos + ")";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }


    }
}
