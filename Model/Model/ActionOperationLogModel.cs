using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ActionOperationLogModel:NotificationObject
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                this.RaisePropertyChanged(nameof(IsChecked));
            }
        }

        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public int MainMenuId { get; set; }

        public int UserId { get; set; }

        public string ActionName { get; set; }

        public string ActionDesc { get; set; }

        public int PKId { get; set; }

        public string HostName { get; set; }

    }
}
