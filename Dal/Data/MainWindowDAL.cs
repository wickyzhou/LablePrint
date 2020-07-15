using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data
{
    public class MainWindowDAL
    {
        public IEnumerable<MainMenuModel> GetAllMenu()
        {
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT * FROM SJMainMenu order by ShowOrder ;", null);
            return SqlHelper.DataTableToModelList<MainMenuModel>(data);
        }

        public IEnumerable<MainMenuModel> GetUserMenu(int userId)
        {
            DataTable data = SqlHelper.ExecuteDataTable(
                @" SELECT a.* FROM SJMainMenu a join SJPageOwner b on a.ID=b.MainMenuId where UserId=@UserId
                    union
                   SELECT a.* FROM SJMainMenu a join  SJPageOwner b on a.ID = b.MainMenuId join  SJUserRole c on b.RoleId = c.RoleId where c.UserId = @UserId
                     order by ShowOrder ;", 
                new SqlParameter[] { new SqlParameter("@UserId", userId) });
            return SqlHelper.DataTableToModelList<MainMenuModel>(data);
        }


        public object GetUserRole(int userId)
        {
            return SqlHelper.ExecuteScalar("select RoleId from SJUserRole where UserId=@UserId order by RoleId", new SqlParameter[] { new SqlParameter("@UserId", userId) });
        }
    }
}
