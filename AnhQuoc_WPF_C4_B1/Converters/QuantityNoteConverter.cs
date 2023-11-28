using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AnhQuoc_WPF_C4_B1
{
    class QuantityNoteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double valueQuantity = 0;
            try
            {
                valueQuantity = (double)value;
            }
            catch
            {
                Utilities.CatchError();
            }
            if (valueQuantity == 0)
                return "Sold out";
            else if (valueQuantity > 0 && valueQuantity < 10)
                return "Almost Sold out";
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
