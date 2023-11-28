using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class InvoiceViewModel
    {
        public Invoice Item { get; set; }
        public List<Invoice> Items { get; set; }
        public List<Product> Products { get; set; }
        
        public XmlNode WriteItems(XmlNode parentNode)
        {
            foreach (Invoice item in Items)
            {
                Item = item;
                XmlNode newNode = DataProvider.Instance.createNode("Invoice");
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

        public void WriteItemList(XmlNode parentNode, InvoiceDetailList detailList)
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
            InvoiceDetailViewModel detailVM = new InvoiceDetailViewModel();
            foreach (Invoice invoice in Items)
            {
                Item = invoice;

                #region GetInvoiceNode
                XmlNode invoiceNode = DataProvider.Instance.createNode("Invoice");
                XmlAttribute newAttr = DataProvider.Instance.createAttr("Id");
				newAttr.Value = Item.Id;
                invoiceNode.Attributes.Append(newAttr);
				#endregion
                
                XmlNode invoiceDetailListNode = DataProvider.Instance.createNode("InvoiceDetailList");
                WriteItemList(invoiceDetailListNode, invoice.Details);

                XmlNode detailsNode = DataProvider.Instance.FindNodeByNodeName(invoiceDetailListNode, "Details");
                detailVM.ItemList = invoice.Details;
                detailVM.WriteItems(detailsNode, "Detail");

                invoiceNode.AppendChild(invoiceDetailListNode);
                grandPaNode.AppendChild(invoiceNode);
            }
            int count = grandPaNode.ChildNodes.Count;
            grandPaNode.Attributes["Count"].Value = count.ToString();
            return grandPaNode;
        }

        public List<Invoice> LoadItems(XmlNodeList lstNode)
        {
            Items = new List<Invoice>();
            foreach (XmlNode nodeData in lstNode)
            {
                Items.Add(LoadItem(nodeData));
            }
            return Items;
        }

        public Invoice LoadItem(XmlNode nodeData)
        {
            XmlNode nodeTemp = nodeData.FirstChild;
            Invoice newItem = new Invoice();
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

        public void LoadItemList(XmlNode nodeData, InvoiceDetailList list)
        {
            InvoiceDetailViewModel detailVM = new InvoiceDetailViewModel();
            detailVM.Products = Products;

            XmlNode nodeTemp = null;

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
            InvoiceDetailViewModel detailVM = new InvoiceDetailViewModel();
            detailVM.Products = Products;
            foreach (Invoice item in Items)
            {
                string xpath = $"Invoice[@Id = '{item.Id}']";
                XmlNode InvoiceNode = DataProvider.Instance.nodeRoot.SelectSingleNode(xpath);
                XmlNode InvoiceDetailListNode = DataProvider.Instance.FindNodeByNodeName(InvoiceNode, "InvoiceDetailList");

                LoadItemList(InvoiceDetailListNode, item.Details);
            }
        }

        public void WriteAll()
        {
            XmlNode parentNode = null;
            
            DataProvider.Instance.Open(Constants.fInvoices);
            parentNode = WriteItems(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fInvoiceDetails);
            parentNode = WriteDetail(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fInvoices);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fInvoiceDetails);
            LoadDetails();
            DataProvider.Instance.Close();
        }

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

            foreach (Invoice item in Items)
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

        public double GetTotalQuantity(List<Invoice> invoices)
        {
            double result = 0.0;
            foreach (var item in invoices)
            {
                result += item.TotalQuantity;
            }
            return result;
        }

        public Price GetTotalPrice(List<Invoice> invoices)
        {
            Price result = new Price(0, 0);
            foreach (var item in invoices)
            {
                result.In += item.TotalPrice.In;
                result.Out += item.TotalPrice.Out;
            }
            return result;
        }

        public string GetId(int no)
        {
            return "PXK" + no.ToString();
        }

        public Invoice FindById(string id, bool ignoreCase = true)
        {
            foreach (Invoice item in Items)
            {
                if (string.Compare(item.Id, id, ignoreCase) == 0)
                    return item;
            }
            return null;
        }
    }
}
