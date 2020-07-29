using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using Ui.Service;

namespace Ui.ViewModel
{
    public class BaseViewModel : NotificationObject
    {

        public UserModel User { get; set; } = (MemoryCache.Default["UserCache"] as UserCacheModel).User ;

        public string HostName { get; set; } = Dns.GetHostName();

        public CommonService CommonService { get; set; } = new CommonService();

        public ComboBoxSearchService ComboBoxSearchService { get; set; } = new ComboBoxSearchService();

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

        public DataTableImportExportHelper ExportHelper { get; set; } = new DataTableImportExportHelper();


    }
}
