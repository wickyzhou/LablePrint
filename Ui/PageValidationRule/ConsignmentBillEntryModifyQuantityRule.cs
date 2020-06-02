using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Ui.PageValidationRule
{
   public class ConsignmentBillEntryModifyQuantityRule: ValidationRule
    {
        public float MaxQuatity { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "不能为空值！");
            if (!IsCorrectNumeric(value.ToString()))
                return new ValidationResult(false, "数值必须是大于等于0且小于可用数量");

            return new ValidationResult(true, null);
        }

        private bool IsCorrectNumeric(string obj)
        {
            if (double.TryParse(obj, out double r))
            {
                if (r >= 0 && r <= MaxQuatity)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
