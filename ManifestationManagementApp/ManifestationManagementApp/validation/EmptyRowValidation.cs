using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ManifestationManagementApp.validation
{
    public class EmptyRowValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Type of manifestation can not be empty. Please, choose one of the offered types.");
            }
            string strValue = Convert.ToString(value);
            if (strValue.Length == 0 || strValue == null)
            {
                return new ValidationResult(false, "Type of manifestation can not be empty. Please, choose one of the offered types.");
            }

            return new ValidationResult(true, "");
        }
    }
}
