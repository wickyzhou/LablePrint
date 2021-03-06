﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ui.Service;

namespace Ui.ViewModel
{
    public class SalesRebateAmountRangeCreateViewModel: NewDialogViewModel<SalesRebateRecentParameterSonModel>
    {
        public SalesRebateAmountRangeCreateViewModel()
        {
            IsValidLists = CommonService.GetEnumLists(999);
        }

        public override void Save(object obj)
        {
     
            if (Entity.AmountUpper < 0 || Entity.AmountLower < 0 || Entity.SalesRebatePctValue <= 0)
            {
                MessageBox.Show("必须填写上下限、比例值、是否有效");
                return;
            }
            else if (Entity.AmountLower >= Entity.AmountUpper)
            {
                MessageBox.Show("下限不能高于上限值");
                return;
            }
            else if (Entity.SalesRebatePctValue <= 0 || Entity.SalesRebatePctValue > 80)
            {
                MessageBox.Show("返利百分比必须为 0 到 80 ");
                return;
            }
            base.Save(obj);
        }


        private IEnumerable<EnumModel> isValidLists;

        public IEnumerable<EnumModel> IsValidLists
        {
            get { return isValidLists; }
            set
            {
                isValidLists = value;
                this.RaisePropertyChanged(nameof(IsValidLists));
            }
        }

        public EnumModel IsValidSelectedItem { get; set; }
    }
}
