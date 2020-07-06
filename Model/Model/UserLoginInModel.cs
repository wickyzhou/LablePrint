using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserLoginLogModel
    {

        public int ID { get; set; }

        public int UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public string LoginHostName { get; set; }

        public DateTime LogoutTime { get; set; }

        public int LoginId { get; set; }

        public bool IsSystemLogout { get; set; }
        
    }
}
