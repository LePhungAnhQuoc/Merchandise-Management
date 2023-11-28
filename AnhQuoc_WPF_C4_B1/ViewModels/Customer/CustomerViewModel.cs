using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class CustomerViewModel
    {
        public Customer Item { get; set; }
        public List<Customer> Items { get; set; }

        public void GetPointAndCard(Customer Customer, double totalAmount)
        {
            Customer.Point += (totalAmount * OrderViewModel.PercentPoint / 100);
            Customer.Card = PointToCard.ToCard(Customer.Point);
        }

        public List<Customer> LoadItems(XmlNodeList lstNode)
        {
            List<Customer> result = new List<Customer>();
            foreach (XmlNode nodeData in lstNode)
            {
                result.Add(LoadItem(nodeData));
            }
            return result;
        }

        public Customer LoadItem(XmlNode nodeData)
        {
            XmlNode nodeTemp = nodeData.FirstChild;
            Customer newItem = new Customer();
            newItem.IDCard = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            newItem.Name = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            newItem.Phone = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            newItem.Point = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.Card = (CardType)Enum.Parse(typeof(CardType), nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.IsGuest = Convert.ToBoolean(Convert.ToInt32(nodeTemp.InnerText));

            return newItem;
        }

        public XmlNode WriteItems(XmlNode parentNode, string childName = "Customer")
        {
            foreach (Customer item in Items)
            {
                Item = item;
                XmlNode newNode = DataProvider.Instance.createNode(childName);
                parentNode.AppendChild(WriteItem(newNode));
            }
            int count = parentNode.ChildNodes.Count;
            parentNode.Attributes["Count"].Value = count.ToString();
            return parentNode;
        }

        public XmlNode WriteItem(XmlNode newNode)
        {
            XmlNode newAttr = null;

            newAttr = DataProvider.Instance.createNode("IDCard");
            newAttr.InnerText = Item.IDCard;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Name");
            newAttr.InnerText = Item.Name;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Phone");
            newAttr.InnerText = Item.Phone;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Point");
            newAttr.InnerText = Item.Point.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Card");
            newAttr.InnerText = Item.Card.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("IsGuest");
            newAttr.InnerText = Convert.ToInt32(Item.IsGuest).ToString();
            newNode.AppendChild(newAttr);

            return newNode;
        }

        public void WriteAll()
        {
            DataProvider.Instance.Open(Constants.fCustomers);
            WriteItems(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }
        
        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fCustomers);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }

        public List<string> GetFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");
            lst.Add("IDCard");
            lst.Add("Name");
            lst.Add("Phone");
            lst.Add("Point");
            lst.Add("Card");
            return lst;
        }

        public List<int> GetLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);
            lst.Add(10);
            lst.Add(20);
            lst.Add(15);
            lst.Add(15);
            lst.Add(15);
            return lst;
        }

        public List<object> GetRecords(int no = 0)
        {
            List<object> lst = new List<object>();

            if (no != 0)
                lst.Add(no);
            lst.Add(Item.IDCard);
            lst.Add(Item.Name);
            lst.Add(Item.Phone);
            lst.Add(Item.Point.ToString(Constants.formatThousand));
            lst.Add(Item.Card);
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

            foreach (Customer item in Items)
            {
                Item = item;
                List<object> records = GetRecords(no);
                Utilities.Output(records.ToArray(), lengths);

                no += (no != 0) ? 1 : 0;
            }
            Console.CursorTop -= Items.Count;
            OutputColor(displayNo);
        }

        private ConsoleColor GetColorByCard(CardType card)
        {
            switch (card)
            {
                case CardType.Gold:
                    return ConsoleColor.Yellow;
                case CardType.Platinum:
                    return ConsoleColor.White;
                case CardType.None:
                    return ConsoleColor.Black;
            }
            return ConsoleColor.Black;
        }

        public void OutputColor(bool displayNo)
        {
            List<int> lengths = GetLengths(displayNo);
            List<int> fixLengths = Utilities.GetFixLengths(lengths, 0);

            foreach (Customer item in Items)
            {
                ConsoleColor textColor = GetColorByCard(item.Card);
                Utilities.WriteLineAt(item.Card, textColor, fixLengths[5]);
            }
        }

        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
        }

        public Customer FindByIDCard(string IDCardValue)
        {
            foreach (Customer item in Items)
            {
                if (item.IDCard == IDCardValue)
                    return item;
            }
            return null;
        }

        public Customer CreateGuest(int no)
        {
            Customer newCus = new Customer();
            newCus.Name = $"Guest {no}";
            newCus.Phone = "None";
            newCus.IDCard = "None";
            newCus.Point = 0.0;
            newCus.Card = CardType.None;
            newCus.IsGuest = true;
            return newCus;
        }

        public List<Customer> FillGuest()
        {
            List<Customer> guests = new List<Customer>();
            foreach (Customer item in Items)
            {
                if (item.IsGuest == true)
                {
                    guests.Add(item);
                }
            }
            return guests;
        }

        public List<Customer> FillCustomer()
        {
            List<Customer> guests = new List<Customer>();
            foreach (Customer item in Items)
            {
                if (item.IsGuest == false)
                {
                    guests.Add(item);
                }
            }
            return guests;
        }
    }
}
