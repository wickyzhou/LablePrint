using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Ui.PageValidationRule
{
    public class DateRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "不能为空值！");
            if (!DateTime.TryParse(value.ToString(), out _))
                return new ValidationResult(false, "必须为日期");

            return new ValidationResult(true, null);
        }

    }
}
