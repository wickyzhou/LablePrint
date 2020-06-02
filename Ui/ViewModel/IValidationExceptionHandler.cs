using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ui.ViewModel
{
    interface IValidationExceptionHandler
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        bool IsValid
        {
            get;
            set;

        }
    }
}
