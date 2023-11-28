using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class OrderViewModel
    {
        public Order Item { get; set; }
        public List<Order> Items { get; set; }
        public List<Product> Products { get; set; }

        public const double PercentPoint = 0.1; // 0.001

        public string GetId(int no) => "PBH" + no.ToString();
        
        public XmlNode WriteItems(XmlNode parentNode)
        {
            foreach (Order item in Items)
            {
                Item = item;
                XmlNode newNode = DataProvider.Instance.createNode("Order");
                parentNode.AppendChild(WriteItem(newNode));
            }
            int count = parentNode.ChildNodes.Count;
            parentNode.Attributes["Count"].Value = count.ToString();

            return parentNode;
        }

        public XmlNode WriteItem(XmlNode newNode)
        {
            CustomerViewModel customerVM = new CustomerViewModel();
            XmlNode newAttr = null;

            newAttr = DataProvider.Instance.createNode("Id");
            newAttr.InnerText = Item.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("UserCreated");
            newAttr.InnerText = Item.UserCreated.Name;
            newNode.AppendChild(newAttr);
            
            newAttr = DataProvider.Instance.createNode("Customer");

            // Get Data
            customerVM.Item = Item.Customer;

            customerVM.WriteItem(newAttr);
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

        public void WriteItemList(XmlNode parentNode, OrderDetailList detailList)
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
            OrderDetailViewModel detailVM = new OrderDetailViewModel();

            foreach (Order order in Items)
            {
                Item = order;

                #region GetOrderNode
                XmlNode orderNode = DataProvider.Instance.createNode("Order");
                XmlAttribute newAttr = DataProvider.Instance.createAttr("Id");
				newAttr.Value = Item.Id;
                orderNode.Attributes.Append(newAttr);
				#endregion

                XmlNode orderDetailListNode = DataProvider.Instance.createNode("OrderDetailList");
                WriteItemList(orderDetailListNode, order.Details);

                XmlNode detailsNode = DataProvider.Instance.FindNodeByNodeName(orderDetailListNode, "Details");
                detailVM.ItemList = order.Details;
                detailVM.WriteItems(detailsNode, "Detail");

                orderNode.AppendChild(orderDetailListNode);
                grandPaNode.AppendChild(orderNode);
            }
            int count = grandPaNode.ChildNodes.Count;
            grandPaNode.Attributes["Count"].Value = count.ToString();

            return grandPaNode;
        }

        public List<Order> LoadItems(XmlNodeList lstNode)
        {
            Items = new List<Order>();
            foreach (XmlNode nodeData in lstNode)
            {
                Items.Add(LoadItem(nodeData));
            }
            return Items;
        }

        public Order LoadItem(XmlNode nodeData)
        {
            CustomerViewModel customerVM = new CustomerViewModel();
            Order newItem = new Order();
			
			XmlNode nodeTemp = nodeData.FirstChild;
            newItem.Id = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            newItem.UserCreated.Name = nodeTemp.InnerText;

			nodeTemp = nodeTemp.NextSibling;
            newItem.Customer = customerVM.LoadItem(nodeTemp);

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

        public void LoadItemList(XmlNode nodeData, OrderDetailList list)
        {
            OrderDetailViewModel detailVM = new OrderDetailViewModel();
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
            OrderDetailViewModel detailVM = new OrderDetailViewModel();
            detailVM.Products = Products;
            foreach (Order item in Items)
            {
                string xpath = $"Order[@Id = '{item.Id}']";
                XmlNode OrderNode = DataProvider.Instance.nodeRoot.SelectSingleNode(xpath);
                XmlNode OrderDetailListNode = DataProvider.Instance.FindNodeByNodeName(OrderNode, "OrderDetailList");

                LoadItemList(OrderDetailListNode, item.Details);
            }
        }

        public void WriteAll()
        {
            XmlNode parentNode = null;
            
            DataProvider.Instance.Open(Constants.fOrders);
            parentNode = WriteItems(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fOrderDetails);
            parentNode = WriteDetail(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fOrders);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fOrderDetails);
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
            lst.Add("Customer");
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
            lst.Add(Item.UserCreated.Name);
            lst.Add(Item.Customer.Name);
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

            foreach (Order item in Items)
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
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[4]);
            strOutput = GetTotalPrice(Items).Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[5]);

            Utilities.WriteLine();
        }

        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
            if (Items.Count > 1)
                OutputBottom(displayNo);
        }

        public double GetTotalQuantity(List<Order> orders)
        {
            double result = 0.0;
            foreach (var item in orders)
            {
                result += item.TotalQuantity;
            }
            return result;
        }

        public Price GetTotalPrice(List<Order> orders)
        {
            Price result = new Price(0, 0);
            foreach (var item in orders)
            {
                result.In += item.TotalPrice.In;
                result.Out += item.TotalPrice.Out;
            }
            return result;
        }

        public Order Find(string id)
        {
            foreach (Order item in Items)
            {
                if (item.Id == id)
                    return item;
            }
            return null;
        }

        public Order FindById(string id, bool ignoreCase = true)
        {
            foreach (Order item in Items)
            {
                if (string.Compare(item.Id, id, ignoreCase) == 0)
                    return item;
            }
            return null;
        }


        public List<Order> FillByDate(DateTime dateValue)
        {
            List<Order> result = new List<Order>();
            foreach (Order item in Items)
            {
                if (item.Date.Date == dateValue.Date)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public Price GetTotalSold()
        {
            Price result = new Price(0, 0);
            foreach (Order item in Items)
            {
                result.In += item.TotalPrice.In;
                result.Out += item.TotalPrice.Out;
            }
            return result;
        }

        // 2 Phuong thuc dang phat trien
        public double GetRevenue(Price totalSold)
        {
            return totalSold.Out;
        }

        public double GetProfit(Price totalSold)
        {
            return totalSold.Out - totalSold.In;
        }
    }
}
