using CRMApiModel;
using CRMApiModel.QueryXoqlModel;
using Dal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;

namespace Ui.ViewModel.IndexPage
{
    public class CRMViewModel: CRMApiBaseViewModel
	{


		public CRMViewModel()
        {
			InitData();
			InitCommand();
		}

		private void InitCommand()
		{
			CustCommand = new DelegateCommand((obj) =>
			{
				//var ss1 = CRMService.GetQueryXoqlData<OpportunityAccountQueryXoqlModel>(@"select  accountId.accountName AccountName,opportunityName OpportunityName from opportunity");
				//if (ss1.code == 200)
				//{
				//	OpportunityAccountQueryXoqlModel[] rr = ss1.data.records;
				//	SqlHelper.LoadArrayToDBModelTable(rr, "OpportunityAccountQueryXoqlTable");
				//}
				//else
				//{
				//	MessageBox.Show($"{ss1.msg}");
				//}
					

				var ss2 = CRMService.GetQueryXoqlData<SROpportunityQueryXoqlModel>(@" select customItem177__c  ItemCode,opportunityName  ItemName,ownerId XiangMuJingLi,dbcVarchar6 ShiChangZhiChi,customItem172__c YeWuZhiChi,
customItem175__c JiShuZhiChi,customItem182__c PinZhiZhiChi,customItem173__c ChanPinJingLi,customItem180__c JiFuZhiChi,customItem181__c SeCaiZhiChi  from opportunity ");
				if (ss2.code == 200)
				{
					SROpportunityQueryXoqlModel[] rr = ss2.data.records;
					SqlHelper.LoadArrayToDBModelTable(rr, "SROpportunityXoqlTable");
				}
				else
				{
					MessageBox.Show($"{ss2.code} \r\n{ss2.msg}");
				}

				//var ss = CRMService.GetQueryXoqlData(Token, "opportunity", " select dbcSelect1,opportunityName,customItem169__c,(select accountName from account where id=opportunity.customItem169__c) accountName from  opportunity ");
				// var xx = CRMService.GetEntityDescription();
				//Text1 = CRMService.GetEntityDescriptionString();
			});
		}

		private void InitData()
		{
			Task.Factory.StartNew(() =>
			{
				UIExecute.RunAsync(() =>
				{
			
				});
			});
		}

		public DelegateCommand CustCommand { get; set; }


		private string text1;

		public string Text1
		{
			get { return text1; }
			set
			{
				text1 = value;
				this.RaisePropertyChanged(nameof(Text1));
			}
		}

		private string text2;

		public string Text2
		{
			get { return text2; }
			set
			{
				text2 = value;
				this.RaisePropertyChanged(nameof(Text2));
			}
		}

		private string text3;

		public string Text3
		{
			get { return text3; }
			set
			{
				text3 = value;
				this.RaisePropertyChanged(nameof(Text3));
			}
		}

		private string text4;

		public string Text4
		{
			get { return text4; }
			set
			{
				text4 = value;
				this.RaisePropertyChanged(nameof(Text4));
			}
		}

	}
}
