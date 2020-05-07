using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class EnumModel :NotificationObject
    {
        private int id;

        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged(nameof(ID));
            }
        }


        private int groupSeq;

        public int GroupSeq
        {
            get { return groupSeq; }
            set
            {
                groupSeq = value;
                this.RaisePropertyChanged(nameof(GroupSeq));
            }
        }

        private string groupName;

        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                this.RaisePropertyChanged(nameof(GroupName));
            }
        }

        private int itemSeq;

        public int ItemSeq
        {
            get { return itemSeq; }
            set
            {
                itemSeq = value;
                this.RaisePropertyChanged(nameof(ItemSeq));
            }
        }

        private string itemValue;

        public string ItemValue
        {
            get { return itemValue; }
            set
            {
                itemValue = value;
                this.RaisePropertyChanged(nameof(ItemValue));
            }
        }

        private int parentGroupSeq;

        public int ParentGroupSeq
        {
            get { return parentGroupSeq; }
            set
            {
                parentGroupSeq = value;
                this.RaisePropertyChanged(nameof(ParentGroupSeq));
            }
        }


    }
}

