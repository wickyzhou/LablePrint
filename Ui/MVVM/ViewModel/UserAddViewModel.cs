using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;
using Ui.MVVM.Service;

namespace Ui.MVVM.ViewModel
{
    public class UserAddViewModel:ViewModelBase
    {
        private Action<bool, UserEntity> CallBack;
        private EnumService _enumService;
        private OrganizationService _organizationService;

        public UserAddViewModel()
        {
            _enumService = new EnumService();
            _organizationService = new OrganizationService();
            SaveCommand = new RelayCommand(Save);
            ExitCommand = new RelayCommand(Exit);

            EnumList = new List<EnumEntity>();
            OrganizationList = new List<OrganizationEntity>();
            GetEnumList();
            GetOrganizationList();
        }

        #region 命令属性
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        #endregion

        #region 委托方法实现
        private void Save(object obj)
        {
            CallBack?.Invoke(true, User);
        }

        private void Exit(object obj)
        {
            CallBack?.Invoke(false, null);
        }

        public void WithParam(UserEntity user, Action<bool, UserEntity> callBack)
        {
            User = user ?? new UserEntity();
            CallBack = callBack;
        }

        #endregion

        #region 数据属性

        private UserEntity user;

        public UserEntity User
        {
            get { return user; }
            set
            {
                user = value;
                this.RaisePropertyChanged(nameof(User));
            }
        }

        private List<EnumEntity> enumList;

        public List<EnumEntity> EnumList
        {
            get { return enumList; }
            set
            {
                enumList = value;
                this.RaisePropertyChanged(nameof(EnumList));
            }
        }

        private List<OrganizationEntity> organizationList;

        public List<OrganizationEntity> OrganizationList
        {
            get { return organizationList; }
            set
            {
                organizationList = value;
                this.RaisePropertyChanged(nameof(OrganizationList));
            }
        }

        #endregion

        #region 获取数据
        public void GetEnumList()
        {
            EnumList.Clear();
            _enumService.GetEnumByGroupSeq(0).ToList().ForEach(x=> { EnumList.Add(x);    });

        }
        public void GetOrganizationList()
        {
            organizationList.Clear();
            _organizationService.GetAllOrganizations().ToList().ForEach(x=>OrganizationList.Add(x));
        }
        #endregion
    }
}
