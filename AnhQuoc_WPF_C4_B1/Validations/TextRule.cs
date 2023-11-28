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
    public class TextRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strInput = (string)value;

            if (strInput == string.Empty)
                return new ValidationResult(false, "This text box can not empty");

            if (Utilities.ValidateText(strInput) == false)
                return new ValidationResult(false, "Please enter alphabetic characters.");

            return ValidationResult.ValidResult;
        }
    }
}
