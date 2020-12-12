using Common;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Ui.Command;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class ProductionDeptIndexPageViewModel : BaseViewModel
    {

        private readonly ProductionDeptIndexPageService _service;

        public ProductionDeptIndexPageViewModel()
        {
            _service = new ProductionDeptIndexPageService();
            HostConfig = CommonService.GetHostConfig(5, HostName, User.ID) ?? new HostConfigModel() { TypeId=5,Host=HostName,UserId=User.ID,TypeDesciption= "投入产出率" };
            DelegateCommandInit();
            GeneralParameter = new GeneralParameterModel() 
            {
                ParamBeginDate = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).Date,
                ParamEndDate = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1)
            };
        }

        private void DelegateCommandInit()
        {
            DirectorySelectCommand = new DelegateCommand((obj) =>
            {
                // 导出目录选择
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    HostConfig.HostValue = fbd.SelectedPath;
                    var result = CommonService.SaveHostConfig(HostConfig);
                    if (result)
                    {
                        HostConfig = CommonService.GetHostConfig(5, HostName, User.ID);
                    }
                }

            });

            ExportCommand = new DelegateCommand((obj) =>
            {
                // 导出数据
                if (Directory.Exists(HostConfig.HostValue))
                {
                    DateTime date = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
                    ExportHelper.ExportDataTableToExcel(_service.GetWorkNoEarningRatio(date), HostConfig.HostValue, $"投入产出率");
                    MessageBox.Show("导出成功");
                }
                else
                {
                    MessageBox.Show("目录不存在，请先选择导出的目录");
                }
              
            });

            ProfitLossExportCommand = new DelegateCommand((obj) =>
            {
                // 导出数据
                if (Directory.Exists(HostConfig.HostValue))
                {
                    ExportHelper.ExportDataTableToExcel(_service.GetProfitLoss(GeneralParameter.ParamBeginDate.Value,GeneralParameter.ParamEndDate.Value), HostConfig.HostValue, $"盘盈盘亏");
                    MessageBox.Show("导出成功");
                }
                else
                {
                    MessageBox.Show("目录不存在，请先选择导出的目录");
                }
            });


            GenNewDataCommand = new DelegateCommand((obj) =>
            {
                DateTime date = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
                _service.GenWorkNoEarningRatio(date);
                MessageBox.Show("更新成功");
            });

            BucketSyncCommand = new DelegateCommand((obj) =>
            {
                int count = _service.SyncBucketInfo();
                MessageBox.Show($"成功更新【{count}】条桶子名称");
            });

            PrintCommonAdjustmentCommand = new DelegateCommand((obj) =>
            {
                PrintCommonAdjustmentView view = new PrintCommonAdjustmentView();
                view.ShowDialog();
            });
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand DirectorySelectCommand { get; set; }
        public DelegateCommand GenNewDataCommand { get; set; }
        public DelegateCommand BucketSyncCommand { get; set; }
        public DelegateCommand ProfitLossExportCommand { get; set; }
        public DelegateCommand PrintCommonAdjustmentCommand { get; set; }



        private DateTime selectedDate = DateTime.Now;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                this.RaisePropertyChanged(nameof(SelectedDate));
            }
        }

        private GeneralParameterModel generalParameter;

        public GeneralParameterModel GeneralParameter
        {
            get { return generalParameter; }
            set
            {
                generalParameter = value;
                this.RaisePropertyChanged(nameof(GeneralParameter));
            }
        }




    }
}
