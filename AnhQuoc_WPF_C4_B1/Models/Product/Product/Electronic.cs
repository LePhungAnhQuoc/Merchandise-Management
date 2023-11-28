using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    class Electronic: Product
    {
        public override double Warranty { get; set; }
        public override double ElectricPower { get; set; }

        public Electronic() : base()
        {
        }
    }
}
