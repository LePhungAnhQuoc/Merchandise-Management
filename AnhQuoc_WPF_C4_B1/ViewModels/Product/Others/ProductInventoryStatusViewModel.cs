using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInventoryStatusViewModel
    {
        public ProductInventoryStatus Item { get; set; }
        public List<ProductInventoryStatus> Items { get; set; }
        public List<Product> Products { get; set; }

        public void Update(Receipt receipt)
        {
            foreach (ReceiptDetail detail in receipt.Details.ListDetail)
            {
                ProductInventoryStatus item = Find(detail.Product);

                if (item == null)
                {
                    ProductInventoryStatus newItem = new ProductInventoryStatus();
                    newItem.Item.Product = detail.Product;

                    newItem.PreviousQuantity = 0;
                    newItem.PreviousAmount = new Price(0, 0);
                    newItem.PreviousDate = Convert.ToDateTime(Constants.dateNone);

                    newItem.RecentQuantity = detail.Quantity;
                    newItem.RecentAmount = new Price();
                    newItem.RecentAmount.In = detail.TotalPrice.In;
                    newItem.RecentAmount.Out = detail.TotalPrice.Out;
                    newItem.RecentDate = detail.Date;

                    newItem.Item.Quantity = newItem.RecentQuantity;
                    newItem.Item.Price = new Price();
                    newItem.Item.Price.In = newItem.RecentAmount.In;
                    newItem.Item.Price.Out = newItem.RecentAmount.Out;

                    Items.Add(newItem);
                }
                else
                {
                    ProductInventoryStatus newItem = item;

                    newItem.PreviousQuantity += newItem.RecentQuantity;
                    newItem.PreviousAmount.In += newItem.RecentAmount.In;
                    newItem.PreviousAmount.Out += newItem.RecentAmount.Out;
                    newItem.PreviousDate = newItem.RecentDate;

                    newItem.RecentQuantity = detail.Quantity;
                    newItem.RecentAmount = new Price();
                    newItem.RecentAmount.In = detail.TotalPrice.In;
                    newItem.RecentAmount.Out = detail.TotalPrice.Out;
                    newItem.RecentDate = detail.Date;

                    newItem.Item.Quantity += newItem.RecentQuantity;
                    newItem.Item.Price.In += newItem.RecentAmount.In;
                    newItem.Item.Price.Out += newItem.RecentAmount.Out;
                }
            }
        }
        public void Update(List<Receipt> receipts)
        {
            foreach (Receipt item in receipts)
            {
                Update(item);
            }
        }
        public List<string> GetFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");

            lst.Add("IdProduct");
            lst.Add("Name");
            lst.Add("Category");

            lst.Add("Previous");
            lst.Add("AmountInput");
            lst.Add("Date");

            lst.Add("Recent");
            lst.Add("AmountInput");
            lst.Add("Date");

            lst.Add("Total quantity");
            lst.Add("Total AmountInput");
            return lst;
        }
        public List<int> GetLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);

            lst.Add(10);
            lst.Add(20);
            lst.Add(10);

            lst.Add(10);
            lst.Add(15);
            lst.Add(10);

            lst.Add(10);
            lst.Add(15);
            lst.Add(10);
            
            lst.Add(15);
            lst.Add(15);
            return lst;
        }
        public List<object> GetRecords(int no = 0)
        {
            List<object> lst = new List<object>();

            if (no != 0)
                lst.Add(no);
            lst.Add(Item.Item.Product.Id);
            lst.Add(Item.Item.Product.Name);
            lst.Add(Item.Item.Product.Category);

            lst.Add(Item.PreviousQuantity);
            lst.Add(Item.PreviousAmount.In.ToString(Constants.formatCurrency));

            string dateOutput = Item.PreviousDate.ToString(Constants.formatDate);
            if (dateOutput == Constants.dateNone)
                lst.Add("None");
            else
                lst.Add(dateOutput);

            lst.Add(Item.RecentQuantity);
            lst.Add(Item.RecentAmount.In.ToString(Constants.formatCurrency));
            lst.Add(Item.RecentDate.ToString(Constants.formatDate));
            
            lst.Add(Item.Item.Quantity);
            lst.Add(Item.Item.Price.In.ToString(Constants.formatCurrency));
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

            foreach (ProductInventoryStatus item in Items)
            {
                Item = item;

                // Get record (table row)
                List<object> records = GetRecords(no);

                Utilities.Output(records.ToArray(), lengths);

                no += (no != 0) ? 1 : 0;
            }
        }
        public void OutputBottom(bool displayNo = false)
        {
            string strOutput = string.Empty;
            List<int> fixLength = Utilities.GetFixLengths(GetLengths(displayNo), 0);

            strOutput = GetPreviousQuantity(Items).ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[4]);

            strOutput = GetRecentQuantity(Items).ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[7]);

            strOutput = GetTotalQuantity(Items).ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[10]);

            strOutput = GetPreviousPrice(Items).In.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[5]);

            strOutput = GetRecentPrice(Items).In.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[8]);

            strOutput = GetTotalPrice(Items).In.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[11]);

            Utilities.WriteLine();
        }
        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
            if (Items.Count > 1)
                OutputBottom(displayNo);
        }

        public double GetPreviousQuantity(List<ProductInventoryStatus> items)
        {
            double result = 0.0;
            foreach (var item in items)
            {
                result += item.PreviousQuantity;
            }
            return result;
        }
        public double GetRecentQuantity(List<ProductInventoryStatus> items)
        {
            double result = 0.0;
            foreach (var item in items)
            {
                result += item.RecentQuantity;
            }
            return result;
        }
        public double GetTotalQuantity(List<ProductInventoryStatus> items)
        {
            double result = 0.0;
            foreach (var item in items)
            {
                result += item.Item.Quantity;
            }
            return result;
        }

        public Price GetPreviousPrice(List<ProductInventoryStatus> items)
        {
            Price result = new Price(0, 0);
            foreach (var item in items)
            {
                result.In += item.PreviousAmount.In;
                result.Out += item.PreviousAmount.Out;
            }
            return result;
        }
        public Price GetRecentPrice(List<ProductInventoryStatus> items)
        {
            Price result = new Price(0, 0);
            foreach (var item in items)
            {
                result.In += item.RecentAmount.In;
                result.Out += item.RecentAmount.Out;
            }
            return result;
        }
        public Price GetTotalPrice(List<ProductInventoryStatus> items)
        {
            Price result = new Price(0, 0);
            foreach (var item in items)
            {
                result.In += item.Item.Price.In;
                result.Out += item.Item.Price.Out;
            }
            return result;
        }

        public void WriteItems(XmlNode parentNode, string childName)
        {
            foreach (ProductInventoryStatus item in Items)
            {
                Item = item;
                XmlNode newNode = DataProvider.Instance.createNode(childName);
                WriteItem(newNode);
                parentNode.AppendChild(newNode);
            }
        }
        public void WriteItem(XmlNode newNode)
        {
            XmlNode newAttr = null;

            newAttr = DataProvider.Instance.createNode("Product");
            newAttr.InnerText = Item.Item.Product.Id.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Previous");
            newAttr.InnerText = Item.PreviousQuantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("AmountInput");
            newAttr.InnerText = Item.PreviousAmount.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("AmountOutput");
            newAttr.InnerText = Item.PreviousAmount.Out.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Date");
            newAttr.InnerText = Item.PreviousDate.ToString(Constants.formatDateTime);
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Recent");
            newAttr.InnerText = Item.RecentQuantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("AmountInput");
            newAttr.InnerText = Item.RecentAmount.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("AmountOutput");
            newAttr.InnerText = Item.RecentAmount.Out.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Date");
            newAttr.InnerText = Item.RecentDate.ToString(Constants.formatDateTime);
            newNode.AppendChild(newAttr);
            
            newAttr = DataProvider.Instance.createNode("TotalQuantity");
            newAttr.InnerText = Item.Item.Quantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalPriceInput");
            newAttr.InnerText = Item.Item.Price.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalPriceOutput");
            newAttr.InnerText = Item.Item.Price.Out.ToString();
            newNode.AppendChild(newAttr);
        }
        public void WriteAll()
        {
            int count = DataProvider.Instance.nodeRoot.ChildNodes.Count;
            DataProvider.Instance.nodeRoot.Attributes["Count"].Value = count.ToString();
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fProductInventorysStatus);
            Utilities.RemoveAllChilds(DataProvider.Instance.nodeRoot);
            WriteItems(DataProvider.Instance.nodeRoot, "Products");

            count = DataProvider.Instance.nodeRoot.ChildNodes.Count;
            DataProvider.Instance.nodeRoot.Attributes["Count"].Value = count.ToString();
            DataProvider.Instance.Close();
        }
        public void WriteImport()
        {
            DataProvider.Instance.Open(Constants.fProductImportInventorysStatus);
            Utilities.RemoveAllChilds(DataProvider.Instance.nodeRoot);
            WriteItems(DataProvider.Instance.nodeRoot, "Products");
            int count = DataProvider.Instance.nodeRoot.ChildNodes.Count;
            DataProvider.Instance.nodeRoot.Attributes["Count"].Value = count.ToString();
            DataProvider.Instance.Close();
        }

        public List<ProductInventoryStatus> LoadItems(XmlNodeList lstNode)
        {
            Items = new List<ProductInventoryStatus>();
            foreach (XmlNode nodeData in lstNode)
            {
                Items.Add(LoadItem(nodeData));
            }
            return Items;
        }
        public ProductInventoryStatus LoadItem(XmlNode nodeData)
        {
            XmlNode nodeTemp = null;
            string strTemp = string.Empty;
            ProductInventoryStatus newItem = new ProductInventoryStatus();

            nodeTemp = nodeData.FirstChild;
            newItem.Item.Product.Id = nodeTemp.InnerText;

            ProductViewModel ProductViewModel = new ProductViewModel();
            ProductViewModel.Items = Products;

            var productItem = ProductViewModel.FindById(newItem.Item.Product.Id);
            if (productItem == null)
            {
                Utilities.CatchError();
            }
            newItem.Item.Product = productItem;

            nodeTemp = nodeTemp.NextSibling;
            newItem.PreviousQuantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.PreviousAmount.In = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.PreviousAmount.Out = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.PreviousDate = Convert.ToDateTime(nodeTemp.InnerText);


            nodeTemp = nodeTemp.NextSibling;
            newItem.RecentQuantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.RecentAmount.In = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.RecentAmount.Out = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.RecentDate = Convert.ToDateTime(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.Item.Quantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.Item.Price.In = Convert.ToDouble(nodeTemp.InnerText);
            nodeTemp = nodeTemp.NextSibling;
            newItem.Item.Price.Out = Convert.ToDouble(nodeTemp.InnerText);

            return newItem;
        }
        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fProductInventorysStatus);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }
        public void LoadImport()
        {
            DataProvider.Instance.Open(Constants.fProductImportInventorysStatus);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }

        public ProductInventoryStatus Find(Product findValue)
        {
            foreach (ProductInventoryStatus item in Items)
            {
                if (item.Item.Product.Id == findValue.Id)
                    return item;
            }
            return null;
        }
    }
}
