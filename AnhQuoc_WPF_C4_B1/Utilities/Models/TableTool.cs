using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class TableTool
    {
        public List<string> Fields { get; set; }
        public List<double> Lengths { get; set; }
        public List<string> Records { get; set; }

        public int Count { get
            {
                return Fields.Count;
            }
        }
    }
}
