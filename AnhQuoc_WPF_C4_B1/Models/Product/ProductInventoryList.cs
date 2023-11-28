using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInventoryList
    {
        public List<ProductInventory> Items { get; set; }
        public double TotalQuantity { get; set; }
        public Price TotalPrice { get; set; }

        public ProductInventoryList()
        {
            Items = new List<ProductInventory>();
            TotalPrice = new Price();
        }
    }
}
