using Bll.Services;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.Command;
using Ui.ViewModel;

namespace Ui.View.Bucket
{
    public class BucketModifyViewModel:RefreshDialogViewModel<BucketModel>
    {
        public override void Save(object obj)
        {
            IsChanged = true;
            base.Save(obj);
        }
    }
}
