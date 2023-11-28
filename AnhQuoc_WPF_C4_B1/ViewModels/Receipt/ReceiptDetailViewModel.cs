using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ReceiptDetailViewModel
    {
        public ReceiptDetail Item { get; set; }
        public List<ReceiptDetail> Items { get { return ItemList.ListDetail; } }
        public ReceiptDetailList ItemList = new ReceiptDetailList();
        public List<Product> Products { get; set; }

        public List<string> GetFields(bool displayNo = false)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");

            List<string> lstTemp = productVM.GetGeneralFields(false);
            lst.AddRange(lstTemp.ToArray());

            lst.Add("Quantity");
            lst.Add("TotalPrice");
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
            lst.Add(Item.TotalPrice.In.ToString(Constants.formatCurrency)); 
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

            foreach (ReceiptDetail item in Items)
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
            strOutput = ItemList.TotalPrice.In.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[8]);

            Utilities.WriteLine();
        }
        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
            if (Items.Count > 1)
                OutputBottom(displayNo);
        }

        public void WriteItems(XmlNode parentNode, string childName)
        {
            foreach (ReceiptDetail detail in Items)
            {
                Item = detail;
                XmlNode newNode = DataProvider.Instance.createNode(childName);
                parentNode.AppendChild(WriteItem(newNode));
            }
        }
        public XmlNode WriteItem(XmlNode newNode)
        {
            XmlNode newAttr = null;
            newAttr = DataProvider.Instance.createNode("Id");
            newAttr.InnerText = Item.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("IdReceipt");
            newAttr.InnerText = Item.IdReceipt;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Product");
            newAttr.InnerText = Item.Product.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Quantity");
            newAttr.InnerText = Item.Quantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalPriceInput");
            newAttr.InnerText = Item.TotalPrice.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalPriceOutput");
            newAttr.InnerText = Item.TotalPrice.Out.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Date");
            newAttr.InnerText = Item.Date.ToString(Constants.formatDateTime);
            newNode.AppendChild(newAttr);
            return newNode;
        }

        public List<ReceiptDetail> LoadItems(XmlNodeList lstNode)
        {
            ItemList = new ReceiptDetailList();
            foreach (XmlNode nodeData in lstNode)
            {
                Items.Add(LoadItem(nodeData));
            }
            return Items;
        }
        public ReceiptDetail LoadItem(XmlNode nodeData)
        {
            string strTemp = string.Empty;
            ReceiptDetail newItem = new ReceiptDetail();

            XmlNode nodeTemp = nodeData.FirstChild;
            newItem.Id = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            newItem.IdReceipt = nodeTemp.InnerText;
            
            nodeTemp = nodeTemp.NextSibling;
            newItem.Product.Id = nodeTemp.InnerText;

            ProductViewModel ProductViewModel = new ProductViewModel();
            ProductViewModel.Items = Products;

            var productItem = ProductViewModel.FindById(newItem.Product.Id);
            if (productItem == null)
            {
                Utilities.CatchError();
            }
            newItem.Product = productItem;

            nodeTemp = nodeTemp.NextSibling;
            newItem.Quantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.TotalPrice.In = Convert.ToDouble(nodeTemp.InnerText);
            nodeTemp = nodeTemp.NextSibling;
            newItem.TotalPrice.Out = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.Date = Convert.ToDateTime(nodeTemp.InnerText);
            return newItem;
        }

        public string GetId(int no)
        {
            string newId = string.Empty;
            newId = no.ToString();
            return newId;
        }
        public double GetTotalQuantity(List<ReceiptDetail> details)
        {
            double result = 0.0;
            foreach (var item in details)
            {
                result += item.Quantity;
            }
            return result;
        }
        public Price GetTotalPrice(List<ReceiptDetail> details)
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

            Price result = GetTotalPrice(ItemList.ListDetail);
            ItemList.TotalPrice.In = result.In;
            ItemList.TotalPrice.Out = result.Out;
        }

        public void GetTotalPriceSingle(List<ReceiptDetail> details)
        {
            foreach (ReceiptDetail detail in details)
            {
                GetTotalPriceSingle(detail);
            }
        }
        public void GetTotalPriceSingle(ReceiptDetail detail)
        {
            detail.TotalPrice.In = detail.Quantity * detail.Product.Price.In;
            detail.TotalPrice.Out = detail.Quantity * detail.Product.Price.Out;
        }

        public ReceiptDetail FindByIdProduct(string idProduct)
        {
            foreach (ReceiptDetail item in Items)
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
