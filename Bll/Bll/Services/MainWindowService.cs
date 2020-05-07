using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Services
{
    public class MainWindowService
    {
        private readonly MainWindowDAL dal = new MainWindowDAL();

        public IEnumerable<MainMenuModel> GetUserMenu(bool superAdmin ,int userId)
        {
            //// 如果角色是管理员，则获取所有数据
            //int roleId = Convert.ToInt32(dal.GetUserRole(userId));
            //if (roleId==1)
            //    return dal.GetAllMenu().ToList();
            //else
            //    return dal.GetUserMenu(userId).ToList();

            // 如果是超级管理员，就显示所有界面
            if(superAdmin)
                return dal.GetAllMenu().ToList();
            else
                return dal.GetUserMenu(userId).ToList();

        }
    }
}
