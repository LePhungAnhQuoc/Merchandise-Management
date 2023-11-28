using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductByCategory: ICloneable
    {
        public ProductCategory Category { get; set; }
        public List<Product> Products { get; set; }

        public object Clone()
        {
            var result = this.MemberwiseClone() as ProductByCategory;
            result.Products = this.Products.CloneList();
            return result;
        }

        public ProductByCategory()
        {
            Products = new List<Product>();
        }
    }
}
