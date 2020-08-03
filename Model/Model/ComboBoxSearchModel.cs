using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ComboBoxSearchModel:NotificationObject
    {
		private int id;

		public int Id
		{
			get { return id; }
			set
			{
				id = value;
				this.RaisePropertyChanged(nameof(Id));
			}
		}

		private string name;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				this.RaisePropertyChanged(nameof(Name));
			}
		}

		private string searchText;

		public string SearchText
		{
			get { return searchText; }
			set
			{
				searchText = value;
				this.RaisePropertyChanged(nameof(SearchText));
			}
		}
    }
}
