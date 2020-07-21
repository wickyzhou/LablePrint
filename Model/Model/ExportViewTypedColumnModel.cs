using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ExportViewTypedColumnModel:NotificationObject
    {
		private int viewGroupId;

		public int ViewGroupId
		{
			get { return viewGroupId; }
			set
			{
				viewGroupId = value;
				this.RaisePropertyChanged(nameof(ViewGroupId));
			}
		}

		private string viewName;

		public string ViewName
		{
			get { return viewName; }
			set
			{
				viewName = value;
				this.RaisePropertyChanged(nameof(ViewName));
			}
		}

		private int typedColumnId;

		public int TypedColumnId
		{
			get { return typedColumnId; }
			set
			{
				typedColumnId = value;
				this.RaisePropertyChanged(nameof(TypedColumnId));
			}
		}


		private string typedModelPropertyName;

		public string TypedModelPropertyName
		{
			get { return typedModelPropertyName; }
			set
			{
				typedModelPropertyName = value;
				this.RaisePropertyChanged(nameof(TypedModelPropertyName));
			}
		}

		private string typedColumnName;

		public string TypedColumnName
		{
			get { return typedColumnName; }
			set
			{
				typedColumnName = value;
				this.RaisePropertyChanged(nameof(TypedColumnName));
			}
		}


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

    }
}
