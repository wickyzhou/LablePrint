using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class QuerySchemaCurrentStatusModel: ViewModelBase11
	{

		public int Id { get; set; }

		public int UserId { get; set; }

		public DateTime ProductionDate { get; set; }

		public string PageSize { get; set; }

		private int pageSizeStatus;

		public int PageSizeStatus
		{
			get { return pageSizeStatus; }
			set
			{
				pageSizeStatus = value;
				this.RaisePropertyChanged(nameof(PageSizeStatus));
			}
		}

		public int SchemaId { get; set; }

		private int schemaStatus;

		public int SchemaStatus
		{
			get { return schemaStatus; }
			set
			{
				schemaStatus = value;
				this.RaisePropertyChanged(nameof(SchemaStatus));
			}
		}

	}
}
