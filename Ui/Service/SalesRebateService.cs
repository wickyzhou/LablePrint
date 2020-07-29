using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class SalesRebateService
    {

        public bool Insert(SalesRebateModel salesRebateModel)
        {
            string sql = @" insert into SJSalesRebate(ProductionModelId,CaseId,CustId,RebateClass,TaxAmountType,RebatePctType,RebatePct,RebateAmout,AmountRangeNote)
                            values(@ProductionModelId,@CaseId,@CustId,@RebateClass,@TaxAmountType,@RebatePctType,@RebatePct,@RebateAmout,@AmountRangeNote) ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
            }
        }
    }
}
