using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInventoryByCategory: ICloneable
    {
        public ProductCategory Category { get; set; }
        public List<ProductInventory> Products { get; set; }

        public ProductInventoryByCategory()
        {
            Products = new List<ProductInventory>();
        }

        public object Clone()
        {
            var result = this.MemberwiseClone() as ProductInventoryByCategory;
            result.Products = this.Products.CloneList();

            return result;
        }
    }
}
