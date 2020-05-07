using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class PageOwnerModel
    {
        public int Id { get; set; }

        public int MainMenuId { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
