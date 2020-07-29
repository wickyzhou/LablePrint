using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class ComboBoxSearchService
    {
        public IEnumerable<ComboBoxSearchModel> GetOrganizationLists()
        {
            string sql = @" select  * from SJOrganizationComboBoxSearchView ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ComboBoxSearchModel>(sql);
            }
        }

        public IEnumerable<ComboBoxSearchModel> GetCaseLists()
        {
            string sql = @" select  * from SJCaseComboBoxSearchView ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ComboBoxSearchModel>(sql);
            }
        }

        public IEnumerable<ComboBoxSearchModel> GetMaterialLists()
        {
            string sql = @" select  * from SJMaterialComboBoxSearchView ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ComboBoxSearchModel>(sql);
            }
        }

        //public IEnumerable<ComboBoxSearchModel> GetMaterialLists(string text)
        //{
        //    string sql = @" select top 10 * from SJMaterialComboBoxSearchView where SearchText like '%"+ text +@"%' ; ";
        //    using (var connection = SqlDb.UpdateConnection)
        //    {
        //        return connection.Query<ComboBoxSearchModel>(sql);
        //    }
        //}


        public IEnumerable<ComboBoxSearchModel> GetMaterialLists(string text)
        {
            string sql = @" select top 10 * from temp20200729 where SearchText like '%" + text + @"%' ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ComboBoxSearchModel>(sql);
            }
        }

    }
}
