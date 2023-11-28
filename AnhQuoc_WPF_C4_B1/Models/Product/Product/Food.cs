using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    class Food: Product
    {
        public override DateTime MfgDate { get; set; }
        public override DateTime ExpDate { get; set; }

        public Food() : base()
        {
        }
    }
}
