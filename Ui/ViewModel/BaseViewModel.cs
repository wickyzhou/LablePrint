using Model;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Caching;
using System.Windows;
using Ui.Command;
using Ui.Helper;
using Ui.Service;

namespace Ui.ViewModel
{
    public class BaseViewModel : NotificationObject
    {
        public BaseViewModel()
        {
            CommonService = new CommonService();
            Menu = MemoryCache.Default["PageCache"] as MainMenuModel;
            User = (MemoryCache.Default["UserCache"] as UserCacheModel).User;
            HostName = Dns.GetHostName();
            DataGridId = Menu==null ? 0: Menu.ID * 10000 + 1;
            ComboBoxSearchService = new ComboBoxSearchService();
            ExportHelper = new DataTableImportExportHelper();
            DataGridManagementService = new DataGridManagementService();
            UserDataId = CommonService.GetUserDataId(User, Menu == null ? 0 :Menu.ID);

            PrintConfiguration = new BarTenderPrintConfigModelXX();
        }
        public MainMenuModel Menu { get; set; } 

        public UserModel User { get; set; }

        public string HostName { get; set; }

        public CommonService CommonService { get; set; } 

        public ComboBoxSearchService ComboBoxSearchService { get; set; }

        public DataGridManagementService DataGridManagementService { get; set; }


        // 导出数据主机模型
        private HostConfigModel hostConfig;

        public HostConfigModel HostConfig
        {
            get { return hostConfig; }
            set
            {
                hostConfig = value;
                this.RaisePropertyChanged(nameof(HostConfig));
            }
        }

        public DataTableImportExportHelper ExportHelper { get; set; }

        public DelegateCommand ExportBaseCommand { get; set; }

        public DelegateCommand DirectorySelectBaseCommand { get; set; }

        public DelegateCommand DataGridManageBaseCommand { get; set; }

        public DelegateCommand DataGridSaveBaseCommand { get; set; }
        
        public DelegateCommand QueryBaseCommand { get; set; }

        public int UserDataId { get; set; }

        public int DataGridId { get; set; }

        #region 打印功能

        public IEnumerable<string> ComputerPrinters { get; set; }

        public DelegateCommand PrintBaseCommand { get; set; }

        public DelegateCommand TemplateSelectBaseCommand { get; set; }

        public DelegateCommand PrintConfigurationSaveBaseCommand { get; set; }

        private IEnumerable<BarTenderTemplateModel> pirntTemplates;

        public IEnumerable<BarTenderTemplateModel> PirntTemplates
        {
            get { return pirntTemplates; }
            set
            {
                pirntTemplates = value;
                this.RaisePropertyChanged(nameof(PirntTemplates));
            }
        }

        private BarTenderTemplateModel templateSelectedItem;

        public BarTenderTemplateModel TemplateSelectedItem
        {
            get { return templateSelectedItem; }
            set
            {
                templateSelectedItem = value;
                this.RaisePropertyChanged(nameof(TemplateSelectedItem));
            }
        }

        private BarTenderPrintConfigModelXX printConfiguration;

        public BarTenderPrintConfigModelXX PrintConfiguration
        {
            get { return printConfiguration; }
            set
            {
                printConfiguration = value;
                this.RaisePropertyChanged(nameof(PrintConfiguration));
            }
        }

        #endregion
    }
}
