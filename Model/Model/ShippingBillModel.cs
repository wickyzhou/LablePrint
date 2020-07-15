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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
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

				SystemApportionedAmount=YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
				//this.RaisePropertyChanged(nameof(TotalAmount));
				//this.RaisePropertyChanged(nameof(SystemApportionedAmount));
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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
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
				SystemApportionedAmount = YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong + OtherCosts;
				TotalAmount = UnApportionedAmount + SystemApportionedAmount;
			}
		}


		private float totalAmount;

		public float TotalAmount
		{
			get { return totalAmount; } //return SystemApportionedAmount + HaoCaiFei + YangYouFei + SheBeiFei + ChengPinTuiHuoFei + TuiYuanCaiLiaoFei;  
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
			get { return systemApportionedAmount; } //return YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuoNeiDuanFeiYong + GuoJiDuanFeiYong + YunShuDuanFeiYong  + OtherCosts; 
			set
			{
				systemApportionedAmount = value;
				this.RaisePropertyChanged(nameof(SystemApportionedAmount));
				
				//this.RaisePropertyChanged(nameof(TotalAmount));
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

	
		private int isSystem;

		public int IsSystem
		{
			get { return isSystem; }
			set
			{
				isSystem = value;
				this.RaisePropertyChanged(nameof(IsSystem));
			}
		}

		private float systemApportionedQuantity;

		public float SystemApportionedQuantity
		{
			get { return systemApportionedQuantity; }
			set
			{
				systemApportionedQuantity = value;
				this.RaisePropertyChanged(nameof(SystemApportionedQuantity));
			}
		}

		private float unApportionedAmount;

		public float UnApportionedAmount
		{
			get { return unApportionedAmount; }
			set
			{
				unApportionedAmount = value;
				this.RaisePropertyChanged(nameof(UnApportionedAmount));
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


		private string supercargo;

		public string Supercargo
		{
			get { return supercargo; }
			set
			{
				supercargo = value;
				this.RaisePropertyChanged(nameof(Supercargo));
			}
		}

		private string driver;

		public string Driver
		{
			get { return driver; }
			set
			{
				driver = value;
				this.RaisePropertyChanged(nameof(Driver));
			}
		}

		public int UserId { get; set; }

		public string UserName { get; set; }

	}
}
