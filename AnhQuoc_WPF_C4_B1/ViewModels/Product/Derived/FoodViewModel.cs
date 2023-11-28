using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class FoodViewModel: ProductViewModel, IProductViewModel
    {
        public List<string> GetFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");

            List<string> lstTemp = GetGeneralFields(false);
            lst.AddRange(lstTemp.ToArray());

            lst.Add("MfgDate");
            lst.Add("ExpDate");
            return lst;
        }

        public List<int> GetLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();
            if (displayNo)
                lst.Add(3);
            lst.AddRange(GetGeneralLengths(false).ToArray());

            lst.Add(15);
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

            lst.Add(Item.MfgDate.ToString(Constants.formatDate));
            lst.Add(Item.ExpDate.ToString(Constants.formatDate));
            return lst;
        }

        public Product LoadItem(XmlNode nodeItem)
        {
            Product newItem = new Food();
            LoadGeneral(newItem, nodeItem);
            newItem.Category = ProductCategory.Food;

            newItem.MfgDate = Convert.ToDateTime(nodeItem.Attributes["MfgDate"].Value);
            newItem.ExpDate = Convert.ToDateTime(nodeItem.Attributes["ExpDate"].Value);
            return newItem;
        }

        public void WriteItem(Product product, XmlNode newNode)
        {
            XmlAttribute newAttr = null;
            WriteGeneral(product, newNode);

            newAttr = DataProvider.Instance.createAttr("MfgDate");
            newAttr.Value = product.MfgDate.ToString(Constants.formatDateTime);
            newNode.Attributes.Append(newAttr);

            newAttr = DataProvider.Instance.createAttr("ExpDate");
            newAttr.Value = product.ExpDate.ToString(Constants.formatDateTime);
            newNode.Attributes.Append(newAttr);
        }
    }
}
