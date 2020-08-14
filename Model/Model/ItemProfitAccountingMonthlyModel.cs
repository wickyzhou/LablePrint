using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ItemProfitAccountingMonthlyModel : NotificationObject
    {
		private string settleMonth;

		public string SettleMonth
		{
			get { return settleMonth; }
			set
			{
				settleMonth = value;
				this.RaisePropertyChanged(nameof(SettleMonth));
			}
		}

		private string itemCode;

		public string ItemCode
		{
			get { return itemCode; }
			set
			{
				itemCode = value;
				this.RaisePropertyChanged(nameof(ItemCode));
			}
		}

		private string itemName;

		public string ItemName
		{
			get { return itemName; }
			set
			{
				itemName = value;
				this.RaisePropertyChanged(nameof(itemName));
			}
		}

		private string xiangMuJingLi;

		public string XiangMuJingLi
		{
			get { return xiangMuJingLi; }
			set
			{
				xiangMuJingLi = value;
				this.RaisePropertyChanged(nameof(XiangMuJingLi));
			}
		}

		private string shiChangZhiChi;

		public string ShiChangZhiChi
		{
			get { return shiChangZhiChi; }
			set
			{
				shiChangZhiChi = value;
				this.RaisePropertyChanged(nameof(ShiChangZhiChi));
			}
		}

		private string seCaiZhiChi;

		public string SeCaiZhiChi
		{
			get { return seCaiZhiChi; }
			set
			{
				seCaiZhiChi = value;
				this.RaisePropertyChanged(nameof(SeCaiZhiChi));
			}
		}

		private string yeWuZhiChi;

		public string YeWuZhiChi
		{
			get { return yeWuZhiChi; }
			set
			{
				yeWuZhiChi = value;
				this.RaisePropertyChanged(nameof(YeWuZhiChi));
			}
		}

		private string jiFuZhiChi;

		public string JiFuZhiChi
		{
			get { return jiFuZhiChi; }
			set
			{
				jiFuZhiChi = value;
				this.RaisePropertyChanged(nameof(JiFuZhiChi));
			}
		}

		private string chanPinJingLi;

		public string ChanPinJingLi
		{
			get { return chanPinJingLi; }
			set
			{
				chanPinJingLi = value;
				this.RaisePropertyChanged(nameof(ChanPinJingLi));
			}
		}

		private string jiShuZhiChi;

		public string JiShuZhiChi
		{
			get { return jiShuZhiChi; }
			set
			{
				jiShuZhiChi = value;
				this.RaisePropertyChanged(nameof(JiShuZhiChi));
			}
		}

		private string pinZhiZhiChi;

		public string PinZhiZhiChi
		{
			get { return pinZhiZhiChi; }
			set
			{
				pinZhiZhiChi = value;
				this.RaisePropertyChanged(nameof(PinZhiZhiChi));
			}
		}

		private string houDuanZhiChi;

		public string HouDuanZhiChi
		{
			get { return houDuanZhiChi; }
			set
			{
				houDuanZhiChi = value;
				this.RaisePropertyChanged(nameof(HouDuanZhiChi));
			}
		}

		public double Income { get; set; }
		public double ProductionCost { get; set; }
		public double TravelCost { get; set; }
		public double EntertainmentCost { get; set; }
		public double LogisticCost { get; set; }
		public double TestCost { get; set; }
		public double MaterialCost { get; set; }
		public double EmployeeCost { get; set; }
		public double Profit { get; set; }
	}
}
