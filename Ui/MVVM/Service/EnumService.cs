using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;

namespace Ui.MVVM.Service
{
    public class EnumService
    {
        public IList<EnumEntity> GetEnumByGroupSeq(int groupSeq)
        {
            string sql = @"select * from SJEnumTable where GroupSeq=@GroupSeq";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<EnumEntity>(sql,new { GroupSeq = groupSeq }).ToList();
            }
        }

        public IList<EnumEntity> GetAllEnums()
        {
            string sql = @"select * from SJEnumTable ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<EnumEntity>(sql).ToList();
            }
        }

    }
}
