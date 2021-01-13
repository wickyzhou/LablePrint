using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Ui.Command;
using Ui.Service;

namespace Ui.View
{
    public class HamburgerMenuViewModel:NotificationObject
    {

        UserModel User = (MemoryCache.Default["UserCache"] as UserCacheModel).User;

        public AdminService AdminService { get; set; } = new AdminService();

        public HamburgerMenuViewModel()
        {
            InitCommand();
            InitData();
        }

        private void InitCommand()
        {
            NavigateToPageCommand = new DelegateCommand((obj) =>
            {
                if (MainMenuSelectedItem != null)
                {
                    ((((obj as Grid).Parent as HamburgerMenu).Parent as Grid).FindName("MainFrame") as Frame).Navigate(new Uri(MainMenuSelectedItem.Uri, UriKind.Relative));
                    ObjectCache cache = MemoryCache.Default;
                    cache.Set("PageCache", MainMenuSelectedItem, new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(24.0) });
                }
                //this.MainFrame.Navigate(new Uri(IndexPageValue, UriKind.Relative));
            });

            NavigateToIndexCommand = new DelegateCommand((obj) =>
            {
                var s1 = ((obj as Grid).Parent as HamburgerMenu).Parent as Grid;

                if (!string.IsNullOrEmpty(IndexPageValue))
                    ((((obj as Grid).Parent as HamburgerMenu).Parent as Grid).FindName("MainFrame") as Frame).Navigate(new Uri(IndexPageValue, UriKind.Relative));
                else
                    ((((obj as Grid).Parent as HamburgerMenu).Parent as Grid).FindName("MainFrame") as Frame).Navigate(new Uri("/View/IndexPage/UserDefaultPage.xaml", UriKind.Relative));
                MainMenuSelectedItem = null;
            });

        }

        private void InitData()
        {
            MainMenuLists.Clear();
            AdminService.GetMainMenuLists(User).ForEach(x => MainMenuLists.Add(x));
            IndexPageValue = AdminService.GetUserIndexPage(User)?.DefaultPage;
        }

        public DelegateCommand NavigateToPageCommand { get; set; }

        public DelegateCommand NavigateToIndexCommand { get; set; }

        private ObservableCollection<MainMenuModel> mainMenuLists = new ObservableCollection<MainMenuModel>();

        public ObservableCollection<MainMenuModel> MainMenuLists
        {
            get { return mainMenuLists; }
            set
            {
                mainMenuLists = value;
                this.RaisePropertyChanged(nameof(MainMenuLists));
            }
        }

        private MainMenuModel mainMenuSelectedItem;

        public MainMenuModel MainMenuSelectedItem
        {
            get { return mainMenuSelectedItem; }
            set
            {
                mainMenuSelectedItem = value;
                this.RaisePropertyChanged(nameof(MainMenuSelectedItem));
            }
        }

        public string IndexPageValue { get; set; }

        private bool isOpen = true;

        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                this.RaisePropertyChanged(nameof(IsOpen));
            }
        }

    }
}
