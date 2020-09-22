using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui.Command;

namespace Ui.ViewModel.IndexPage
{
    public class ProductionDeptLabelPrintA4ViewModel:BaseViewModel
    {
		public ProductionDeptLabelPrintA4ViewModel()
		{
			InitCommand();
			InitData();
		}

		public DelegateCommand PrintCommand { get; set; }
		public DelegateCommand QueryCommand { get; set; }
		public DelegateCommand DataGridManageCommand { get; set; }


		private void InitCommand()
		{
			PrintCommand = new DelegateCommand((obj) =>
			{

			});

			QueryCommand = new DelegateCommand((obj) =>
			{

			});

			DataGridManageCommand = new DelegateCommand((obj) =>
			{

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
	}



}
