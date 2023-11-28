using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInventoryViewModel
    {
        #region Properties
        public Inventory Inventory { get; set; }
        public ProductInventory Item { get; set; }
        public List<ProductInventory> Items { get { return ItemList.Items; } }

        public List<List<ProductInventory>> ItemsByCat { get; set; }
        public List<Product> Products { get; set; }
        #endregion

        #region Fields
        public ProductInventoryList ItemList = new ProductInventoryList();
        #endregion
      
        public List<ProductInventory> ConvertTo1D()
        {
            List<ProductInventory> items = new List<ProductInventory>();
            foreach (List<ProductInventory> item in ItemsByCat)
            {
                items.AddRange(item.ToArray());
            }
            return items;
        }
        public List<List<ProductInventory>> ConvertTo2D()
        {
            return new List<List<ProductInventory>>
            {
                FillByType(ProductCategory.Electronic),
                FillByType(ProductCategory.Porcelain),
                FillByType(ProductCategory.Food),
            };
        }
        public List<ProductInventory> ConvertTo(List<ProductInventoryStatus> source)
        {
            List<ProductInventory> result = new List<ProductInventory>();
            foreach (ProductInventoryStatus item in source)
            {
                result.Add(item.Item);
            }
            return result;
        }
        
        public List<ProductInventory> GetListAvailable()
        {
            List<ProductInventory> result = new List<ProductInventory>();
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity > 0)
                    result.Add(item);
            }
            return result;
        }

        public void WriteItems(XmlNode parentNode, string childName)
        {
            foreach (ProductInventory item in Items)
            {
                Item = item;
                XmlNode newNode = DataProvider.Instance.createNode(childName);
                WriteItem(newNode);
                parentNode.AppendChild(newNode);
            }
            int count = parentNode.ChildNodes.Count;
            parentNode.Attributes["Count"].Value = count.ToString();
            DataProvider.Instance.Close();
        }
        public void WriteItem(XmlNode newNode)
        {
            XmlNode newAttr = null;

            newAttr = DataProvider.Instance.createNode("Product");
            newAttr.InnerText = Item.Product.Id.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("Quantity");
            newAttr.InnerText = Item.Quantity.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalPriceInput");
            newAttr.InnerText = Item.Price.In.ToString();
            newNode.AppendChild(newAttr);

            newAttr = DataProvider.Instance.createNode("TotalPriceOutput");
            newAttr.InnerText = Item.Price.Out.ToString();
            newNode.AppendChild(newAttr);
        }

        public List<ProductInventory> LoadItems(XmlNodeList lstNode)
        {
            List<ProductInventory> items = new List<ProductInventory>();
            foreach (XmlNode nodeData in lstNode)
            {
                items.Add(LoadItem(nodeData));
            }
            return items;
        }
        public ProductInventory LoadItem(XmlNode nodeData)
        {
            XmlNode nodeTemp = null;
            string strTemp = string.Empty;
			
            ProductInventory newItem = new ProductInventory();

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
            newItem.Quantity = Convert.ToDouble(nodeTemp.InnerText);

            nodeTemp = nodeTemp.NextSibling;
            newItem.Price.In = Convert.ToDouble(nodeTemp.InnerText);
            nodeTemp = nodeTemp.NextSibling;
            newItem.Price.Out = Convert.ToDouble(nodeTemp.InnerText);

            return newItem;
        }

        public void WriteAll()
        {
            DataProvider.Instance.Open(Constants.fProductInventorys);
            Utilities.RemoveAllChilds(DataProvider.Instance.nodeRoot);
            WriteItems(DataProvider.Instance.nodeRoot, "Products");
        }
        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fProductInventorys);
            ItemList.Items = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }
        public void LoadImport(XmlNodeList lstNodeData)
        {
            ItemList.Items = LoadItems(lstNodeData);
        }

        public double GetTotalQuantity(List<ProductInventory> items)
        {
            double result = 0.0;
            foreach (var item in items)
            {
                result += item.Quantity;
            }
            return result;
        }
        public Price GetTotalPrice(List<ProductInventory> items)
        {
            Price result = new Price(0, 0);
            foreach (var item in items)
            {
                result.In += item.Price.In;
                result.Out += item.Price.Out;
            }
            return result;
        }

        public List<string> GetGeneralFields(bool displayNo = false)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");

            lst.AddRange(productVM.GetGeneralFields(false));

            lst.Add("Quantity");
            lst.Add("TotalPriceInput");
            lst.Add("TotalPriceOutput");
            lst.Add("Note");

            return lst;
        }
        public List<int> GetGeneralLengths(bool displayNo = false)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<int> lst = new List<int>();
            if (displayNo)
                lst.Add(3);

            lst.AddRange(productVM.GetGeneralLengths(false));
            lst.Add(8);
            lst.Add(17);
            lst.Add(17);
            lst.Add(15);
            return lst;
        }
        public List<object> GetGeneralRecords(ProductInventory item, int no = 0)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<object> lst = new List<object>();
            if (no != 0)
                lst.Add(no);
            productVM.Item = Item.Product;
            lst.AddRange(productVM.GetGeneralRecords(0));

            lst.Add(item.Quantity.ToString(Constants.formatThousand));
            lst.Add(item.Price.In.ToString(Constants.formatCurrency));
            lst.Add(item.Price.Out.ToString(Constants.formatCurrency));
            lst.Add(string.Empty);
            return lst;
        }

        public List<string> GetFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            ProductViewModel productVM = new ProductViewModel();

            // Update code
            Item = Items[0];

            IProductViewModel IProductVM = productVM.AllocateVM(Item.Product.Category);
            List<string> tmpLst = IProductVM.GetFields(false);

            if (displayNo)
                lst.Add("No.");
            lst.AddRange(tmpLst);

            lst.Add("Quantity");
            lst.Add("PriceOutput");
            lst.Add("Note");
            return lst;
        }
        public List<int> GetLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();
            ProductViewModel productVM = new ProductViewModel();
            Item = Items[0];
            IProductViewModel IProductVM = productVM.AllocateVM(Item.Product.Category);
            List<int> tmpLst = IProductVM.GetLengths(false);
            if (displayNo)
                lst.Add(3);

            lst.AddRange(tmpLst);

            lst.Add(8);
            lst.Add(15);
            lst.Add(15);
            return lst;
        }
        public List<object> GetRecords(ProductInventory item, int no = 0)
        {
            List<object> lst = new List<object>();
            ProductViewModel productVM = new ProductViewModel();
            IProductViewModel IProductVM = productVM.AllocateVM(item.Product.Category);
            productVM.Item = item.Product;

            List<object> tmpLst = IProductVM.GetRecords(0, productVM.Item);

            if (no != 0)
                lst.Add(no);
            lst.AddRange(tmpLst);

            lst.Add(item.Quantity.ToString(Constants.formatThousand));
            lst.Add(item.Price.Out.ToString(Constants.formatCurrency));
            lst.Add(string.Empty);
            return lst;
        }

        public void OutputTableGeneral(bool displayNo = false)
        {
            List<string> fields = GetGeneralFields(displayNo);
            List<int> lengths = GetGeneralLengths(displayNo);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            int no = (displayNo) ? 1 : 0;
            foreach (ProductInventory item in Items)
            {
                Item = item;
                List<object> records = GetGeneralRecords(item, no);
                //Console.CursorLeft = currentPos.X;
                Utilities.Output(records.ToArray(), lengths);

                no += (no > 0) ? 1 : 0;
            }
            List<int> getFixLength = Utilities.GetFixLengths(GetGeneralLengths(displayNo), 0);
            SetColorGeneral(getFixLength, displayNo);
        }
        public void OutputTableDetail(bool displayNo = false)
        {
            Item = Items[0];
            Coord currentPos = Utilities.GetCurrentCursorPosition();

            List<string> fields = GetFields(displayNo);
            List<int> lengths = GetLengths(displayNo);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.CursorLeft = currentPos.X;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            int no = (displayNo) ? 1 : 0;
            foreach (ProductInventory item in Items)
            {
                Item = item;
                List<object> records = GetRecords(item, no);

                Console.CursorLeft = currentPos.X;
                Utilities.Output(records.ToArray(), lengths);

                no += (displayNo) ? 1 : 0;
            }
            List<int> getFixLength = Utilities.GetFixLengths(GetLengths(displayNo), 0);
            SetColorDetail(getFixLength, displayNo);
        }
        public void OutputBottomDetail(bool displayNo = false)
        {
            string strOutput = string.Empty;
            List<int> fixLength = Utilities.GetFixLengths(GetLengths(displayNo), 0);

            strOutput = GetTotalQuantity(Items).ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[4]);

            strOutput = GetTotalPrice(Items).Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[5]);

            Utilities.WriteLine();
        }
        public void OutputBottomGeneral(bool displayNo = false)
        {
            string strOutput = string.Empty;
            List<int> fixLength = Utilities.GetFixLengths(GetGeneralLengths(displayNo), 0);
            Price totalPrice = GetTotalPrice(Items);

            strOutput = GetTotalQuantity(Items).ToString(Constants.formatThousand);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[7]);

            strOutput = totalPrice.In.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[8]);

            strOutput = totalPrice.Out.ToString(Constants.formatCurrency);
            Utilities.WriteAt($"{strOutput}", ConsoleColor.Green, fixLength[9]);

            Utilities.WriteLine();
        }
        public void OutputAllDetail(bool displayNo = false)
        {
            OutputTableDetail(displayNo);
            if (Items.Count > 1)
                OutputBottomDetail(displayNo);
        }
        public void OutputAllGeneral(bool displayNo = false)
        {
            OutputTableGeneral(displayNo);
            if (Items.Count > 1)
                OutputBottomGeneral(displayNo);
        }

        public void SetColorGeneral(List<int> fixLength, bool displayNo)
        {

            #region SetColor
            Console.CursorTop -= Items.Count;
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity < Inventory.MinQuantity)
                {
                    List<string> fields = GetGeneralFields(displayNo);
                    int idxQuantity = fields.IndexOf("Quantity");
                    if (idxQuantity == -1)
                        return;
                    if (item.Quantity == 0)
                        Utilities.WriteAt(item.Quantity, ConsoleColor.Red, fixLength[idxQuantity]);
                    else
                        Utilities.WriteAt(item.Quantity, ConsoleColor.Yellow, fixLength[idxQuantity]);
                }
                Console.CursorTop++;
            }
            #endregion

            #region SetNote
            Console.CursorTop -= Items.Count;
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity < Inventory.MinQuantity)
                {
                    List<string> fields = GetGeneralFields(displayNo);
                    int idxQuantity = fields.IndexOf("Note");
                    if (idxQuantity == -1)
                        return;
                    if (item.Quantity == 0)
                        Utilities.WriteAt("Sold out!", ConsoleColor.Red, fixLength[idxQuantity]);
                    else
                        Utilities.WriteAt("Almost sold out!", ConsoleColor.Yellow, fixLength[idxQuantity]);
                }
                Console.CursorTop++;
            }
            #endregion

        }
        public void SetColorDetail(List<int> fixLength, bool displayNo)
        {
            // Set color "Quantity"
            Console.CursorTop -= Items.Count;
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity < Inventory.MinQuantity)
                {
                    List<string> fields = GetFields(displayNo);
                    int idxQuantity = fields.IndexOf("Quantity");
                    if (idxQuantity == -1)
                        return;
                    if (item.Quantity == 0)
                        Utilities.WriteAt(item.Quantity, ConsoleColor.Red, fixLength[idxQuantity]);
                    else
                        Utilities.WriteAt(item.Quantity, ConsoleColor.Yellow, fixLength[idxQuantity]);
                }
                Console.CursorTop++;
            }
            // Set color "Note"
            Console.CursorTop -= Items.Count;
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity < Inventory.MinQuantity)
                {
                    List<string> fields = GetFields(displayNo);
                    int idxQuantity = fields.IndexOf("Note");

                    if (idxQuantity == -1)
                        return;
                    if (item.Quantity == 0)
                        Utilities.WriteAt("Sold out!", ConsoleColor.Red, fixLength[idxQuantity]);
                    else
                        Utilities.WriteAt("Almost sold out!", ConsoleColor.Yellow, fixLength[idxQuantity]);
                }
                Console.CursorTop++;
            }
        }

        public int SelectCategory(bool isEsc)
        {
            MenuData menuData = new MenuData();
            menuData.ProductCategory();
            menuData.Menu.Output(isEsc);

            Utilities.Write("\nSelect category: ");
            return menuData.Menu.Select(isEsc);
        }
        public ProductInventory Select(bool isEsc)
        {
            int startIndex = -1;
            int cateOption = -1;
            int productOption = -1;

            List<ProductInventory> Items = null;
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
                    Utilities.NotifyAvailable("Product", "Inventory", true);
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
                ProductInventory result = Items[productOption - 1];
                if (result.Quantity == 0)
                {
                    Utilities.NotifyAvailable("Product", "Inventory", false, ConsoleColor.Red);
                    Console.ReadKey(true);

                    Console.Clear();
                    goto SelectProduct;
                }
                return result;
            }
            else
            {
                Console.Clear();
                Items = ConvertTo1D();

                if (Items.Count == 0)
                {
                    Utilities.NotifyAvailable("Product", "Inventory", true);
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
                ProductInventory result = Items[productOption - 1];
                if (result.Quantity == 0)
                {
                    Utilities.NotifyAvailable("Product", "Inventory", false, ConsoleColor.Red);
                    Console.ReadKey(true);

                    Console.Clear();
                    goto SelectProduct2;
                }
                return result;
            }
        }

        public ProductInventory Find(Product findValue)
        {
            foreach (ProductInventory item in Items)
            {
                if (item.Product.Id == findValue.Id)
                    return item;
            }
            return null;
        }
        public List<ProductInventory> Fill(List<Product> source)
        {
            List<ProductInventory> result = new List<ProductInventory>();
            foreach (ProductInventory item in Items)
            {
                foreach (Product srcItem in source)
                {
                    if (srcItem.Id == item.Product.Id)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
        public List<ProductInventory> FillByType(ProductCategory type)
        {
            ProductViewModel productVM = new ProductViewModel();
            List<ProductInventory> result = null;

            productVM.Items = productVM.ConvertTo(Items);
            productVM.Items = productVM.FillByType(type);

            result = Fill(productVM.Items);
            return result;
        }

        public List<ProductInventory> FillOutOfInventory()
        {
            List<ProductInventory> result = new List<ProductInventory>();
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity == 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<ProductInventory> FillInOfInventory()
        {
            List<ProductInventory> result = new List<ProductInventory>();
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity > 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<ProductInventory> FillAlmostOutOfInventory(double minQuantity)
        {
            List<ProductInventory> result = new List<ProductInventory>();
            foreach (ProductInventory item in Items)
            {
                if (item.Quantity > 0)
                {
                    if (item.Quantity < minQuantity)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
    }
}
