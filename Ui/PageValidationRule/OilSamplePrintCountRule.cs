using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Ui.PageValidationRule
{
    public class OilSamplePrintCountRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "不能为空值！");
            if (!IsValidNumeric(value.ToString()))
                return new ValidationResult(false, "数值要求   0＜V≤4");

            return new ValidationResult(true, null);
        }

        private bool IsValidNumeric(string obj)
        {
            if (double.TryParse(obj, out double r))
            {
                if (r> 0 && r<=4)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
