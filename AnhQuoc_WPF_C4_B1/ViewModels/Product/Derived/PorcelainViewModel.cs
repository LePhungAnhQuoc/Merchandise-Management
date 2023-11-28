using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class PorcelainViewModel: ProductViewModel, IProductViewModel
    {
        public List<string> GetFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");
            lst.AddRange(GetGeneralFields(false).ToArray());

            lst.Add("Material");
            return lst;
        }

        public List<int> GetLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);
            lst.AddRange(GetGeneralLengths(false).ToArray());

            lst.Add(15);
            return lst;
        }

        public List<object> GetRecords(int no, Product item)
        {
            List<object> lst = new List<object>();
            Item = item;

            if (no != 0)
                lst.Add(no);
            lst.AddRange(GetGeneralRecords(0).ToArray());

            lst.Add(Item.Material);
            return lst;
        }

        public Product LoadItem(XmlNode nodeItem)
        {
            Product newItem = new Porcelain();
            LoadGeneral(newItem, nodeItem);
            newItem.Category = ProductCategory.Porcelain;

            newItem.Material = nodeItem.Attributes["Material"].Value;
            return newItem;
        }

        public void WriteItem(Product product, XmlNode newNode)
        {
            XmlAttribute newAttr = null;
            WriteGeneral(product, newNode);
            
            newAttr = DataProvider.Instance.createAttr("Material");
            newAttr.Value = product.Material.ToString();
            newNode.Attributes.Append(newAttr);
        }
    }
}
