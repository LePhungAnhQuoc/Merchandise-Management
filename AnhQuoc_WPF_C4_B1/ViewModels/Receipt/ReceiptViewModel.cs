using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ReceiptViewModel
    {
        public Receipt Item { get; set; }
        public List<Receipt> Items { get; set; }
        public List<Product> Products { get; set; }

        public List<int> fixLength;

        public List<string> GetFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");
            lst.Add("Id");
            lst.Add("UserCreated");
            lst.Add("TotalQuantity");
            lst.Add("TotalPriceOutput");
            lst.Add("Date");
            return lst;
        }
        public List<int> GetLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);
            lst.Add(3);
            lst.Add(15);
            lst.Add(15);
            lst.Add(20);
            lst.Add(25);
            return lst;
        }
        public List<object> GetRecords(int no = 0)
        {
            List<object> lst = new List<object>();

            if (no != 0)
                lst.Add(no);
            lst.Add(Item.Id);
            lst.Add(Item.User.Name);
            lst.Add(Item.TotalQuantity.ToString(Constants.formatThousand));
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

            foreach (Receipt item in Items)
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
            fixLength = Utilities.GetFixLengths(GetLengths(displayNo), 0);
            strOutput = GetTotalQuantity(Items).ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[3]);
            strOutput = GetTotalPrice(Items).Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[4]);

            Utilities.WriteLine();
        }
        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
            if (Items.Count > 1)
                OutputBottom(displayNo);
        }

        public XmlNode WriteItems(XmlNode parentNode)
        {
            foreach (Receipt item in Items)
            {
                Item = item;
                XmlNode newNode = DataProvider.Instance.createNode("Receipt");
                parentNode.AppendChild(WriteItem(newNode));
            }
            int count = parentNode.ChildNodes.Count;
            parentNode.Attributes["Count"].Value = count.ToString();
            return parentNode;
        }
        public XmlNode WriteItem(XmlNode newNode)
        {
            XmlNode newAttr = null;

            newAttr = DataProvider.Instance.createNode("Id");
            newAttr.InnerText = Item.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("UserCreated");
            newAttr.InnerText = Item.User.Name;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalQuantity");
            newAttr.InnerText = Item.TotalQuantity.ToString();
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

        public void WriteItemList(XmlNode parentNode, ReceiptDetailList detailList)
        {
            XmlNode newNode = null;
            newNode = DataProvider.Instance.createNode("TotalQuantity");
            newNode.InnerText = detailList.TotalQuantity.ToString();
            parentNode.AppendChild(newNode);

            newNode = DataProvider.Instance.createNode("TotalPriceIn");
            newNode.InnerText = detailList.TotalPrice.In.ToString();
            parentNode.AppendChild(newNode);

            newNode = DataProvider.Instance.createNode("TotalPriceOut");
            newNode.InnerText = detailList.TotalPrice.Out.ToString();
            parentNode.AppendChild(newNode);

            newNode = DataProvider.Instance.createNode("Details");
            parentNode.AppendChild(newNode);
        }
        public XmlNode WriteDetail(XmlNode grandPaNode)
        {
            ReceiptDetailViewModel detailVM = new ReceiptDetailViewModel();
            foreach (Receipt receipt in Items)
            {
                Item = receipt;
                
				#region GetReceiptNode
				XmlNode receiptNode = DataProvider.Instance.createNode("Receipt");
			    XmlAttribute newAttr = DataProvider.Instance.createAttr("Id");
				newAttr.Value = Item.Id;
                receiptNode.Attributes.Append(newAttr);
				#endregion
                
                XmlNode receiptDetailListNode = DataProvider.Instance.createNode("ReceiptDetailList");
                WriteItemList(receiptDetailListNode, receipt.Details);

                XmlNode detailsNode = DataProvider.Instance.FindNodeByNodeName(receiptDetailListNode, "Details");
                detailVM.ItemList = receipt.Details;
                detailVM.WriteItems(detailsNode, "Detail");

                receiptNode.AppendChild(receiptDetailListNode);
                grandPaNode.AppendChild(receiptNode);
            }
            int count = grandPaNode.ChildNodes.Count;
            grandPaNode.Attributes["Count"].Value = count.ToString();
            return grandPaNode;
        }

        public List<Receipt> LoadItems(XmlNodeList lstNode)
        {
            Items = new List<Receipt>();
            foreach (XmlNode nodeData in lstNode)
            {
                Items.Add(LoadItem(nodeData));
            }
            return Items;
        }
        public Receipt LoadItem(XmlNode nodeData)
        {
            XmlNode nodeTemp = nodeData.FirstChild;
            Receipt newItem = new Receipt();
            newItem.Id = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            newItem.User.Name = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            newItem.TotalQuantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.TotalPrice.In = Convert.ToDouble(nodeTemp.InnerText);
            nodeTemp = nodeTemp.NextSibling;
            newItem.TotalPrice.Out = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.Date = Convert.ToDateTime(nodeTemp.InnerText);
            return newItem;
        }

        public void LoadItemList(XmlNode nodeData, ReceiptDetailList list)
        {
            XmlNode nodeTemp = null;
            ReceiptDetailViewModel detailVM = new ReceiptDetailViewModel();
            detailVM.Products = Products;
            
            nodeTemp = nodeData.FirstChild;
            list.TotalQuantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            list.TotalPrice.In = Convert.ToDouble(nodeTemp.InnerText);
            nodeTemp = nodeTemp.NextSibling;
            list.TotalPrice.Out = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            list.ListDetail = detailVM.LoadItems(nodeTemp.ChildNodes);
        }
        public void LoadDetails()
        {
            foreach (Receipt item in Items)
            {
                string xpath = $"Receipt[@Id = '{item.Id}']";
                XmlNode receiptNode = DataProvider.Instance.nodeRoot.SelectSingleNode(xpath);
                XmlNode receiptDetailListNode = DataProvider.Instance.FindNodeByNodeName(receiptNode, "ReceiptDetailList");

                LoadItemList(receiptDetailListNode, item.Details);
            }
        }

        public void WriteAll()
        {
            XmlNode parentNode = null;
            
            DataProvider.Instance.Open(Constants.fReceipts);
            parentNode = WriteItems(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fReceiptDetails);
            parentNode = WriteDetail(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }
        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fReceipts);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fReceiptDetails);
            LoadDetails();
            DataProvider.Instance.Close();
        }

        public double GetTotalQuantity(List<Receipt> receipts)
        {
            double result = 0.0;
            foreach (var item in receipts)
            {
                result += item.TotalQuantity;
            }
            return result;
        }
        public Price GetTotalPrice(List<Receipt> receipts)
        {
            Price result = new Price(0, 0);
            foreach (var item in receipts)
            {
                result.In += item.TotalPrice.In;
                result.Out += item.TotalPrice.Out;
            }
            return result;
        }
        public Receipt FindById(string id, bool isIgnoreCase = true)
        {
            foreach (Receipt item in Items)
            {
                if (string.Compare(id, item.Id, isIgnoreCase) == 0)
                    return item;
            }
            return null;
        }
        public string GetId(int no)
        {
            string newId = string.Empty;
            newId = "PNK" + no.ToString();
            return newId;
        }
    }
}
