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
				this.RaisePropertyChanged(nameof(StringTotalAmount));
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
				this.RaisePropertyChanged(nameof(StringTotalAmount));
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
				this.RaisePropertyChanged(nameof(StringTotalAmount));
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
				this.RaisePropertyChanged(nameof(StringTotalAmount));
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
				this.RaisePropertyChanged(nameof(StringTotalAmount));
			}
		}

		private float guanShuiFei;

		public float GuanShuiFei
		{
			get { return guanShuiFei; }
			set
			{
				guanShuiFei = value;
				this.RaisePropertyChanged(nameof(GuanShuiFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(StringTotalAmount));
			}
		}

		private float tiHuoFei;

		public float TiHuoFei
		{
			get { return tiHuoFei; }
			set
			{
				tiHuoFei = value;
				this.RaisePropertyChanged(nameof(TiHuoFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(StringTotalAmount));
			}
		}

		private float weiXianPinFei;

		public float WeiXianPinFei
		{
			get { return weiXianPinFei; }
			set
			{
				weiXianPinFei = value;
				this.RaisePropertyChanged(nameof(WeiXianPinFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(StringTotalAmount));
			}
		}

		private float qingGuanFei;

		public float QingGuanFei
		{
			get { return qingGuanFei; }
			set
			{
				qingGuanFei = value;
				this.RaisePropertyChanged(nameof(QingGuanFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(StringTotalAmount));
			}
		}

		private float baoXianFei;

		public float BaoXianFei
		{
			get { return baoXianFei; }
			set
			{
				baoXianFei = value;
				this.RaisePropertyChanged(nameof(BaoXianFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(StringTotalAmount));
			}
		}

		private float paiSongFei;

		public float PaiSongFei
		{
			get { return paiSongFei; }
			set
			{
				paiSongFei = value;
				this.RaisePropertyChanged(nameof(PaiSongFei));
				this.RaisePropertyChanged(nameof(TotalAmount));
				this.RaisePropertyChanged(nameof(StringTotalAmount));
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


		public float TotalAmount
		{
			get { return YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuanShuiFei + TiHuoFei + WeiXianPinFei + QingGuanFei + BaoXianFei + PaiSongFei + OtherCosts+ HaoCaiFei+ YangYouFei+ SheBeiFei+ ChengPinTuiHuoFei+ TuiYuanCaiLiaoFei; }
			private set { }
		}

		public float ApportionedAmount
		{
			get { return YunShuFei + YouFei + GuoLuFei + ChaiLvFei + WeiXiuFei + GuanShuiFei + TiHuoFei + WeiXianPinFei + QingGuanFei + BaoXianFei + PaiSongFei + OtherCosts; }
			private set { }
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



		private string stringTotalAmount;

		public string StringTotalAmount
		{
			get { return $"金额合计：{Convert.ToString(TotalAmount)} 元 "; }
			set
			{
				//stringTotalAmount = value;
				//this.RaisePropertyChanged(nameof(StringTotalAmount));
			}
		}

	


	}
}
