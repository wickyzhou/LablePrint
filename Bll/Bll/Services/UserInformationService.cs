using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Services
{
   public class UserInformationService
    {
        private readonly UserInformationDAL dal = new UserInformationDAL();

        public string GetDefaultPage(int userId)
        {
            return Convert.ToString(dal.GetDefaultPage(userId));
        }
    }
}
