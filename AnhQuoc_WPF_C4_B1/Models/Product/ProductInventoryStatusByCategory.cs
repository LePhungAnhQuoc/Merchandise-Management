using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInventoryStatusByCategory
    {
        public ProductCategory Category { get; set; }
        public List<ProductInventoryStatus> Products { get; set; }

        public ProductInventoryStatusByCategory()
        {
            Products = new List<ProductInventoryStatus>();
        }
    }
}
