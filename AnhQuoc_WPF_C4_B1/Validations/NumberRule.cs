using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AnhQuoc_WPF_C4_B1
{ 
    public class NumberRule : ValidationRule
    {
        public bool IsCanEmpty { get; set; }

        public NumberRule()
        {
            IsCanEmpty = false;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strInput = (string)value;

            if (strInput == string.Empty)
            {
                if (IsCanEmpty)
                    return ValidationResult.ValidResult;
                return new ValidationResult(false, "This text box can not empty");
            }

            if (Utilities.ValidateNumber(strInput, 0, 9) == false)
                return new ValidationResult(false, "Please enter a valid number");

            return ValidationResult.ValidResult;
        }
    }
}
