using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public static class ObjectExtensions
    {
        public static List<T> CloneList<T>(this List<T> listSource) where T : class, ICloneable
        {
            return listSource.Select(item => (item as T).Clone() as T).ToList();
        }
    }
}
