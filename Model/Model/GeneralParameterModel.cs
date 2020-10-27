using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class GeneralParameterModel:NotificationObject
    {
		private DateTime? paramBeginDate;

		public DateTime? ParamBeginDate
		{
			get { return paramBeginDate; }
			set
			{
				paramBeginDate = value;
				this.RaisePropertyChanged(nameof(ParamBeginDate));
			}
		}

		private DateTime? paramEndDate;

		public DateTime? ParamEndDate
		{
			get { return paramEndDate; }
			set
			{
				paramEndDate = value;
				this.RaisePropertyChanged(nameof(ParamEndDate));
			}
		}

		private int? paramInt1;

		public int? ParamInt1
		{
			get { return paramInt1; }
			set
			{
				paramInt1 = value;
				this.RaisePropertyChanged(nameof(ParamInt1));
			}
		}

		private int? paramInt2;

		public int? ParamInt2
		{
			get { return paramInt2; }
			set
			{
				paramInt2 = value;
				this.RaisePropertyChanged(nameof(ParamInt2));
			}
		}

		private int? paramInt3;

		public int? ParamInt3
		{
			get { return paramInt3; }
			set
			{
				paramInt3 = value;
				this.RaisePropertyChanged(nameof(ParamInt3));
			}
		}


		private double? paramDouble1;

		public double? ParamDouble1
		{
			get { return paramDouble1; }
			set
			{
				paramDouble1 = value;
				this.RaisePropertyChanged(nameof(ParamDouble1));
			}
		}

		private double? paramDouble2;

		public double? ParamDouble2
		{
			get { return paramDouble2; }
			set
			{
				paramDouble2 = value;
				this.RaisePropertyChanged(nameof(ParamDouble2));
			}
		}

		private double? paramDouble3;

		public double? ParamDouble3
		{
			get { return paramDouble3; }
			set
			{
				paramDouble3 = value;
				this.RaisePropertyChanged(nameof(ParamDouble3));
			}
		}



		private DateTime? paramDateTime1;	

		public DateTime? ParamDateTime1	
		{
			get { return paramDateTime1; }
			set
			{
				paramDateTime1 = value;
				this.RaisePropertyChanged(nameof(ParamDateTime1));
			}
		}

		private DateTime? paramDateTime2;

		public DateTime? ParamDateTime2
		{
			get { return paramDateTime2; }
			set
			{
				paramDateTime2 = value;
				this.RaisePropertyChanged(nameof(ParamDateTime2));
			}
		}

		private DateTime? paramDateTime3;

		public DateTime? ParamDateTime3
		{
			get { return paramDateTime3; }
			set
			{
				paramDateTime3 = value;
				this.RaisePropertyChanged(nameof(ParamDateTime3));
			}
		}

		private string paramString1;

		public string ParamString1
		{
			get { return paramString1; }
			set
			{
				paramString1 = value;
				this.RaisePropertyChanged(nameof(ParamString1));
			}
		}

		private string paramString2;

		public string ParamString2
		{
			get { return paramString2; }
			set
			{
				paramString2 = value;
				this.RaisePropertyChanged(nameof(ParamString2));
			}
		}

		private string paramString3;

		public string ParamString3
		{
			get { return paramString3; }
			set
			{
				paramString3 = value;
				this.RaisePropertyChanged(nameof(ParamString3));
			}
		}

		private string paramString4;

		public string ParamString4
		{
			get { return paramString4; }
			set
			{
				paramString4 = value;
				this.RaisePropertyChanged(nameof(ParamString4));
			}
		}

		private string paramString5;

		public string ParamString5
		{
			get { return paramString5; }
			set
			{
				paramString5 = value;
				this.RaisePropertyChanged(nameof(ParamString5));
			}
		}

		private string paramString6;

		public string ParamString6
		{
			get { return paramString6; }
			set
			{
				paramString6 = value;
				this.RaisePropertyChanged(nameof(ParamString6));
			}
		}

	}
}
