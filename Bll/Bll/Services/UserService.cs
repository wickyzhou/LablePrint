using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Services
{
   public class UserService
    {
        UserDAL user = new UserDAL();

        public IEnumerable<UserModel> GetAllUsers()
        {
            return user.GetAllUsers();
        }

        public int RecordLoginLog(int id,string hostName)
        {
            return user.RecordLoginLog(id,hostName);
        }

        public void RecordLogoutLog(int id,bool isSystemLogout)
        {
             user.RecordLogoutLog(id, isSystemLogout);
        }

        public int ModifyUserPassword( int userId,string ps)
        {
            return user.ModifyUserPassword(userId, ps);
        }
        
    }
}
