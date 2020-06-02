using Dapper;
using Model;
using System;
using System.Collections.Generic;
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
                return  connection.Query<ShippingBillModel>(sql).ToList();
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
                            YunShuFei=@YunShuFei  ,YouFei=@YouFei  ,GuoLuFei=@GuoLuFei  ,ChaiLvFei=@ChaiLvFei  ,WeiXiuFei=@WeiXiuFei  ,GuanShuiFei=@GuanShuiFei,
                            TiHuoFei=@TiHuoFei, WeiXianPinFei=@WeiXianPinFei, QingGuanFei=@QingGuanFei, BaoXianFei=@BaoXianFei, PaiSongFei=@PaiSongFei, Demander=@Demander, OtherCosts=@OtherCosts,TotalAmount=@TotalAmount,
                            Note=@Note,LogisticsBillNo=@LogisticsBillNo,GoodsType=@GoodsType
                        where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, billModel) > 0;
            }
        }

        public bool UpdateShipingBillEntry(ShippingBillModel billModel)
        {
            string sql = @" update SJShippingBillEntry set ApportionedAmount= Quantity/TotalQuantity*@TotalAmount,TotalAmount=@TotalAmount where MainId=@Id ";
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
                            LogisticsCompanyName,LogisticsBillNo,YunShuFei,YouFei,GuoLuFei,ChaiLvFei,WeiXiuFei,GuanShuiFei,TiHuoFei,WeiXianPinFei,QingGuanFei,BaoXianFei,PaiSongFei,Demander,OtherCosts,Note	,
                             (select ItemValue from  SJEnumTable where GroupSeq=4 and ItemSeq = a.GoodsType) GoodsTypeName ,
                            EntryId,CaseName,BrandName,DeptName,CustName,Quantity,ApportionedAmount from  SJShippingBill a join SJShippingBillEntry b on a.Id=b.MainId  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillExportModel>(sql).ToList();
            }
        }

        public bool AddShipingBillEntry(ShippingBillEntryModel entryModel)
        {
            string sql = @" insert into SJShippingBillEntry(MainId,CaseName,Quantity,BrandName,DeptName,CustName,DeptId,CustId,BrandId,CaseId,TotalQuantity,EntryId,ApportionedAmount,TotalAmount,GoodsType)
values(@MainId,@CaseName,@Quantity,@BrandName,@DeptName,@CustName,@DeptId,@CustId,@BrandId,@CaseId,@TotalQuantity,@EntryId,@ApportionedAmount,@TotalAmount,@GoodsType); update SJShippingBillEntry set TotalQuantity=@TotalQuantity,ApportionedAmount=Quantity/@TotalQuantity*TotalAmount where MainId=@MainId ; update SJShippingBill set TotalQuantity=TotalQuantity+@Quantity  where Id=@MainId ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }

        public bool DeleteShippingBillEntry(ShippingBillEntryModel entryModel)
        {
            string sql = @" delete from SJShippingBillEntry where Id=@Id;
                            update t set TotalQuantity=s,ApportionedAmount=Quantity/s * TotalAmount
                            from ( select *, sum(Quantity)over() s from SJShippingBillEntry  where MainId=@MainId) as t; 
                        update SJShippingBill set TotalQuantity=TotalQuantity-@Quantity  where Id=@MainId;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }


        public bool UpdateShippingBillEntry2(ShippingBillEntryModel entryModel)
        {
            string sql = @" update  SJShippingBillEntry  set EntryId=@EntryId,GoodsType=@GoodsType where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }

        public bool UpdateShippingBillEntry3(ShippingBillEntryModel entryModel,float diff)
        {
            string sql = @" update  SJShippingBillEntry  set EntryId=@EntryId,GoodsType=@GoodsType,Quantity=@Quantity where Id=@Id;
                            update t set TotalQuantity=s,ApportionedAmount=Quantity/s * TotalAmount
                            from ( select *, sum(Quantity)over() s from SJShippingBillEntry  where MainId=@MainId) as t; update SJShippingBill set TotalQuantity=TotalQuantity+diff where id=@MainId;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }

    }
}









