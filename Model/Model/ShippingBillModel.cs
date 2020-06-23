using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Model
{
	public class ShippingBillModel : NotificationObject
	{
		public int Id { get; set; }

		private string billNo
;
		public string BillNo

		{
			get { return billNo; }
			set
			{
				billNo = value;
				this.RaisePropertyChanged(nameof(BillNo));
			}
		}


		public DateTime CreateTime { get; set; }

		private DateTime billDate;

		public DateTime BillDate
		{
			get { return billDate; }
			set
			{
				billDate = value;
				this.RaisePropertyChanged(nameof(BillDate));
			}
		}

		private int logisticsType;

		public int LogisticsType
		{
			get { return logisticsType; }
			set
			{
				logisticsType = value;
				this.RaisePropertyChanged(nameof(LogisticsType));
			}
		}

		private string logisticsCompanyName;

		public string LogisticsCompanyName
		{
			get { return logisticsCompanyName; }
			set
			{
				logisticsCompanyName = value;
				this.RaisePropertyChanged(nameof(LogisticsCompanyName));
			}
		}

		private float yunShuFei;

		public float YunShuFei
		{
			get { return yunShuFei; }
			set
			{
				yunShuFei = value;
				this.RaisePropertyChanged(nameof(YunShuFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
			}
		}

		private float youFei;

		public float YouFei
		{
			get { return youFei; }
			set
			{
				youFei = value;
				this.RaisePropertyChanged(nameof(YouFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
			}
		}

		private float guoLuFei;

		public float GuoLuFei
		{
			get { return guoLuFei; }
			set
			{
				guoLuFei = value;
				this.RaisePropertyChanged(nameof(GuoLuFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
			}
		}

		private float chaiLvFei;

		public float ChaiLvFei
		{
			get { return chaiLvFei; }
			set
			{
				chaiLvFei = value;
				this.RaisePropertyChanged(nameof(ChaiLvFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
			}
		}

		private float weiXiuFei;

		public float WeiXiuFei
		{
			get { return weiXiuFei; }
			set
			{
				weiXiuFei = value;
				this.RaisePropertyChanged(nameof(WeiXiuFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
			}
		}







		private float otherCosts;
		public float OtherCosts
		{
			get { return otherCosts; }
			set
			{
				otherCosts = value;
				this.RaisePropertyChanged(nameof(OtherCosts));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
			}
		}

		private float haoCaiFei;

		public float HaoCaiFei
		{
			get { return haoCaiFei; }
			set
			{
				haoCaiFei = value;
				this.RaisePropertyChanged(nameof(HaoCaiFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}

		private float yangYouFei;

		public float YangYouFei
		{
			get { return yangYouFei; }
			set
			{
				yangYouFei = value;
				this.RaisePropertyChanged(nameof(YangYouFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}

		private float sheBeiFei;

		public float SheBeiFei
		{
			get { return sheBeiFei; }
			set
			{
				sheBeiFei = value;
				this.RaisePropertyChanged(nameof(SheBeiFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}

		private float chengPinTuiHuoFei;

		public float ChengPinTuiHuoFei
		{
			get { return chengPinTuiHuoFei; }
			set
			{
				chengPinTuiHuoFei = value;
				this.RaisePropertyChanged(nameof(ChengPinTuiHuoFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}


		private float tuiYuanCaiLiaoFei;

		public float TuiYuanCaiLiaoFei 
		{
			get { return tuiYuanCaiLiaoFei; }
			set
			{
				tuiYuanCaiLiaoFei = value;
				this.RaisePropertyChanged(nameof(TuiYuanCaiLiaoFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}


		private string demander;

		public string Demander
		{
			get { return demander; }
			set
			{
				demander = value;
				this.RaisePropertyChanged(nameof(Demander));
			}
		}


		private string note;

		public string Note
		{
			get { return note; }
			set
			{
				note = value;
				this.RaisePropertyChanged(nameof(Note));
			}
		}

		//

		private float totalAmount;

		public float TotalAmount
		{
			get { return SystemApportionedAmount + HaoCaiFei + YangYouFei + SheBeiFei + ChengPinTuiHuoFei + TuiYuanCaiLiaoFei;  }
			set
			{
				totalAmount = value;
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}

		//
		private float systemApportionedAmount;

		public float SystemApportionedAmount
		{
			get { return YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong  + OtherCosts; }
			set
			{
				systemApportionedAmount = value;
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}


		private string logisticsBillNo;

		public string LogisticsBillNo
		{
			get { return logisticsBillNo; }
			set
			{
				logisticsBillNo = value;
				this.RaisePropertyChanged(nameof(LogisticsBillNo));
			}
		}

		private float totalQuantity;

		public float TotalQuantity
		{
			get { return totalQuantity; }
			set
			{
				totalQuantity = value;
				this.RaisePropertyChanged(nameof(TotalQuantity));
			}
		}

		private float systemQuantity;

		public float SystemQuantity
		{
			get { return systemQuantity; }
			set
			{
				systemQuantity = value;
				this.RaisePropertyChanged(nameof(SystemQuantity));
			}
		}

		private float guoNeiDuanFeiYong;

		public float GuoNeiDuanFeiYong
		{
			get { return guoNeiDuanFeiYong; }
			set
			{
				guoNeiDuanFeiYong = value;
				this.RaisePropertyChanged(nameof(GuoNeiDuanFeiYong));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}


		private float guoJiDuanFeiYong;

		public float GuoJiDuanFeiYong
		{
			get { return guoJiDuanFeiYong; }
			set
			{
				guoJiDuanFeiYong = value;
				this.RaisePropertyChanged(nameof(GuoJiDuanFeiYong));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}

		private int yunShuDuanFeiYong;

		public int YunShuDuanFeiYong
		{
			get { return yunShuDuanFeiYong; }
			set
			{
				yunShuDuanFeiYong = value;
				this.RaisePropertyChanged(nameof(YunShuDuanFeiYong));
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
				this.RaisePropertyChanged(nameof(TotalAmount));
			}
		}

	}
}
