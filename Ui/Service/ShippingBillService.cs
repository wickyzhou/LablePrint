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
        public IList<ShippingBillModel> GetAllShippingBills(int userDataId)
        {
            string sql ;
            if (userDataId==-1)
                sql = @"select * from SJShippingBill order by Id desc ";
            else
                sql = @"select * from SJShippingBill where UserId=@UserDataId order by Id desc ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillModel>(sql,new { UserDataId=userDataId }).ToList();
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

        public bool UpdateShipingBill2(ShippingBillModel billModel)
        {
            string sql = @" update SJShippingBill 
                             set TotalAmount=@TotalAmount,TotalQuantity=@TotalQuantity,SystemApportionedQuantity=@SystemApportionedQuantity,SystemApportionedAmount=@SystemApportionedAmount,UnApportionedAmount=@UnApportionedAmount
                             where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, billModel) > 0;
            }
        }


        public bool UpdateShipingBillEntry(ShippingBillModel billModel)
        {
            string sql = @" update SJShippingBillEntry set Amount= Quantity/@SystemApportionedQuantity*@SystemApportionedAmount where MainId=@Id and GoodsType in(1,3)  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, billModel) > 0;
            }
        }

        public bool UpdateShipingBillEntry(ShippingBillEntryModel model)
        {
            string sql = @" update  SJShippingBillEntry  set CaseName=@CaseName,BrandName=@BrandName,DeptName=@DeptName,CustName=@CustName,Quantity=@Quantity,Amount=@Amount,GoodsType=@GoodsType,Note=@Note where Id=@Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                //DynamicParameters dp = new DynamicParameters();
                //dp.Add("@Id", model.Id, DbType.Int32, ParameterDirection.Input);
                //dp.Add("@CaseName", model.CaseName, DbType.String, ParameterDirection.Input);
                //dp.Add("@BrandName", model.BrandName, DbType.String, ParameterDirection.Input);
                //dp.Add("@DeptName", model.DeptName, DbType.String, ParameterDirection.Input);
                //dp.Add("@CustName", model.CustName, DbType.String, ParameterDirection.Input);
                //dp.Add("@Quantity", model.Quantity, DbType.Single, ParameterDirection.Input);
                //dp.Add("@Amount", model.Amount, DbType.Single, ParameterDirection.Input);
                //dp.Add("@GoodsType", model.GoodsType, DbType.Int32, ParameterDirection.Input);
                //dp.Add("@Note", model.Note, DbType.String, ParameterDirection.Input);
                //dp.Add("@SystemApportionedQuantity", systemApportionedQuantity, DbType.Single, ParameterDirection.Input);
                //dp.Add("@SystemApportionedAmount", systemApportionedAmount, DbType.Single, ParameterDirection.Input);
                return connection.Execute(sql, model) > 0;
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

        public IList<ShippingBillExportModel> GetExprotShippingBill(int userDataId)
        {
            string sql;
            if(userDataId==-1)
            sql = @"select TotalQuantity,TotalAmount, BillNo,BillDate,
                            (select ItemValue from  SJEnumTable where GroupSeq=3 and ItemSeq = a.LogisticsType) LogisticsTypeName,
                            LogisticsCompanyName,LogisticsBillNo,YunShuFei,YouFei,GuoLuFei,ChaiLvFei,WeiXiuFei,Demander,OtherCosts,a.Note NoteA	,
                             (select ItemValue from  SJEnumTable where GroupSeq=4 and ItemSeq = b.GoodsType) GoodsTypeName ,
							 GuoNeiDuanFeiYong,GuoJiDuanFeiYong,YunShuDuanFeiYong,
                            EntryId,CaseName,BrandName,DeptName,CustName,Quantity,Amount,b.Note NoteB from  SJShippingBill a left join SJShippingBillEntry b on a.Id=b.MainId  
                        order by BillNo,EntryId ";
            else
                sql = @"select TotalQuantity,TotalAmount, BillNo,BillDate,
                            (select ItemValue from  SJEnumTable where GroupSeq=3 and ItemSeq = a.LogisticsType) LogisticsTypeName,
                            LogisticsCompanyName,LogisticsBillNo,YunShuFei,YouFei,GuoLuFei,ChaiLvFei,WeiXiuFei,Demander,OtherCosts,a.Note NoteA	,
                             (select ItemValue from  SJEnumTable where GroupSeq=4 and ItemSeq = b.GoodsType) GoodsTypeName ,
							 GuoNeiDuanFeiYong,GuoJiDuanFeiYong,YunShuDuanFeiYong,
                            EntryId,CaseName,BrandName,DeptName,CustName,Quantity,Amount,b.Note NoteB from  SJShippingBill a left join SJShippingBillEntry b on a.Id=b.MainId
                            where a.UserId=@UserId
                        order by BillNo,EntryId ";

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillExportModel>(sql,new { UserId=userDataId}).ToList();
            }
        }

        public bool AddShipingBillEntry(ShippingBillEntryModel entryModel)
        {
            string sql = @" insert into SJShippingBillEntry(MainId,CaseName,Quantity,BrandName,DeptName,CustName,DeptId,CustId,BrandId,CaseId,EntryId,Amount,GoodsType,Note,IsSystem,SystemQuantity)
                            select @MainId,@CaseName,@Quantity,@BrandName,@DeptName,@CustName,@DeptId,@CustId,@BrandId,@CaseId,@EntryId,@Amount,@GoodsType,@Note,@IsSystem,0;
            update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @Quantity,TotalAmount = isnull(TotalAmount, 0) + @Amount, SystemApportionedQuantity = isnull(SystemApportionedQuantity, 0) + @Quantity  where Id = @MainId;";

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }

        public bool AddShipingBillEntry2(ShippingBillEntryModel entryModel)
        {
            string sql = @" insert into SJShippingBillEntry(MainId,CaseName,Quantity,BrandName,DeptName,CustName,DeptId,CustId,BrandId,CaseId,EntryId,Amount,GoodsType,Note,IsSystem,SystemQuantity)
                            select @MainId,@CaseName,@Quantity,@BrandName,@DeptName,@CustName,@DeptId,@CustId,@BrandId,@CaseId,@EntryId,@Amount,@GoodsType,@Note,@IsSystem,0;
            update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @Quantity,TotalAmount = isnull(TotalAmount, 0) + @Amount  where Id = @MainId;";

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entryModel) > 0;
            }
        }



        public bool DeleteShippingBillEntry(ShippingBillEntryModel entryModel)
        {
            string sql = @" delete from SJShippingBillEntry where Id=@Id;  
                    update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) - @Quantity
                                                , TotalAmount = isnull(TotalAmount, 0) - @Amount 
                                                , SystemApportionedQuantity = isnull(SystemApportionedQuantity, 0) - @Quantity
                    where Id = @MainId ;";
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
            string sql = @" update  SJShippingBillEntry  set Quantity=@Quantity,Amount=@Amount,Note=@Note  where Id=@Id;
