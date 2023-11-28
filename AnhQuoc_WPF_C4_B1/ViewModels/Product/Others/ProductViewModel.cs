using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductViewModel
    {
        public RepositoryBase<Product> Repo { get; set; }

        public Product Item { get; set; }
        public List<Product> Items { get; set; }
        public RepositoryBase<List<Product>> ItemsByCatRepo { get; set; }


        private const double PercentVAT = 10;
        private double PercentProfit = 30;
        private const double PercentEmployee = 1.2;

        public ProductViewModel()
        {
            Repo = new RepositoryBase<Product>();
        }

        public double CalculatePriceOutput(int numberOfEmps)
        {
            double priceInput = Item.Price.In;
            double VATFee = priceInput * PercentVAT / 100;
            double profitFee = priceInput * PercentProfit / 100;
            double employeeFee = priceInput * (numberOfEmps * PercentEmployee / 100);

            double totalFee = VATFee + profitFee + employeeFee;

            return priceInput + totalFee;
        }
        public void GetPriceOutput(int numberOfEmps)
        {
            foreach (Product item in Items)
            {
                Item = item;
                item.Price.Out = CalculatePriceOutput(numberOfEmps);
            }
        }
        public Product Allocate(ProductCategory type)
        {
            Product result = null;
            switch (type)
            {
                case ProductCategory.Electronic:
                    result = new Electronic();
                    break;
                case ProductCategory.Porcelain:
                    result = new Porcelain();
                    break;
                case ProductCategory.Food:
                    result = new Food();
                    break;
            }
            return result;
        }
        public IProductViewModel AllocateVM(ProductCategory type)
        {
            IProductViewModel IProductVM = null;
            switch (type)
            {
                case ProductCategory.Electronic:
                    IProductVM = new ElectronicViewModel();
                    break;
                case ProductCategory.Porcelain:
                    IProductVM = new PorcelainViewModel();
                    break;
                case ProductCategory.Food:
                    IProductVM = new FoodViewModel();
                    break;
            }
            return IProductVM;
        }

        public List<Product> ConvertTo1D()
        {
            Items = new List<Product>();
            foreach (List<Product> item in ItemsByCatRepo.Gets())
            {
                Items.AddRange(item.ToArray());
            }
            return Items;
        }
        public List<List<Product>> ConvertTo2D()
        {
            return new List<List<Product>>
            {
                FillByType(ProductCategory.Electronic),
                FillByType(ProductCategory.Porcelain),
                FillByType(ProductCategory.Food),
            };
        }

        public List<Product> ConvertTo(List<ProductInventory> source)
        {
            List<Product> lst = new List<Product>();
            foreach (var item in source)
            {
                lst.Add(item.Product);
            }
            return lst;
        }
        public List<List<Product>> ConvertTo(List<List<ProductInventory>> source)
        {
            List<List<Product>> result = new List<List<Product>>();

            foreach (List<ProductInventory> itemLst in source)
            {
                List<Product> newLst = new List<Product>();
                foreach (ProductInventory item in itemLst)
                {
                    newLst.Add(item.Product);
                }
                result.Add(newLst);
            }
            return result;
        }

        public List<string> GetGeneralFields(bool displayNo = false)
        {
            List<string> lst = new List<string>();
            if (displayNo)
                lst.Add("No.");
            lst.Add("Id");
            lst.Add("Name");
            lst.Add("Category");
            lst.Add("Producer");
            lst.Add("Price Input");
            lst.Add("Price Output");
            return lst;
        }
        public List<int> GetGeneralLengths(bool displayNo = false)
        {
            List<int> lst = new List<int>();

            if (displayNo)
                lst.Add(3);
            lst.Add(7);
            lst.Add(25);
            lst.Add(13);
            lst.Add(20);
            lst.Add(15);
            lst.Add(15);
            return lst;
        }
        public List<object> GetGeneralRecords(int no = 0)
        {
            List<object> lst = new List<object>();

            // Solve problem

            if (no != 0)
                lst.Add(no);
            lst.Add(Item.Id);
            lst.Add(Item.Name);
            lst.Add(Item.Category);
            lst.Add(Item.Producer);
            lst.Add(Item.Price.In.ToString(Constants.formatCurrency));
            lst.Add(Item.Price.Out.ToString(Constants.formatCurrency));
            return lst;
        }

        public void OutputTableDetail(bool displayNo = false)
        {
            int no = (displayNo) ? 1 : 0;

            Item = Items[0];
            IProductViewModel IProductVM = AllocateVM(Item.Category);

            List<string> fields = IProductVM.GetFields(displayNo);
            List<int> lengths = IProductVM.GetLengths(displayNo);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            foreach (Product item in Items)
            {
                Item = item;
                List<object> records = IProductVM.GetRecords(no, Item);
                Utilities.Output(records.ToArray(), lengths);

                no += (no != 0) ? 1 : 0;
            }
        }
        public void OutputTableGeneral(bool displayNo = false)
        {
            List<string> fields = GetGeneralFields(displayNo);
            List<int> lengths = GetGeneralLengths(displayNo);

            int no = (displayNo) ? 1 : 0;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utilities.Output(fields.ToArray(), lengths);
            Console.ResetColor();

            foreach (Product item in Items)
            {
                Item = item;
                List<object> records = GetGeneralRecords(no);
                Utilities.Output(records.ToArray(), lengths);

                no += (no != 0) ? 1 : 0;
            }
        }

        public void WriteItemsByCatAll(string fileName, List<ProductCategory> categories)
        {
            DataProvider.Instance.Open(fileName);
            XmlNode parentNode = DataProvider.Instance.nodeRoot;
            Utilities.RemoveAllChilds(parentNode);

            WriteItemsByCat(parentNode, categories);
            DataProvider.Instance.Close();
        }
        public void WriteItemsByCat(XmlNode nodeRoot, List<ProductCategory> categories)
        {
            int index = 0;

            // Nghiep vu can phat trien
            foreach (List<Product> lstItem in ItemsByCatRepo.Gets())
            {
                ProductCategory category = categories[index];
                XmlNode categoryNode = DataProvider.Instance.createNode(category.ToString());
                Items = lstItem;
                WriteItems(categoryNode, category);
                nodeRoot.AppendChild(categoryNode);
                index++;
            }
        }
        public void WriteItems(XmlNode parentNode, ProductCategory category)
        {
            foreach (Product item in Items)
            {
                IProductViewModel IProductVM = AllocateVM(category);
                XmlNode newNode = DataProvider.Instance.createNode("Product");

                Item = item;

                IProductVM.WriteItem(item, newNode);
                parentNode.AppendChild(newNode);
            }
        }
        public void WriteGeneral(Product item, XmlNode newNode)
        {
            XmlAttribute newAttr = null;

            newAttr = DataProvider.Instance.createAttr("Id");
            newAttr.Value = item.Id;
            newNode.Attributes.Append(newAttr);

            newAttr = DataProvider.Instance.createAttr("Name");
            newAttr.Value = item.Name;
            newNode.Attributes.Append(newAttr);

            newAttr = DataProvider.Instance.createAttr("Category");
            newAttr.Value = item.Category.ToString();
            newNode.Attributes.Append(newAttr);

            newAttr = DataProvider.Instance.createAttr("Producer");
            newAttr.Value = item.Producer;
            newNode.Attributes.Append(newAttr);

            newAttr = DataProvider.Instance.createAttr("PriceInput");
            newAttr.Value = item.Price.In.ToString();
            newNode.Attributes.Append(newAttr);

            newAttr = DataProvider.Instance.createAttr("PriceOutput");
            newAttr.Value = item.Price.Out.ToString();
            newNode.Attributes.Append(newAttr);
        }

        public RepositoryBase<List<Product>> LoadItems(XmlNodeList lstCategoryNodes)
        {
            ItemsByCatRepo = new RepositoryBase<List<Product>>();
            foreach (XmlNode catNode in lstCategoryNodes)
            {
                ProductCategory productCategory = (ProductCategory)Enum.Parse(typeof(ProductCategory), catNode.Name);
                List<Product> products = new List<Product>();

                IProductViewModel IProductVM = AllocateVM(productCategory);

                XmlNodeList productNodes = catNode.ChildNodes;
                foreach (XmlNode nodeItem in productNodes)
                {
                    products.Add(IProductVM.LoadItem(nodeItem));
                }
                ItemsByCatRepo.Add(products);
            }
            return ItemsByCatRepo;
        }
        public void LoadGeneral(Product item, XmlNode nodeData)
        {
            item.Id = nodeData.Attributes["Id"].Value;
            item.Name = nodeData.Attributes["Name"].Value;
            item.Producer = nodeData.Attributes["Producer"].Value;

            item.Price.In = Convert.ToDouble(nodeData.Attributes["PriceInput"].Value);
            item.Price.Out = Convert.ToDouble(nodeData.Attributes["PriceOutput"].Value);
        }
        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fProducts);
            ItemsByCatRepo = LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }

        public int SelectCategory(bool isEsc)
        {
            MenuData menuData = new MenuData();
            menuData.ProductCategory();
            menuData.Menu.Output(isEsc);

            Utilities.Write("\nSelect category: ");
            return menuData.Menu.Select(isEsc);
        }
        public List<Product> OutputListProducts(bool isEsc, bool isLoop, ref int cateOption)
        {
            List<Product> result = null;

            getProductCategory: // Chon  product cate
            cateOption = SelectCategory(true);
            if (cateOption == 0) return null;

            Console.Clear();

            // DS
            result = OutputProducts(isEsc, ref cateOption);
            Items = result;

            if (isLoop || result.Count == 0) // Loop until choose cateOption == 0
            {
                Console.ReadKey();
                Console.Clear();
                goto getProductCategory;
            }

            return result;
        }
        public List<Product> OutputProducts(bool isEsc, ref int cateOption)
        {
            List<Product> result = new List<Product>();
            if (cateOption != 1)
            {
                int tempCateOption = cateOption - 1;


                result = ItemsByCatRepo.GetByIndex(tempCateOption - 1);
                Items = result;
                if (Items.Count == 0)
                {
                    Utilities.NotifyAvailable("Product", "List", true);
                    // Noting
                    cateOption = -1;
                    return result;
                }
                //IProductViewModel viewModel = AllocateVM((ProductType)(tempCateOption - 1));
                OutputTableDetail(true);
            }
            else
            {
                result = ConvertTo1D();
                Items = result;
                if (Items.Count == 0)
                {
                    Utilities.NotifyAvailable("Product", "List", true);
                    cateOption = -1;
                    return result;
                }
                OutputTableGeneral(true);
            }
            if (isEsc)
            {
                Utilities.Write("\nPress [0] to escape..", ConsoleColor.DarkYellow);
            }
            return result;
        }
        public Product SelectProduct(bool isEsc)
        {
            int startIndex = -1;
            int productOption = -1;

            startIndex = (isEsc) ? 0 : 1;
            Utilities.Write("\nSelect product: ");
            Utilities.ReadLine(out productOption, ">= && <=", startIndex, Items.Count);

            if (isEsc && productOption == 0)
            {
                Console.Clear();
                return null;
            }
            return Items[productOption - 1];
        }

        public List<Product> FillByType(ProductCategory type)
        {
            List<Product> result = new List<Product>();
            foreach (Product item in Items)
            {
                if (item.Category == type)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public Product FindById(string id)
        {
            foreach (Product item in Items)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }
        public bool IsEmpty(List<List<Product>> lst)
        {
            foreach (var lstItem in lst)
            {
                if (lstItem.Count > 0)
                    return false;
            }
            return true;
        }

        public List<Product> FillExpDate()
        {
            List<Product> result = new List<Product>();
            foreach (Product item in Items)
            {
                if (item is Food)
                {
                    if (item.ExpDate.Date < DateTime.Now.Date)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public void OnPriceChangeUI(Inventory inventory, Inventory importInventory, List<Receipt> receipts, List<Invoice> invoices, List<Order> orders, 
            List<ProductInvoice> productInvoices, List<ProductInvoice> productInvoicesOrder)
        {
            foreach (Receipt receipt in receipts)
            {
                ReceiptDetailViewModel ReceiptDetailViewModel = new ReceiptDetailViewModel();
               
                // First: Get Single (of each item of list<detail>
                ReceiptDetailViewModel.GetTotalPriceSingle(receipt.Details.ListDetail);

                // Next: Get Total of all item in List<detail>
                ReceiptDetailViewModel.ItemList = receipt.Details;
                ReceiptDetailViewModel.GetListInfo();

                // Assign values
                receipt.TotalQuantity = ReceiptDetailViewModel.ItemList.TotalQuantity;
                receipt.TotalPrice.In = ReceiptDetailViewModel.ItemList.TotalPrice.In;
                receipt.TotalPrice.Out = ReceiptDetailViewModel.ItemList.TotalPrice.Out;
            }
            foreach (Invoice invoice in invoices)
            {
                InvoiceDetailViewModel invoiceDetailViewModel = new InvoiceDetailViewModel();
                invoiceDetailViewModel.GetTotalPriceSingle(invoice.Details.ListDetail);
                invoiceDetailViewModel.ItemList = invoice.Details;
                invoiceDetailViewModel.GetListInfo();
                invoice.TotalQuantity = invoiceDetailViewModel.ItemList.TotalQuantity;
                invoice.TotalPrice.In = invoiceDetailViewModel.ItemList.TotalPrice.In;
                invoice.TotalPrice.Out = invoiceDetailViewModel.ItemList.TotalPrice.Out;
            }
            foreach (Order order in orders)
            {
                OrderDetailViewModel orderDetailViewModel = new OrderDetailViewModel();
                orderDetailViewModel.GetTotalPriceSingle(order.Details.ListDetail);
                orderDetailViewModel.ItemList = order.Details;
                orderDetailViewModel.GetListInfo();

                // Assign Value to Order
                order.TotalQuantity = orderDetailViewModel.ItemList.TotalQuantity;
                order.TotalPrice.In = orderDetailViewModel.ItemList.TotalPrice.In;
                order.TotalPrice.Out = orderDetailViewModel.ItemList.TotalPrice.Out;
            }
            InventoryViewModel InventoryViewModel = new InventoryViewModel();

            InventoryViewModel.Item = inventory;
            inventory.Products.Clear();
            inventory.ProductsStatus.Clear();
            InventoryViewModel.Update(receipts);

            InventoryViewModel.Item = importInventory;
            importInventory.Products.Clear();
            importInventory.ProductsStatus.Clear();
            InventoryViewModel.UpdateImport(receipts);

            ProductInvoiceViewModel ProductInvoiceViewModel = new ProductInvoiceViewModel();

            ProductInvoiceViewModel.Items = productInvoices;
            productInvoices.Clear();
            ProductInvoiceViewModel.Update(invoices);

            ProductInvoiceViewModel.Items = productInvoicesOrder;
            productInvoicesOrder.Clear();
            ProductInvoiceViewModel.Update(invoices);
            ProductInvoiceViewModel.Update(orders);
        }

        public void OnPriceChangeDatabase(Inventory inventory, Inventory importInventory, List<Receipt> receipts, List<Invoice> invoices, List<Order> orders,
             List<ProductInvoice> productInvoices, List<ProductInvoice> productInvoicesOrder)
        {
            ReceiptViewModel ReceiptViewModel = new ReceiptViewModel();
            Utilities.ClearfReceipt();
            ReceiptViewModel.Items = receipts;
            ReceiptViewModel.WriteAll();

            InvoiceViewModel InvoiceViewModel = new InvoiceViewModel();
            Utilities.ClearfInvoice();
            InvoiceViewModel.Items = invoices;
            InvoiceViewModel.WriteAll();

            OrderViewModel OrderViewModel = new OrderViewModel();
            Utilities.ClearfOrder();
            OrderViewModel.Items = orders;
            OrderViewModel.WriteAll();

            InventoryViewModel InventoryViewModel = new InventoryViewModel();

            InventoryViewModel.Item = inventory;
            InventoryViewModel.WriteAll();

            InventoryViewModel.Item = importInventory;
            InventoryViewModel.WriteImportInventory();

            ProductInvoiceViewModel ProductInvoiceViewModel = new ProductInvoiceViewModel();

            ProductInvoiceViewModel.Items = productInvoices;
            ProductInvoiceViewModel.WriteAll();

            ProductInvoiceViewModel.Items = productInvoicesOrder;
            ProductInvoiceViewModel.WritefProductInvoicesOrder();
        }
    }
}
