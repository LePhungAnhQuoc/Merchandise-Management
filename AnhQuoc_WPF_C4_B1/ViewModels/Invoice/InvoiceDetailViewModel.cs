using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class InvoiceDetailViewModel
    {
        public InvoiceDetail Item;
        public List<InvoiceDetail> Items { get { return ItemList.ListDetail; } }
        public InvoiceDetailList ItemList = new InvoiceDetailList();
        public List<List<ProductInventory>> ListProducts { get; set; }
        public List<Product> Products { get; set; }

        public XmlNode WriteItems(XmlNode parentNode, string nodeName)
        {
            foreach (InvoiceDetail item in Items)
            {
                XmlNode newNode = DataProvider.Instance.createNode(nodeName);
                WriteItem(newNode, item);
                parentNode.AppendChild(newNode);
            }
            return parentNode;
        }

        public XmlNode WriteItem(XmlNode newNode, InvoiceDetail item)
        {
            Item = item;
            XmlNode newAttr = null;

            newAttr = DataProvider.Instance.createNode("Id");
            newAttr.InnerText = item.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("IdInvoice");
            newAttr.InnerText = item.IdInvoice;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Product");
            newAttr.InnerText = item.Product.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Quantity");
            newAttr.InnerText = item.Quantity.ToString();
            newNode.AppendChild(newAttr);
            
            newAttr = DataProvider.Instance.createNode("TotalPriceInput");
            newAttr.InnerText = item.TotalPrice.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalPriceOutput");
            newAttr.InnerText = item.TotalPrice.Out.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Date");
            newAttr.InnerText = item.Date.ToString(Constants.formatDateTime);
            newNode.AppendChild(newAttr);

            return newNode;
        }

        public List<InvoiceDetail> LoadItems(XmlNodeList lstNode)
        {
            List<InvoiceDetail> items = new List<InvoiceDetail>();
            foreach (XmlNode nodeData in lstNode)
            {
                items.Add(LoadItem(nodeData));
            }
            return items;
        }

        public InvoiceDetail LoadItem(XmlNode nodeData)
        {
            Item = new InvoiceDetail();
            XmlNode nodeTemp = nodeData.FirstChild;
            Item.Id = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            Item.IdInvoice = nodeTemp.InnerText;
            
            nodeTemp = nodeTemp.NextSibling;
            Item.Product.Id = nodeTemp.InnerText;

            ProductViewModel ProductViewModel = new ProductViewModel();
            ProductViewModel.Items = Products;
            var productItem = ProductViewModel.FindById(Item.Product.Id);
            if (productItem == null)
            {
                Utilities.CatchError();
            }
            Item.Product = productItem;

            nodeTemp = nodeTemp.NextSibling;
            Item.Quantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            Item.TotalPrice.In = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            Item.TotalPrice.Out = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            Item.Date = Convert.ToDateTime(nodeTemp.InnerText);

            return Item;
        }

        public List<string> GetFields(bool displayNo = false)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");

            List<string> lstTemp = productVM.GetGeneralFields(false);
            lst.AddRange(lstTemp.ToArray());

            lst.Add("Quantity");
            lst.Add("TotalPriceOutput");
            lst.Add("Date");
            return lst;
        }

        public List<int> GetLengths(bool displayNo = false)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);

            List<int> lstTemp = productVM.GetGeneralLengths(false);
            lst.AddRange(lstTemp.ToArray());

            lst.Add(10);
            lst.Add(15);
            lst.Add(20);
            return lst;
        }

        public List<object> GetRecords(int no = 0)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<object> lst = new List<object>();

            if (no != 0)
                lst.Add(no);
            productVM.Item = Item.Product;
            List<object> lstTemp = productVM.GetGeneralRecords(0);
            lst.AddRange(lstTemp.ToArray());

            lst.Add(Item.Quantity.ToString(Constants.formatThousand));
            lst.Add(Item.TotalPrice.Out.ToString(Constants.formatCurrency));
            lst.Add(Item.Date.ToString(Constants.formatDateTime));
            return lst;
        }

        public void OutputTable(bool displayNo = false)
        {
            int no = (displayNo) ? 1 : 0;
            List<string> fields = GetFields(displayNo);
            List<int> lengths = GetLengths(displayNo);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            foreach (InvoiceDetail item in Items)
            {
                Item = item;
                List<object> records = GetRecords(no);
                Utilities.Output(records.ToArray(), lengths);

                no += (no != 0) ? 1 : 0;
            }
        }

        public void OutputBottom(bool displayNo = false)
        {
            string strOutput = string.Empty;

            List<int> fixLength = Utilities.GetFixLengths(GetLengths(displayNo), 0);
            strOutput = ItemList.TotalQuantity.ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[7]);

            strOutput = ItemList.TotalPrice.Out.ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[8]);

            Utilities.WriteLine();
        }

        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
            if (Items.Count > 1)
                OutputBottom(displayNo);
        }

        public string GetId(int no) => no.ToString();

        public double GetTotalQuantity(List<InvoiceDetail> details)
        {
            double result = 0.0;
            foreach (var item in details)
            {
                result += item.Quantity;
            }
            return result;
        }

        public Price GetTotalPrice(List<InvoiceDetail> details)
        {
            Price result = new Price(0, 0);
            foreach (var item in details)
            {
                result.In += item.TotalPrice.In;
                result.Out += item.TotalPrice.Out;
            }
            return result;
        }

        public void GetListInfo()
        {
            ItemList.TotalQuantity = GetTotalQuantity(ItemList.ListDetail);
            ItemList.TotalPrice = GetTotalPrice(ItemList.ListDetail);
        }

        public void GetTotalPriceSingle(List<InvoiceDetail> details)
        {
            foreach (InvoiceDetail detail in details)
            {
                GetTotalPriceSingle(detail);
            }
        }
        public void GetTotalPriceSingle(InvoiceDetail detail)
        {
            detail.TotalPrice.In = detail.Quantity * detail.Product.Price.In;
            detail.TotalPrice.Out = detail.Quantity * detail.Product.Price.Out;
        }

        public InvoiceDetail FindByIdProduct(string idProduct)
        {
            foreach (InvoiceDetail item in Items)
            {
                if (item.Product.Id == idProduct)
                {
                    return item;
                }
            }
            return null;
        }

    }
}
