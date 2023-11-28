using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AnhQuoc_WPF_C4_B1
{
    [ValueConversion(typeof(string[]), typeof(string))]
    public class StringArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null)
            //    return string.Empty;
            //List<Genre> values = value as List<Genre>;
            //if (values == null)
            //    return string.Empty;

            //int index = 0;
            //string[] arrStr = new string[values.Count];
            //foreach (var item in values)
            //{
            //    arrStr[index++] = item.Name;
            //}

            ////string separator = parameter == null ? string.Empty : parameter.ToString();
            //string separator = ", ";

            //string result = string.Join(separator, arrStr);
            ////result = result.Remove(result.Length - 2);
            //return result;
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
