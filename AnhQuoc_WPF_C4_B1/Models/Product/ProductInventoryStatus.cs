using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInventoryStatus
    {
        public ProductInventory Item { get; set; }

        // Dau ky
        public double PreviousQuantity { get; set; }
        public Price PreviousAmount { get; set; }
        public DateTime PreviousDate { get; set; }

        // Cuoi ky
        public double RecentQuantity { get; set; }
        public Price RecentAmount { get; set; }
        public DateTime RecentDate { get; set; }
      
        public ProductInventoryStatus()
        {
            PreviousAmount = new Price();
            RecentAmount = new Price();
            Item = new ProductInventory();
        }
    }
}
