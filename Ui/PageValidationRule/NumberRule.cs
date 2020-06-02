using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Ui.PageValidationRule
{
    public class NumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
       {
            if (value == null)
                return new ValidationResult(false, "不能为空值！");
            if (!IsNumeric(value.ToString()))
                return new ValidationResult(false, "数值必须是大于等于0");

            return new ValidationResult(true, null);
        }

        private bool IsNumeric(string obj)
        {
            if (double.TryParse(obj, out double r))
            {
                if (r >= 0 )
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