";

            switch (entryModel.GoodsType)
            {
                case 2: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,HaoCaiFei= isnull(HaoCaiFei,0) + @AmountDiff  where Id = @MainId "; break;
                //case 3: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,YangYouFei= isnull(YangYouFei,0) + @AmountDiff  where Id = @MainId "; break;
                case 4: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,SheBeiFei= isnull(SheBeiFei,0) + @AmountDiff  where Id = @MainId "; break;
                case 5: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,HaoCaiFei= isnull(HaoCaiFei,0) + @AmountDiff  where Id = @MainId "; break;
                case 6: sql += " update SJShippingBill set TotalQuantity = isnull(TotalQuantity, 0) + @QtyDiff, TotalAmount = isnull(TotalAmount, 0) + @AmountDiff,TuiYuanCaiLiaoFei= isnull(TuiYuanCaiLiaoFei,0) + @AmountDiff  where Id = @MainId "; break;
                default: break;
            }

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", entryModel.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("@MainId", entryModel.MainId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@GoodsType", entryModel.GoodsType, DbType.Int32, ParameterDirection.Input);
            dp.Add("@Quantity", entryModel.Quantity, DbType.Single, ParameterDirection.Input);
            dp.Add("@Amount", entryModel.Amount, DbType.Single, ParameterDirection.Input);
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

        public bool DeleteNonSystemShipingBill(int id)
        {
            string sql = @" delete SJShippingBillEntry where MainId=@Id; delete from SJShippingBill where Id=@id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id=id}) > 0;
            }
        }



        public int AddShippingBill(int userId)
        {

            using (var connection = SqlDb.UpdateConnection)
            {
                var dp = new DynamicParameters();
                dp.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                dp.Add("@UserId",userId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                connection.Execute("SJCreateShippingBillProc", dp, null, null, CommandType.StoredProcedure);
                return dp.Get<int>("@Id");
            }
        }

        public ShippingBillModel GetShippingBillById(int id)
        {
            string sql = @" select * from SJShippingBill where Id=@Id ;";
            using (var connection = SqlDb.UpdateConnection)
            {
              return  connection.Query<ShippingBillModel>(sql,new { Id=id}).FirstOrDefault();
            }
        }


        public string GetCaseNameByGoodsType(int goodstype)
        {
            string sql = @"  select ItemValue from SJEnumTable where GroupSeq=4 and ItemSeq = @GoodsType ;";

            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql,new { GoodsType=goodstype }));
            }
        }

     
    }
}









