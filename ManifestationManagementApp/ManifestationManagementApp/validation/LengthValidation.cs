using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ManifestationManagementApp.validation
{
    public class LengthValidation : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            if (value == null)
            {
                return new ValidationResult(false, "This field can not be empty");
            }
            if (strValue.Length == 0 || strValue == null)
            {
                return new ValidationResult(false, "This field can not be empty");
            }

            else if (strValue.Length < Min)
            {
                return new ValidationResult(false, "Minimum size is " + Min + " characters");
            }

            else if (strValue.Length > Max)
            {
                return new ValidationResult(false, "Maximum size is " + Max + " characters");
            }

            return new ValidationResult(true, "");
        }
        
    }
}
