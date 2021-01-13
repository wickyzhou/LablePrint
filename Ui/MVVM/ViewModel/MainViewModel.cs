using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;
using Ui.MVVM.Service;
using Ui.MVVM.View;

namespace Ui.MVVM.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private EmpolyeeService _service;

        public MainViewModel()
        {
            _service = new EmpolyeeService();
            DataList = new ObservableCollection<Employee>();

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);

            GetAll();
            InitCommand();
        }

        private void InitCommand()
        {
            ExportCommand = new RelayCommand((obj)=> {
                var ss = 234;
            });
        }

        private string _displayName { get; set; }
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private ObservableCollection<Employee> _dataList;
        public ObservableCollection<Employee> DataList
        {
            get => _dataList;
            set
            {
                _dataList = value;
                OnPropertyChanged(nameof(DataList));
            }
        }

        private Employee _selectedItem;
        public Employee SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }



        private void Search(object obj)
        {
            GetAll();
        }
        private void Add(object obj)
        {
            EditView edit = new EditView();

            (edit.DataContext as EditViewModel).WithParam(null, (type, val) =>
            {
                edit.Close();
                if (type == 1)
                {
                    // DataList.Add(val);
                    _service.Insert(val);
                    GetAll();
                }
            });
            var flag = edit.ShowDialog() ?? false;
        }

        private void Edit(object obj)
        {
            if (SelectedItem == null) return;

            EditView edit = new EditView();

            (edit.DataContext as EditViewModel).WithParam(SelectedItem, (type, val) =>
            {
                edit.Close();
                if (type == 1)
                {
                    // DataList.Add(val);
                    _service.Update(val);
                    GetAll();
                }
            });
            edit.ShowDialog();
        }

        private void Delete(object obj)
        {
            if (SelectedItem == null) return;

            //DataList.Remove(SelectedItem);
            _service.Delete(SelectedItem.Id);
            GetAll();
        }

        private void GetAll()
        {
            DataList.Clear();
            _service.GetAll().ToList().ForEach(x =>
            {
                DataList.Add(x);
            });

            HostConfig = new HostConfigModel { HostValue = "默认位置" };
        }

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
    }
}
