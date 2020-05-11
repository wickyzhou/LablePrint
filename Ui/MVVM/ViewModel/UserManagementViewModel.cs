using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;
using Ui.MVVM.Service;
using Ui.MVVM.View;

namespace Ui.MVVM.ViewModel
{
    public class UserManagementViewModel: ViewModelBase
    {
        private UserService _userService;
        private UserRoleService _userRoleService;
        private RoleService _roleService;

        public UserManagementViewModel()
        {   
            // 初始化服务
            _userService = new UserService();
            _userRoleService = new UserRoleService();
            _roleService = new RoleService();

            // 加载DataGrid数据
            UserList = new ObservableCollection<UserEntity>();
            UserRoleList = new ObservableCollection<UserRoleEntity>();
            RoleList = new ObservableCollection<RoleEntity>();
            GetAllUsers();
            GetAllUserRoles();
            GetAllRoles();

            // 加载命令对应的委托方法
            UserAddCommand = new RelayCommand(UserAdd);
            UserModifyCommand = new RelayCommand(UserModify);
            UserReMoveCommand = new RelayCommand(UserReMove);
            UserRoleSaveCommand = new RelayCommand(UserRoleSave);
            RoleAddCommand = new RelayCommand(RoleAdd);
            RoleModifyCommand = new RelayCommand(RoleModify);
            RoleReMoveCommand = new RelayCommand(RoleReMove);
        }

        #region 数据属性

            #region 用户列表
            private ObservableCollection<UserEntity> userList;

            public ObservableCollection<UserEntity> UserList
            {
                get { return userList; }
                set
                {
                    userList = value;
                    this.RaisePropertyChanged(nameof(UserList));
                }
            }

            private UserEntity userSelectedItem;

            public UserEntity UserSelectedItem
            {
                get { return userSelectedItem; }
                set
                {
                    userSelectedItem = value;
                    this.RaisePropertyChanged(nameof(UserSelectedItem));
                }
            }
            #endregion

            #region 用户角色列表
            private ObservableCollection<UserRoleEntity> userRoleList;

            public ObservableCollection<UserRoleEntity> UserRoleList
            {
                get { return userRoleList; }
                set
                {
                    userRoleList = value;
                    this.RaisePropertyChanged(nameof(UserRoleList));
                }
            }

            private UserRoleEntity userRoleSelectedItem;

            public UserRoleEntity UserRoleSelectedItem
            {
                get { return userRoleSelectedItem; }
                set
                {
                    userRoleSelectedItem = value;
                    this.RaisePropertyChanged(nameof(UserRoleSelectedItem));
                }
            }
            #endregion

            #region 角色列表
            private ObservableCollection<RoleEntity> roleList;

            public ObservableCollection<RoleEntity> RoleList
            {
                get { return roleList; }
                set
                {
                    roleList = value;
                    this.RaisePropertyChanged(nameof(RoleList));
                }
            }

            private RoleEntity roleSelectedItem;

            public RoleEntity RoleSelectedItem
            {
                get { return roleSelectedItem; }
                set
                {
                    roleSelectedItem = value;
                    this.RaisePropertyChanged(nameof(RoleSelectedItem));
                }
            }
        #endregion

        #endregion

        #region 命令属性
        public RelayCommand UserAddCommand { get; set; } //用户新增
        public RelayCommand UserModifyCommand { get; set; }//用户修改
        public RelayCommand UserReMoveCommand { get; set; } //用户删除
        public RelayCommand UserRoleSaveCommand { get; set; } //保存用户角色
        public RelayCommand RoleAddCommand { get; set; } //角色添加
        public RelayCommand RoleModifyCommand { get; set; } //角色修改
        public RelayCommand RoleReMoveCommand { get; set; } //角色删除
        #endregion

        #region 获取数据
        private void GetAllUsers()
        {
            UserList.Clear();
            _userService.GetAllUsers().ToList().ForEach(x =>
            {
                UserList.Add(x);
            });
        }

        private void GetAllUserRoles()
        {
            UserRoleList.Clear();
            _userRoleService.GetAllUserRoles().ToList().ForEach(x =>
            {
                UserRoleList.Add(x);
            });
        }

        private void GetAllRoles()
        {
            RoleList.Clear();
            _roleService.GetAllRoles().ToList().ForEach(x =>
            {
                RoleList.Add(x);
            });
        }
        #endregion

        #region 委托方法,命令实现
        private void UserAdd(object obj)
        {
            UserAddView addView = new UserAddView();// 创建视图

            //指定视图的ViewModel, 并且传递null作为用户实体，并且实现callback执行代码
            (addView.DataContext as UserAddViewModel).WithParam(null, (canExecute, val) =>
            {
                addView.Close();
                if (canExecute)
                {
                    // DataList.Add(val);
                    _userService.Insert(val);
                    GetAllUsers();
                }
            });
            // 这个是干嘛的？？？？
            var flag = addView.ShowDialog() ?? false;
        }

        private void UserModify(object obj)
        {

        }

        private void UserReMove(object obj)
        {

        }

        private void UserRoleSave(object obj)
        {

        }

        private void RoleAdd(object obj)
        {

        }

        private void RoleModify(object obj)
        {

        }

        private void RoleReMove(object obj)
        {

        }
        #endregion
    }
}
