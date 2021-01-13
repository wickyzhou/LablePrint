using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Model;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using Ui.Command;
using Ui.Service;

namespace Ui.View
{
    public class UserMainWindowViewModel:NotificationObject
    {
		UserModel User = (MemoryCache.Default["UserCache"] as UserCacheModel).User;

		public AdminService AdminService { get; set; } = new AdminService();

		public UserMainWindowViewModel()
		{
			var s = AdminService.GetUserIndexPage(User);
			PageUri = s==null ? "/View/IndexPage/UserDefaultPage.xaml" :s.DefaultPage;
		}
		private string pageUri = "/View/IndexPage/UserDefaultPage.xaml";

		public string PageUri
		{
			get { return pageUri; }
			set
			{
				pageUri = value;
				this.RaisePropertyChanged(nameof(PageUri));
			}
		}


	}
}
