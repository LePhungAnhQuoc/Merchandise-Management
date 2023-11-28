using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class OrderDetailViewModel
    {
        public OrderDetail Item;
        public List<OrderDetail> Items { get { return ItemList.ListDetail; } }
        public OrderDetailList ItemList = new OrderDetailList();
        public List<Product> Products { get; set; }

        public void SettingPercentDiscount(OrderDetail item)
        {
            switch (item.Product.Category)
            {
                case ProductCategory.Electronic:
                    item.PercentDiscount = 15;
                    break;
                case ProductCategory.Porcelain:
                    item.PercentDiscount = 30;
                    break;
                case ProductCategory.Food:
                    item.PercentDiscount = 25;
                    break;
            }
        }
        
        public XmlNode WriteItems(XmlNode parentNode, string nodeName)
        {
            foreach (OrderDetail item in Items)
            {
                XmlNode newNode = DataProvider.Instance.createNode(nodeName);
                WriteItem(newNode, item);
                parentNode.AppendChild(newNode);
            }
            return parentNode;
        }

        public XmlNode WriteItem(XmlNode newNode, OrderDetail item)
        {
            Item = item;
            XmlNode newAttr = null;

            newAttr = DataProvider.Instance.createNode("Id");
            newAttr.InnerText = item.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("IdInvoice");
            newAttr.InnerText = item.IdOrder;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Product");
            newAttr.InnerText = item.Product.Id;
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Quantity");
            newAttr.InnerText = item.Quantity.ToString();
            newNode.AppendChild(newAttr);
            
            newAttr = DataProvider.Instance.createNode("TempPriceInput");
            newAttr.InnerText = item.TempPrice.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TempPriceOutput");
            newAttr.InnerText = item.TempPrice.Out.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("PercentDiscount");
            newAttr.InnerText = item.PercentDiscount.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("DiscountPrice");
            newAttr.InnerText = item.DiscountPrice.ToString();
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

        public List<OrderDetail> LoadItems(XmlNodeList lstNode)
        {
            List<OrderDetail> items = new List<OrderDetail>();
            foreach (XmlNode nodeData in lstNode)
            {
                items.Add(LoadItem(nodeData));
            }
            return items;
        }

        public OrderDetail LoadItem(XmlNode nodeData)
        {
            Item = new OrderDetail();
            XmlNode nodeTemp = nodeData.FirstChild;
            Item.Id = nodeTemp.InnerText;

            nodeTemp = nodeTemp.NextSibling;
            Item.IdOrder = nodeTemp.InnerText;
            
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
            Item.TempPrice.In = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            Item.TempPrice.Out = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            Item.PercentDiscount = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            Item.DiscountPrice = Convert.ToDouble(nodeTemp.InnerText);

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
            lst.Add("TempPrice");

            // Update
            lst.Add("PercentDiscount");
            lst.Add("Discount");
            lst.Add("TotalPrice");

            lst.Add("Date");

            // Update code
            try { lst.RemoveAt(lst.IndexOf("Price Input")); }
            catch (Exception ex)
            {
                Utilities.WriteLine(ex);
                Console.ReadKey();
            }

            try {
                int idxRemove = lst.IndexOf("Price Output");
                lst.RemoveAt(idxRemove);
                lst.Insert(idxRemove, "Price");
            }
            catch (Exception ex)
            {
                Utilities.WriteLine(ex);
                Console.ReadKey();
            }
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

            lst.Add(15);
            lst.Add(15);
            lst.Add(15);

            lst.Add(20);
            // Update code
            try { lst.RemoveAt(5); }
            catch (Exception ex)
            {
                Utilities.WriteLine(ex);
                Console.ReadKey();
            }
            return lst;
        }

        public List<object> GetRecords(int no)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<object> lst = new List<object>();

            if (no != 0)
                lst.Add(no);

            // Lay cac truong cua Product
            productVM.Item = Item.Product;
            List<object> lstTemp = productVM.GetGeneralRecords(0);
            lst.AddRange(lstTemp.ToArray());

            // Lay them cac truong bo xung
            lst.Add(Item.Quantity.ToString(Constants.formatThousand));
            lst.Add(Item.TempPrice.Out.ToString(Constants.formatCurrency));

            // Update
            lst.Add(Item.PercentDiscount.ToString(Constants.formatDiscount));
            lst.Add(Item.DiscountPrice.ToString(Constants.formatCurrency));
            lst.Add(Item.TotalPrice.Out.ToString(Constants.formatCurrency));

            lst.Add(Item.Date.ToString(Constants.formatDateTime));
           
            // Update code
            try {
                int idxRemove = lst.IndexOf(Item.Product.Price.In.ToString(Constants.formatCurrency));
                lst.RemoveAt(idxRemove);
            }
            catch (Exception ex)
            {
                Utilities.WriteLine(ex);
                Console.ReadKey();
            }
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

            foreach (OrderDetail item in Items)
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
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[6]);

            strOutput = ItemList.TotalPrice.Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[10]);

            Utilities.WriteLine();
        }

        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
            if (Items.Count > 1)
                OutputBottom(displayNo);
        }

        public string GetId(int no) => no.ToString();

        public double GetTotalQuantity(List<OrderDetail> details)
        {
            double result = 0.0;
            foreach (var item in details)
            {
                result += item.Quantity;
            }
            return result;
        }

        public Price GetTotalPrice(List<OrderDetail> details)
        {
            Price result = new Price(0, 0);
            foreach (var item in details)
            {
                result.In += item.TempPrice.In;
                result.Out += item.TempPrice.Out;
            }
            return result;
        }

        public void GetListInfo()
        {
            // Update Items
            ItemList.TotalQuantity = GetTotalQuantity(ItemList.ListDetail);
            Price price = GetTotalPrice(ItemList.ListDetail);
            ItemList.TotalPrice.In = price.In;
            ItemList.TotalPrice.Out = price.Out;
        }

        public void GetTotalPriceSingle(List<OrderDetail> details)
        {
            foreach (OrderDetail detail in details)
            {
                GetTotalPriceSingle(detail);
            }
        }

        public void GetTotalPriceSingle(OrderDetail detail)
        {
            detail.TempPrice.In = detail.Quantity * detail.Product.Price.In;
            detail.TempPrice.Out = detail.Quantity * detail.Product.Price.Out;

            SettingPercentDiscount(detail);

            detail.DiscountPrice = detail.TempPrice.Out * detail.PercentDiscount / 100;

            detail.TotalPrice.In = detail.TempPrice.In;
            detail.TotalPrice.Out = detail.TempPrice.Out - detail.DiscountPrice;
        }


        public OrderDetail FindByIdProduct(string idProduct)
        {
            foreach (OrderDetail detail in Items)
            {
                if (detail.Product.Id == idProduct)
                {
                    return detail;
                }
            }
            return null;
        }
    }
}
