using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ManifestationManagementApp.validation
{
    public class ContentValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            if (string.IsNullOrWhiteSpace(strValue))
            {
                return new ValidationResult(false, "Id can not contain only white spaces.");
            }
            else if (strValue.Any(ch => char.IsPunctuation(ch)))
            {
                return new ValidationResult(false, "Id can not contain punctation marks");
            }

            return new ValidationResult(true, "");
        }
    }
}
