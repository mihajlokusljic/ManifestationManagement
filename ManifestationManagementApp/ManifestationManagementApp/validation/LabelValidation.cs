using ManifestationManagementApp.model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ManifestationManagementApp.validation
{
    public class LabelValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);
            foreach (model.Label lab in Repository.GetInstance().Labels)
            {
                if (lab.Id.Equals(strValue))
                {
                    return new ValidationResult(false, "Entered id already exists. Please enter a different value.");
                }
            }

            return new ValidationResult(true, "");
        }
        
    }
    
}
