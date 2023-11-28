using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class Product: ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public string Producer { get; set; }
        public Price Price { get; set; }
        
        // Electronics
        public virtual double Warranty { get; set; }
        public virtual double ElectricPower { get; set; }

        // Porcelain
        public virtual string Material { get; set; }

        // Food
        public virtual DateTime MfgDate { get; set; }
        public virtual DateTime ExpDate { get; set; }

        public Product()
        {
            Price = new Price();
        }

        public object Clone()
        {
            var result = this.MemberwiseClone() as Product;
            result.Price = this.Price.Clone() as Price;
            return result;
        }
    }
}
