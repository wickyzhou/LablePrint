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

        public int RecordLoginLog(int id)
        {
            return user.RecordLoginLog(id);
        }
        public int RecordLogoutLog(int id)
        {
            return user.RecordLogoutLog(id);
        }

        public int ModifyUserPassword( int userId,string ps)
        {
            return user.ModifyUserPassword(userId, ps);
        }
        
    }
}
