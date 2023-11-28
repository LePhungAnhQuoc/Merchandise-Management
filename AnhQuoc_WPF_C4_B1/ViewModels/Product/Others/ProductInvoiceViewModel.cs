using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInvoiceViewModel
    {
        public Inventory Inventory { get; set; }
        public ProductInvoice Item { get; set; }
        public List<ProductInvoice> Items { get; set; }
        public List<Product> Products { get; set; }

        public void Update(Invoice invoice)
        {
            foreach (InvoiceDetail detail in invoice.Details.ListDetail)
            {
                ProductInvoice item = Find(detail.Product);

                if (item == null)
                {
                    ProductInvoice newItem = new ProductInvoice();
                    newItem.Product = detail.Product;

                    newItem.PreviousQuantity = 0;
                    newItem.PreviousAmount = new Price(0, 0.0);
                    newItem.PreviousDate = Convert.ToDateTime(Constants.dateNone);

                    newItem.RecentQuantity = detail.Quantity;
                    newItem.RecentAmount = new Price();
                    newItem.RecentAmount.In = detail.TotalPrice.In;
                    newItem.RecentAmount.Out = detail.TotalPrice.Out;
                    newItem.RecentDate = detail.Date;

                    newItem.TotalQuantity = newItem.RecentQuantity;
                    newItem.TotalPrice = new Price();
                    newItem.TotalPrice.In = newItem.RecentAmount.In;
                    newItem.TotalPrice.Out = newItem.RecentAmount.Out;

                    Items.Add(newItem);
                }
                else
                {
                    ProductInvoice newItem = item;

                    newItem.PreviousQuantity += newItem.RecentQuantity;
                    newItem.PreviousAmount.In += newItem.RecentAmount.In;
                    newItem.PreviousAmount.Out += newItem.RecentAmount.Out;
                    newItem.PreviousDate = newItem.RecentDate;

                    newItem.RecentQuantity = detail.Quantity;
                    newItem.RecentAmount = new Price();
                    newItem.RecentAmount.In = detail.TotalPrice.In;
                    newItem.RecentAmount.Out = detail.TotalPrice.Out;
                    newItem.RecentDate = detail.Date;

                    newItem.TotalQuantity += newItem.RecentQuantity;
                    newItem.TotalPrice.In += newItem.RecentAmount.In;
                    newItem.TotalPrice.Out += newItem.RecentAmount.Out;
                }
            }
        }
        public void Update(List<Invoice> invoices)
        {
            foreach (Invoice item in invoices)
            {
                Update(item);
            }
        }

        public void Update(ProductInvoice productInvoice, OrderDetail orderDetail)
        {
            productInvoice.TotalQuantity -= orderDetail.Quantity;
            productInvoice.TotalPrice.In -= orderDetail.TempPrice.In;
            productInvoice.TotalPrice.Out -= orderDetail.TempPrice.Out;
        }
        public void Update(Order newOrder)
        {
            foreach (OrderDetail detail in newOrder.Details.ListDetail)
            {
                ProductInvoice newItem = Find(detail.Product);
                Update(newItem, detail);
            }
        }
        public void Update(List<Order> orders)
        {
            foreach (Order item in orders)
            {
                Update(item);
            }
        }

        public void Reset(Order newOrder)
        {
            foreach (OrderDetail detail in newOrder.Details.ListDetail)
            {
                Reset(detail);
            }
        }

        public void Reset(OrderDetail detail)
        {
            ProductInvoice newItem = Find(detail.Product);
            newItem.TotalQuantity += detail.Quantity;
            newItem.TotalPrice.In += detail.TempPrice.In;
            newItem.TotalPrice.Out += detail.TempPrice.Out;
        }

        public void WriteItems(XmlNode parentNode, string childName)
        {
            foreach (ProductInvoice item in Items)
            {
                Item = item;
                XmlNode newNode = DataProvider.Instance.createNode(childName);
                WriteItem(newNode);
                parentNode.AppendChild(newNode);
            }
            int count = parentNode.ChildNodes.Count;
            parentNode.Attributes["Count"].Value = count.ToString();
        }
        public void WriteItem(XmlNode newNode)
        {
            XmlNode newAttr = null;
            
            newAttr = DataProvider.Instance.createNode("Product");
            newAttr.InnerText = Item.Product.Id.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("PreviousQuantity");
            newAttr.InnerText = Item.PreviousQuantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("PreviousAmountInput");
            newAttr.InnerText = Item.PreviousAmount.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("PreviousAmountOutput");
            newAttr.InnerText = Item.PreviousAmount.Out.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("PreviousDate");
            newAttr.InnerText = Item.PreviousDate.ToString(Constants.formatDateTime);
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("RecentQuantity");
            newAttr.InnerText = Item.RecentQuantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("RecentAmountInput");
            newAttr.InnerText = Item.RecentAmount.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("RecentAmountOutput");
            newAttr.InnerText = Item.RecentAmount.Out.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("RecentDate");
            newAttr.InnerText = Item.RecentDate.ToString(Constants.formatDateTime);
            newNode.AppendChild(newAttr);
            
            newAttr = DataProvider.Instance.createNode("TotalQuantity");
            newAttr.InnerText = Item.TotalQuantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalAmountInput");
            newAttr.InnerText = Item.TotalPrice.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalAmountOutput");
            newAttr.InnerText = Item.TotalPrice.Out.ToString();
            newNode.AppendChild(newAttr);
        }
        public List<ProductInvoice> LoadItems(XmlNodeList lstNode)
        {
            Items = new List<ProductInvoice>();
            foreach (XmlNode nodeData in lstNode)
            {
                Items.Add(LoadItem(nodeData));
            }
            return Items;
        }
        public ProductInvoice LoadItem(XmlNode nodeData)
        {
            XmlNode nodeTemp = null;
            string strTemp = string.Empty;
            ProductInvoice newItem = new ProductInvoice();

            nodeTemp = nodeData.FirstChild;
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
            newItem.TotalQuantity = Convert.ToDouble(nodeTemp.InnerText);
            nodeTemp = nodeTemp.NextSibling;
            newItem.TotalPrice.In = Convert.ToDouble(nodeTemp.InnerText);
            nodeTemp = nodeTemp.NextSibling;
            newItem.TotalPrice.Out = Convert.ToDouble(nodeTemp.InnerText);
            return newItem;
        }

        public double GetPreviousQuantity(List<ProductInvoice> items)
        {
            double result = 0.0;
            foreach (var item in items)
            {
                result += item.PreviousQuantity;
            }
            return result;
        }
        public double GetRecentQuantity(List<ProductInvoice> items)
        {
            double result = 0.0;
            foreach (var item in items)
            {
                result += item.RecentQuantity;
            }
            return result;
        }
        public double GetTotalQuantity(List<ProductInvoice> items)
        {
            double result = 0.0;
            foreach (var item in items)
            {
                result += item.TotalQuantity;
            }
            return result;
        }

        public Price GetPreviousPrice(List<ProductInvoice> items)
        {
            Price result = new Price(0, 0);
            foreach (var item in items)
            {
                result.In += item.PreviousAmount.In;
                result.Out += item.PreviousAmount.Out;
            }
            return result;
        }
        public Price GetRecentPrice(List<ProductInvoice> items)
        {
            Price result = new Price(0, 0);
            foreach (var item in items)
            {
                result.In += item.RecentAmount.In;
                result.Out += item.RecentAmount.Out;
            }
            return result;
        }
        public Price GetTotalPrice(List<ProductInvoice> items)
        {
            Price result = new Price(0, 0);
            foreach (var item in items)
            {
                result.In += item.TotalPrice.In;
                result.Out += item.TotalPrice.Out;
            }
            return result;
        }

        public void WriteAll()
        {
            DataProvider.Instance.Open(Constants.fProductInvoices);
            Utilities.RemoveAllChilds(DataProvider.Instance.nodeRoot);
            WriteItems(DataProvider.Instance.nodeRoot, "Products");
            DataProvider.Instance.Close();
        }
        public void WritefProductInvoicesOrder()
        {
            DataProvider.Instance.Open(Constants.fProductInvoicesOrder);
            Utilities.RemoveAllChilds(DataProvider.Instance.nodeRoot);
            WriteItems(DataProvider.Instance.nodeRoot, "Products");
            DataProvider.Instance.Close();
        }
        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fProductInvoices);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }
        public void LoadfProductInvoicesOrder()
        {
            DataProvider.Instance.Open(Constants.fProductInvoicesOrder);
            Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }

        public List<ProductInvoice> FillByType(List<ProductInvoice> lst, ProductCategory type)
        {
            List<ProductInvoice> result = new List<ProductInvoice>();
            foreach (ProductInvoice item in lst)
            {
                if (item.Product.Category == type)
                    result.Add(item);
            }
            return result;
        }
        public List<List<ProductInvoice>> ConvertTo2D(List<ProductInvoice> lst)
        {
            return new List<List<ProductInvoice>>
            {
                FillByType(lst, ProductCategory.Electronic),
                FillByType(lst, ProductCategory.Porcelain),
                FillByType(lst, ProductCategory.Food),
            };
        }
        public List<ProductInvoice> GetListAvailable(List<ProductInvoice> source)
        {
            List<ProductInvoice> result = new List<ProductInvoice>();

            foreach (ProductInvoice item in source)
            {
                if (item.TotalQuantity > 0)
                    result.Add(item);
            }
            return result;
        }
        public List<List<ProductInvoice>> ConvertByCategory(List<List<ProductInvoice>> result)
        {
            result.Clear();
            result.Add(new List<ProductInvoice>());
            result.Add(new List<ProductInvoice>());
            result.Add(new List<ProductInvoice>());

            foreach (ProductInvoice item in Items)
            {
                int idx = (int)item.Product.Category;
                result[idx].Add(item);
            }
            return result;
        }
        public List<ProductInvoice> ConvertTo1D(List<List<ProductInvoice>> source)
        {
            Items.Clear();
            foreach (List<ProductInvoice> item in source)
            {
                Items.AddRange(item.ToArray());
            }
            return Items;
        }
        public List<Product> ConvertToList(List<ProductInvoice> source)
        {
            List<Product> lst = new List<Product>();
            foreach (ProductInvoice item in source)
            {
                Product newItem = item.Product;
                lst.Add(newItem);
            }
            return lst;
        }
        public List<List<Product>> ConvertToList2(List<List<ProductInvoice>> source)
        {
            List<List<Product>> lstResult = new List<List<Product>>();
            foreach (List<ProductInvoice> itemLst in source)
            {
                List<Product> line = new List<Product>();
                foreach (ProductInvoice item in itemLst)
                {
                    Product newItem = item.Product;
                    line.Add(newItem);
                }
                lstResult.Add(line);
            }
            return lstResult;
        }

        public List<List<ProductInvoice>> GetListAvailable(List<List<ProductInvoice>> collectionsInven)
        {
            List<ProductInvoice> collect1D = ConvertTo1D(collectionsInven);
            collect1D = GetListAvailable(collect1D);
            collectionsInven = ConvertTo2D(collect1D);
            return collectionsInven;
        }
        public int SelectCategory(bool isEsc)
        {
            MenuData menuData = new MenuData();
            int cateOption = 0;
            menuData.ProductCategory();
            menuData.Menu.Output(isEsc);

            Utilities.Write("\nSelect category: ");
            int startIndex = (isEsc) ? 0 : 1;
            Utilities.ReadLine(out cateOption, ">= && <=", startIndex, menuData.Menu.Items.Count);

            return cateOption;
        }
        public ProductInvoice Select(bool isEsc)
        {
            int startIndex = -1;
            int cateOption = -1;
            int productOption = -1;
            List<List<ProductInvoice>> ItemsByCat = ConvertTo2D(Items);

            Run:
            cateOption = SelectCategory(isEsc);
            if (cateOption == 0) return null;

            if (cateOption != 1)
            {
                cateOption--;
                Console.Clear();
                
                Items = ItemsByCat[cateOption - 1];

                if (Items.Count == 0)
                {
                    Utilities.NotifyAvailable("Product", "Invoice", true);
                    goto Run;
                }
                SelectProduct:
                OutputTableDetail(true);

                if (isEsc)
                {
                    Utilities.Write("\nPress [0] to escape..", ConsoleColor.DarkYellow);
                    startIndex = (isEsc) ? 0 : 1;
                }
                Utilities.Write("\nSelect product: ");
                Utilities.ReadLine(out productOption, ">= && <=", startIndex, Items.Count);

                if (productOption == 0)
                {
                    Console.Clear();
                    goto Run;
                }
                ProductInvoice result = Items[productOption - 1];
                if (result.TotalQuantity == 0)
                {
                    Utilities.NotifyAvailable("Product", "Invoice", false, ConsoleColor.Red);
                    Console.ReadKey(true);

                    Console.Clear();
                    goto SelectProduct;
                }
                return result;
            }
            else
            {
                Console.Clear();
                Items = ConvertTo1D(ItemsByCat);

                if (Items.Count == 0)
                {
                    Utilities.NotifyAvailable("Product", "Invoice", true);
                    goto Run;
                }
                SelectProduct2:
                OutputTableGeneral(true);

                if (isEsc)
                {
                    Utilities.Write("\nPress [0] to escape..", ConsoleColor.DarkYellow);
                    startIndex = (isEsc) ? 0 : 1;
                }
                Utilities.Write("\nSelect product: ");
                Utilities.ReadLine(out productOption, ">= && <=", startIndex, Items.Count);

                if (productOption == 0)
                {
                    Console.Clear();
                    goto Run;
                }
                ProductInvoice result = Items[productOption - 1];
                if (result.TotalQuantity == 0)
                {
                    Utilities.NotifyAvailable("Product", "Invoice", false, ConsoleColor.Red);
                    Console.ReadKey(true);

                    Console.Clear();
                    goto SelectProduct2;
                }
                return result;
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
            lst.Add("Amount");
            lst.Add("Date");

            lst.Add("Recent");
            lst.Add("Amount");
            lst.Add("Date");
            
            lst.Add("Total quantity");
            lst.Add("Total amount");
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
            string dateOutput;
            if (no != 0)
                lst.Add(no);
            lst.Add(Item.Product.Id);
            lst.Add(Item.Product.Name);
            lst.Add(Item.Product.Category);

            lst.Add(Item.PreviousQuantity);
            
            lst.Add(Item.PreviousAmount.Out.ToString(Constants.formatCurrency));

            dateOutput = Item.PreviousDate.ToString(Constants.formatDate);
            if (dateOutput == Constants.dateNone)
                lst.Add("None");
            else
                lst.Add(dateOutput);

            lst.Add(Item.RecentQuantity);
            lst.Add(Item.RecentAmount.Out.ToString(Constants.formatCurrency));

            dateOutput = Item.RecentDate.ToString(Constants.formatDate);
            if (dateOutput == Constants.dateNone)
                lst.Add("None");
            else
                lst.Add(dateOutput);
            
            lst.Add(Item.TotalQuantity);
            lst.Add(Item.TotalPrice.Out.ToString(Constants.formatCurrency));
            return lst;
        }

        public List<string> GetFieldsDetail(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");
            ProductViewModel productVM = new ProductViewModel();

            // Update code
            Item = Items[0];

            IProductViewModel IProductVM = productVM.AllocateVM(Item.Product.Category);
            lst.AddRange(IProductVM.GetFields(false));
            
            lst.Add("Quantity");
            lst.Add("Amount");
            return lst;
        }
        public List<int> GetLengthsDetail(bool displayNo = false)
        {
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);
            ProductViewModel productVM = new ProductViewModel();
            Item = Items[0];
            IProductViewModel IProductVM = productVM.AllocateVM(Item.Product.Category);
            lst.AddRange(IProductVM.GetLengths(false));
            
            lst.Add(15);
            lst.Add(15);
            return lst;
        }
        public List<object> GetRecordsDetail(int no = 0)
        {
            List<object> lst = new List<object>();
            if (no != 0)
                lst.Add(no);
            ProductViewModel productVM = new ProductViewModel();
            IProductViewModel IProductVM = productVM.AllocateVM(Item.Product.Category);
            productVM.Item = Item.Product;

            lst.AddRange(IProductVM.GetRecords(0, productVM.Item));
            
            lst.Add(Item.TotalQuantity);
            lst.Add(Item.TotalPrice.Out.ToString(Constants.formatCurrency));
            return lst;
        }

        public List<string> GetFieldsGeneral(bool displayNo = false)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");
            lst.AddRange(productVM.GetGeneralFields(false));

            lst.Add("Quantity");
            lst.Add("Amount");
            return lst;
        }
        public List<int> GetLengthsGeneral(bool displayNo = false)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);
            lst.AddRange(productVM.GetGeneralLengths(false));

            lst.Add(15);
            lst.Add(15);
            return lst;
        }
        public List<object> GetRecordsGeneral(int no = 0)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<object> lst = new List<object>();
            if (no != 0)
                lst.Add(no);
            productVM.Item = Item.Product;
            lst.AddRange(productVM.GetGeneralRecords(0));

            lst.Add(Item.TotalQuantity.ToString(Constants.formatThousand));
            lst.Add(Item.TotalPrice.Out.ToString(Constants.formatCurrency));
            return lst;
        }

        public void OutputTable(bool displayNo = false)
        {
            Coord currentPos = Utilities.GetCurrentCursorPosition();

            List<string> fields = GetFields(displayNo);
            List<int> lengths = GetLengths(displayNo);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.CursorLeft = currentPos.X;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            int no = (displayNo) ? 1 : 0;
            foreach (ProductInvoice item in Items)
            {
                Item = item;
                List<object> records = GetRecords(no);

                Console.CursorLeft = currentPos.X;
                Utilities.Output(records.ToArray(), lengths);

                no += (displayNo) ? 1 : 0;
            }
        }
        public void OutputTableGeneral(bool displayNo = false)
        {
            Coord currentPos = Utilities.GetCurrentCursorPosition();

            List<string> fields = GetFieldsGeneral(displayNo);
            List<int> lengths = GetLengthsGeneral(displayNo);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.CursorLeft = currentPos.X;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            int no = (displayNo) ? 1 : 0;
            foreach (ProductInvoice item in Items)
            {
                Item = item;
                List<object> records = GetRecordsGeneral(no);

                Console.CursorLeft = currentPos.X;
                Utilities.Output(records.ToArray(), lengths);

                no += (displayNo) ? 1 : 0;
            }
            List<int> getFixLength = Utilities.GetFixLengths(GetLengthsGeneral(displayNo), 0);
            SetColorGeneral(getFixLength, displayNo);
        }
        public void OutputTableDetail(bool displayNo = false)
        {
            Item = Items[0];
            Coord currentPos = Utilities.GetCurrentCursorPosition();

            List<string> fields = GetFieldsDetail(displayNo);
            List<int> lengths = GetLengthsDetail(displayNo);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.CursorLeft = currentPos.X;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            int no = (displayNo) ? 1 : 0;
            foreach (ProductInvoice item in Items)
            {
                Item = item;
                List<object> records = GetRecordsDetail(no);

                Console.CursorLeft = currentPos.X;
                Utilities.Output(records.ToArray(), lengths);

                no += (displayNo) ? 1 : 0;
            }
            List<int> getFixLength = Utilities.GetFixLengths(GetLengthsDetail(displayNo), 0);
            SetColorDetail(getFixLength, displayNo);
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

            strOutput = GetPreviousPrice(Items).Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[5]);

            strOutput = GetRecentPrice(Items).Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[8]);

            strOutput = GetTotalPrice(Items).Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[11]);

            Utilities.WriteLine();
        }
        public void OutputBottomGeneral(bool displayNo = false)
        {
            string strOutput = string.Empty;
            List<int> fixLength = Utilities.GetFixLengths(GetLengthsGeneral(displayNo), 0);
            
            strOutput = GetTotalQuantity(Items).ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[7]);
            
            strOutput = GetTotalPrice(Items).Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[8]);

            Utilities.WriteLine();
        }
        public void OutputAll(bool displayNo = false)
        {
            OutputTable(displayNo);
            if (Items.Count > 1)
                OutputBottom(displayNo);
        }
        public void OutputAllGeneral(bool displayNo = false)
        {
            OutputTableGeneral(displayNo);
            if (Items.Count > 1)
                OutputBottomGeneral(displayNo);
        }
        public void SetColorGeneral(List<int> fixLength, bool displayNo)
        {
            // Set color "Quantity"
            Console.CursorTop -= Items.Count;
            foreach (ProductInvoice item in Items)
            {
                if (item.TotalQuantity < Inventory.MinQuantity)
                {
                    List<string> fields = GetFieldsGeneral(displayNo);
                    int idxQuantity = fields.IndexOf("Quantity");
                    if (idxQuantity == -1)
                        return;
                    if (item.TotalQuantity == 0)
                        Utilities.WriteAt(item.TotalQuantity, ConsoleColor.Red, fixLength[idxQuantity]);
                    else
                        Utilities.WriteAt(item.TotalQuantity, ConsoleColor.Yellow, fixLength[idxQuantity]);
                }
                Console.CursorTop++;
            }
        }
        public void SetColorDetail(List<int> fixLength, bool displayNo)
        {
            // Set color "Quantity"
            Console.CursorTop -= Items.Count;
            foreach (ProductInvoice item in Items)
            {
                if (item.TotalQuantity < Inventory.MinQuantity)
                {
                    List<string> fields = GetFieldsDetail(displayNo);
                    int idxQuantity = fields.IndexOf("Quantity");
                    if (idxQuantity == -1)
                        return;
                    if (item.TotalQuantity == 0)
                        Utilities.WriteAt(item.TotalQuantity, ConsoleColor.Red, fixLength[idxQuantity]);
                    else
                        Utilities.WriteAt(item.TotalQuantity, ConsoleColor.Yellow, fixLength[idxQuantity]);
                }
                Console.CursorTop++;
            }
        }

        public ProductInvoice Find(Product findValue)
        {
            foreach (ProductInvoice item in Items)
            {
                if (item.Product.Id == findValue.Id)
                    return item;
            }
            return null;
        }
        
        public List<ProductInvoice> GetOutOf()
        {
            List<ProductInvoice> result = new List<ProductInvoice>();
            foreach (ProductInvoice item in Items)
            {
                if (item.TotalQuantity == 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<ProductInvoice> GetInOf()
        {
            List<ProductInvoice> result = new List<ProductInvoice>();
            foreach (ProductInvoice item in Items)
            {
                if (item.TotalQuantity > 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<ProductInvoice> GetAlmostOutOf(double minQuantity)
        {
            List<ProductInvoice> result = new List<ProductInvoice>();
            foreach (ProductInvoice item in Items)
            {
                if (item.TotalQuantity > 0)
                {
                    if (item.TotalQuantity < minQuantity)
                        result.Add(item);
                }
            }
            return result;
        }

        public bool IsEmpty()
        {
            if (Items.Count == 0)
                return true;
            foreach (ProductInvoice item in Items)
            {
                if (item.TotalQuantity > 0)
                    return false;
            }
            return true;
        }

        public Price GetTotalSoldSingle()
        {
            Price result = new Price(0, 0);

            result.In = Item.TotalPrice.In;
            result.Out = Item.TotalPrice.Out;

            return result;
        }

        // 2 Phuong thuc dang phat trien
        public double GetRevenueSingle(Price totalSold)
        {
            return totalSold.Out;
        }

        public double GetProfitSingle(Price totalSold)
        {
            return totalSold.Out - totalSold.In;
        }

        public List<ProductInvoice> GetRevenueFromOrders(List<Order> orders)
        {
            List<ProductInvoice> result = new List<ProductInvoice>();
            foreach (Order order in orders)
            {
                foreach (OrderDetail detailOrder in order.Details.ListDetail)
                {
                    Items = result;
                    ProductInvoice itemFinded = Find(detailOrder.Product);
                    if (itemFinded == null)
                    {
                        ProductInvoice newItem = new ProductInvoice();
                        newItem.Product = detailOrder.Product;

                        // Số lượng bán được
                        newItem.TotalQuantity = detailOrder.Quantity;

                        // Doanh thu bán được
                        newItem.TotalPrice = new Price();
                        newItem.TotalPrice.In = detailOrder.TotalPrice.In;
                        newItem.TotalPrice.Out = detailOrder.TotalPrice.Out;

                        result.Add(newItem);
                    }
                    else
                    {
                        itemFinded.TotalQuantity += detailOrder.Quantity;
                        itemFinded.TotalPrice.In += detailOrder.TotalPrice.In;
                        itemFinded.TotalPrice.Out += detailOrder.TotalPrice.Out;
                    }
                }
            }
            return result;
        }
    }
}
