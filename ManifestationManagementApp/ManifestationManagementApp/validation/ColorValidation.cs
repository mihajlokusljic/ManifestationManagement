using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ManifestationManagementApp.validation
{
    class ColorValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Please pick some icon.");
            }
            string strValue = Convert.ToString(value);
            if (strValue.Length == 0 || strValue == null)
            {
                return new ValidationResult(false, "Label of manifestation can not be empty. Please, choose one or more of the offered types.");
            }

            return new ValidationResult(true, "");
        }
        
    }
}
