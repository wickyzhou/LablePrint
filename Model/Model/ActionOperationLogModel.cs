using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ActionOperationLogModel
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public int MainMenuId { get; set; }

        public int UserId { get; set; }

        public string ActionName { get; set; }

        public string ActionDesc { get; set; }

        public int PKId { get; set; }

    }
}
