using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class EnumViewModel
    {
        public IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
        public void Output<T>(string format)
        {
            IEnumerable<T> lst = GetValues<T>();
            Utilities.Output(format, lst);
        }
        public void Output<T>()
        {
            Output<T>("{0}. {1}");
        }
        public int Choose<T>()
        {
            int option;
            int count = Enum.GetNames(typeof(T)).Length;
            Utilities.ReadLine(out option, ">= && <=", 1, count);
            return option;
        }
    }
}
