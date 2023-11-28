using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AnhQuoc_WPF_C4_B1
{
    class NoteEmptyListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int getValue = -1;
            try
            {
                getValue = (int)value;
            }
            catch
            {
                Utilities.CatchError();
            }
            return getValue == 0 ? Utilities.GetListEmptyMessage("item") : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
