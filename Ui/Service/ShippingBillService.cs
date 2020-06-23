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
    public class ShippingBillService
    {
        public IList<ShippingBillModel> GetAllShippingBills()
        {
            string sql = @"select * from SJShippingBill order by Id desc ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillModel>(sql).ToList();
            }
        }

        public IList<ShippingBillEntryModel> GetAllShippingBillEntryById(int id)
        {
            string sql = @" select * from SJShippingBillEntry  where MainId=@Id ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillEntryModel>(sql, new { Id = id }).ToList();
            }
        }

        public bool UpdateShipingBill(ShippingBillModel billModel)
        {
            string sql = @" update SJShippingBill 
                        set BillDate=@BillDate  ,LogisticsType=@LogisticsType  ,LogisticsCompanyName=@LogisticsCompanyName,
                            YunShuFei=@YunShuFei  ,YouFei=@YouFei  ,GuoLuFei=@GuoLuFei  ,ChaiLvFei=@ChaiLvFei  ,WeiXiuFei=@WeiXiuFei ,
                            Demander=@Demander, OtherCosts=@OtherCosts,TotalAmount=@TotalAmount,
                            Note=@Note,LogisticsBillNo=@LogisticsBillNo,SystemApportionedAmount=@SystemApportionedAmount,
                            GuoNeiDuanFeiYong=@GuoNeiDuanFeiYong,GuoJiDuanFeiYong=@GuoJiDuanFeiYong,YunShuDuanFeiYong=@YunShuDuanFeiYong
                        where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, billModel) > 0;
            }
        }

        public bool UpdateShipingBillEntry(ShippingBillModel billModel)
        {
            string sql = @" update SJShippingBillEntry set ApportionedAmount= Quantity/SystemQuantity*@SystemApportionedAmount where MainId=@Id and IsSystem=1  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, billModel) > 0;
            }
        }

        public IList<ShippingBillDetailLogModel> GetAllShippingBillDetails(int mainId)
        {
            string sql = @"select * from SJShippingBillDetailLog where MainId=@MainId ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillDetailLogModel>(sql, new { MainId = mainId }).ToList();
            }
        }

        public IList<ShippingBillExportModel> GetExprotShippingBill()
        {
            string sql = @"select TotalQuantity,TotalAmount, BillNo,BillDate,
                            (select ItemValue from  SJEnumTable where GroupSeq=3 and ItemSeq = a.LogisticsType) LogisticsTypeName,
                            LogisticsCompanyName,LogisticsBillNo,YunShuFei,YouFei,GuoLuFei,ChaiLvFei,WeiXiuFei,Demander,OtherCosts,Note	,
                             (select ItemValue from  SJEnumTable where GroupSeq=4 and ItemSeq = a.GoodsType) GoodsTypeName ,
							 GuoNeiDuanFeiYong,GuoJiDuanFeiYong,YunShuDuanFeiYong
                            EntryId,CaseName,BrandName,DeptName,CustName,Quantity,ApportionedAmount from  SJShippingBill a join SJShippingBillEntry b on a.Id=b.MainId  
                        order by BillNo,EntryId ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillExportModel>(sql).ToList();
            }
        }

        public bool AddShipingBillEntry(ShippingBillEntryModel entryModel)
        {
            string sql = @" insert into SJShippingBillEntry(MainId,CaseName,Quantity,BrandName,DeptName,CustName,DeptId,CustId,BrandId,CaseId,EntryId,ApportionedAmount,GoodsType,Note)
select @MainId,(select ItemValue from SJEnumTable where GroupSeq=4 and ItemSeq=@GoodsType),@Quantity,@BrandName,@DeptName,@CustName,@DeptId,@CustId,@BrandId,@CaseId,@EntryId,@ApportionedAmount,@GoodsType,@Note;";
            switch (entryModel.GoodsType)
            {
                case 2: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @Quantity, TotalAmount = isnull(TotalAmount, 0) + @ApportionedAmount,HaoCaiFei= isnull(HaoCaiFei,0)+@ApportionedAmount  where Id = @MainId "; break;
                case 3: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @Quantity, TotalAmount = isnull(TotalAmount, 0) + @ApportionedAmount,YangYouFei= isnull(YangYouFei,0)+@ApportionedAmount  where Id = @MainId "; break;
                case 4: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @Quantity, TotalAmount = isnull(TotalAmount, 0) + @ApportionedAmount,SheBeiFei= isnull(SheBeiFei,0)+@ApportionedAmount  where Id = @MainId "; break;
                case 5: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @Quantity, TotalAmount = isnull(TotalAmount, 0) + @ApportionedAmount,HaoCaiFei= isnull(HaoCaiFei,0)+@ApportionedAmount  where Id = @MainId "; break;
                case 6: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @Quantity, TotalAmount = isnull(TotalAmount, 0) + @ApportionedAmount,TuiYuanCaiLiaoFei= isnull(TuiYuanCaiLiaoFei,0)+@ApportionedAmount  where Id = @MainId "; break;
            }
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }

        public bool DeleteShippingBillEntry(ShippingBillEntryModel entryModel)
        {
            string sql = @" delete from SJShippingBillEntry where Id=@Id;";
            switch (entryModel.GoodsType)
            {
                case 2: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) - @Quantity, TotalAmount = isnull(TotalAmount, 0) - @ApportionedAmount,HaoCaiFei= isnull(HaoCaiFei,0)-@ApportionedAmount  where Id = @MainId "; break;
                case 3: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) - @Quantity, TotalAmount = isnull(TotalAmount, 0) - @ApportionedAmount,YangYouFei= isnull(YangYouFei,0)-@ApportionedAmount  where Id = @MainId "; break;
                case 4: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) - @Quantity, TotalAmount = isnull(TotalAmount, 0) - @ApportionedAmount,SheBeiFei= isnull(SheBeiFei,0)-@ApportionedAmount  where Id = @MainId "; break;
                case 5: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) - @Quantity, TotalAmount = isnull(TotalAmount, 0) - @ApportionedAmount,HaoCaiFei= isnull(HaoCaiFei,0)-@ApportionedAmount  where Id = @MainId "; break;
                case 6: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) - @Quantity, TotalAmount = isnull(TotalAmount, 0) - @ApportionedAmount,TuiYuanCaiLiaoFei= isnull(TuiYuanCaiLiaoFei,0)-@ApportionedAmount  where Id = @MainId "; break;
            }

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }

        // 系统单据只能改类型
        public bool UpdateShippingBillEntry2(ShippingBillEntryModel entryModel)
        {
            string sql = @" update  SJShippingBillEntry  set GoodsType=@GoodsType,Note=@Note where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }

        // 非系统单据可以修改数量金额
        public bool UpdateShippingBillEntry3(ShippingBillEntryModel entryModel,float qtyDiff,float amountDiff)
        {
            //string sql = @" update  SJShippingBillEntry  set EntryId=@EntryId,GoodsType=@GoodsType,Quantity=@Quantity where Id=@Id;
            //                update t set TotalQuantity=s,ApportionedAmount=Quantity/s * TotalAmount
            //                from ( select *, sum(Quantity)over() s from SJShippingBillEntry  where MainId=@MainId) as t; update SJShippingBill set TotalQuantity=TotalQuantity+diff where id=@MainId;";
            string sql = @" update  SJShippingBillEntry  set Quantity=@Quantity,ApportionedAmount=@ApportionedAmount,Note=@Note  where Id=@Id;
";

            switch (entryModel.GoodsType)
            {
                case 2: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,HaoCaiFei= isnull(HaoCaiFei,0) + @AmountDiff  where Id = @MainId "; break;
                case 3: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,YangYouFei= isnull(YangYouFei,0) + @AmountDiff  where Id = @MainId "; break;
                case 4: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,SheBeiFei= isnull(SheBeiFei,0) + @AmountDiff  where Id = @MainId "; break;
                case 5: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,HaoCaiFei= isnull(HaoCaiFei,0) + @AmountDiff  where Id = @MainId "; break;
                case 6: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,TuiYuanCaiLiaoFei= isnull(TuiYuanCaiLiaoFei,0) + @AmountDiff  where Id = @MainId "; break;
            }

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", entryModel.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("@MainId", entryModel.MainId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@GoodsType", entryModel.GoodsType, DbType.Int32, ParameterDirection.Input);
            dp.Add("@Quantity", entryModel.Quantity, DbType.Single, ParameterDirection.Input);
            dp.Add("@ApportionedAmount", entryModel.ApportionedAmount, DbType.Single, ParameterDirection.Input);
            dp.Add("@AmountDiff", amountDiff, DbType.Single, ParameterDirection.Input);
            dp.Add("@QtyDiff", qtyDiff, DbType.Single, ParameterDirection.Input);
            dp.Add("@Note", entryModel.Note, DbType.String, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, dp) > 0;
            }
        }


        public string DeleteShipingBill(int id)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ShippingId", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar("SJDeleteShippingBill", dp, null, null, CommandType.StoredProcedure));
            }
        }
    }
}









