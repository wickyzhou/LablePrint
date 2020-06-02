using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class HostConfigModel:NotificationObject
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public string TypeDesciption { get; set; }

        public string Host { get; set; }

		private string hostValue;

		public string HostValue
		{
			get { return hostValue; }
			set
			{
				hostValue = value ;
				this.RaisePropertyChanged(nameof(HostValue));
			}
		}

		public int UserId { get; set; }

	}
}
