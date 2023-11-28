using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInvoiceByCategory: ICloneable
    {
        public ProductCategory Category { get; set; }
        public List<ProductInvoice> Products { get; set; }

        public ProductInvoiceByCategory()
        {
            Products = new List<ProductInvoice>();
        }

        public object Clone()
        {
            var result = this.MemberwiseClone() as ProductInvoiceByCategory;
            result.Products = this.Products.CloneList();
            return result;
        }
    }
}
