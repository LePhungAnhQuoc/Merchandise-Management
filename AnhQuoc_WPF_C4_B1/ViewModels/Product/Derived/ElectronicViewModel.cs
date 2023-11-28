using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ElectronicViewModel : ProductViewModel, IProductViewModel
    {
        public List<string> GetFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");

            List<string> lstTemp = GetGeneralFields(false);
            lst.AddRange(lstTemp.ToArray());

            lst.Add("Warranty");
            lst.Add("Electric Power");
            return lst;
        }

        public List<int> GetLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();
            if (displayNo)
                lst.Add(3);

            List<int> lstTemp = GetGeneralLengths(false);
            lst.AddRange(lstTemp.ToArray());

            lst.Add(10);
            lst.Add(15);
            return lst;
        }

        public List<object> GetRecords(int no, Product item)
        {
            List<object> lst = new List<object>();
            if (no != 0)
                lst.Add(no);
            Item = item;
            List<object> lstTemp = GetGeneralRecords(0);
            lst.AddRange(lstTemp.ToArray());

            lst.Add(Item.Warranty.ToString(Constants.formatWarranty));
            lst.Add(Item.ElectricPower.ToString(Constants.formatElectricPower));
            return lst;
        }

        public Product LoadItem(XmlNode nodeItem)
        {
            Product newItem = new Electronic();
            LoadGeneral(newItem, nodeItem);
            newItem.Category = ProductCategory.Electronic;

            newItem.Warranty = Convert.ToDouble(nodeItem.Attributes["Warranty"].Value);
            newItem.ElectricPower = Convert.ToDouble(nodeItem.Attributes["ElectricPower"].Value);
            return newItem;
        }

        public void WriteItem(Product product, XmlNode newNode)
        {
            XmlAttribute newAttr = null;
            WriteGeneral(product, newNode);

            newAttr = DataProvider.Instance.createAttr("Warranty");
            newAttr.Value = product.Warranty.ToString();
            newNode.Attributes.Append(newAttr);

            newAttr = DataProvider.Instance.createAttr("ElectricPower");
            newAttr.Value = product.ElectricPower.ToString();
            newNode.Attributes.Append(newAttr);
        }
    }
}
