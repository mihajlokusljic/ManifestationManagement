using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ManifestationManagementApp.validation
{
    class EnumValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "This field can not be empty. Please, choose one of the offered ones.");
            }
            if (value.ToString() == "")
            {
                return new ValidationResult(false, "This field can not be empty. Please, choose one of the offered ones.");
            }

            return new ValidationResult(true, "");
        }
        
    }
}
