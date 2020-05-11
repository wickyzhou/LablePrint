using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;
using Ui.MVVM.Entity;

namespace Ui.MVVM.ViewModel
{
    public class EditViewModel : ViewModelBase
    {
        private Action<int, Employee> CallBack;

        public EditViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            ExitCommand = new RelayCommand(Exit);

            Init();
        }

        private Employee _data;
        public Employee Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private List<StaticResourceModel> _sexList;
        public List<StaticResourceModel> SexList
        {
            get => _sexList;
            set
            {
                _sexList = value;
                OnPropertyChanged(nameof(SexList));
            }
        }



        public void WithParam(Employee employee, Action<int, Employee> callBack)
        {
            Data = employee ?? new Employee();
            CallBack = callBack;
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }


        private void Save(object obj)
        {
            CallBack?.Invoke(1, Data);
        }

        private void Exit(object obj)
        {
            CallBack?.Invoke(0, null);
        }

        private void Init()
        {
            SexList = new List<StaticResourceModel>
            {
                new StaticResourceModel { Key =0,Value = "男"},
                new StaticResourceModel { Key =1,Value = "女"}
            };
        }
    }
}
