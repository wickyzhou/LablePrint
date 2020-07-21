﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.Command;

namespace Ui.ViewModel
{
    public class ExportViewModel : NotificationObject
    {
        public ExportViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
            DynamicGrid1ShowCommand = new DelegateCommand((obj) =>
            {
                    DynamicVisibility = Visibility.Hidden;
            });
            DynamicGrid2ShowCommand = new DelegateCommand((obj) =>
            {
                    DynamicVisibility = Visibility.Hidden;
            });
            DynamicGrid3ShowCommand = new DelegateCommand((obj) =>
            {
                    DynamicVisibility = Visibility.Visible;
            });

            CheckBoxSelectCommand = new DelegateCommand((x)=> 
            {
                var p1 = (int)x;  // checkboxId 1\2\4\8
                //if ((CheckBoxSelectedValue | p1) == CheckBoxSelectedValue)
                    CheckBoxSelectedValue ^= p1;
                MessageBox.Show($"{CheckBoxSelectedValue}");
            });
        }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand DynamicGrid1ShowCommand { get; set; }
        public DelegateCommand DynamicGrid2ShowCommand { get; set; }
        public DelegateCommand DynamicGrid3ShowCommand { get; set; }
        public DelegateCommand CheckBoxSelectCommand { get; set; }


        private int  checkBoxSelectedValue=1;

        public int CheckBoxSelectedValue
        {
            get { return checkBoxSelectedValue; }
            set
            {
                checkBoxSelectedValue = value;
                this.RaisePropertyChanged(nameof(CheckBoxSelectedValue));
            }
        }



        private Visibility  dynamicVisibility = Visibility.Hidden;

        public Visibility DynamicVisibility
        {
            get { return dynamicVisibility; }
            set
            {
                dynamicVisibility = value;
                this.RaisePropertyChanged(nameof(DynamicVisibility));
            }
        }


        public virtual void Save(object obj)
        {
            if (Entity==3 && CheckBoxSelectedValue == 0)
            {
                MessageBox.Show("分类汇总导出必须选择类别");
                return;
            }
            CallBack?.Invoke(1, Entity, CheckBoxSelectedValue);
        }

        private void Exit(object obj)
        {
            CallBack?.Invoke(0, 0,0);
        }

        public Action<int, int, int> CallBack { get; set; }

        private int entity ;

        public int Entity
        {
            get { return entity; }
            set
            {
                entity = value;
                this.RaisePropertyChanged(nameof(Entity));
            }
        }

        public void Export(int entity, Action<int, int,int> callBack)
        {   
            Entity = entity;
            CallBack = callBack;
        }
    }
}