using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class UnitOfWork
    {
        private RepositoryBase<AccountInfo> accounts;
        private RepositoryBase<List<Product>> products;
        private RepositoryBase<ProductInventory> productInventorys;
        private RepositoryBase<Customer> customers;


        private RepositoryBase<Receipt> receipts;
        private RepositoryBase<Invoice> invoices;
        private RepositoryBase<Order> orders;


        private Inventory inventory;
        private Inventory importInventory;

        public RepositoryBase<AccountInfo> GetRepositoryAccounts
        {
            get
            {
                if (this.accounts == null)
                    this.accounts = new RepositoryBase<AccountInfo>();
                return accounts;
            }
        }

        public RepositoryBase<List<Product>> GetRepositoryProducts
        {
            get
            {
                if (this.products == null)
                    this.products = new RepositoryBase<List<Product>>();
                return products;
            }
        }

        public RepositoryBase<Customer> GetRepositoryCustomers
        {
            get
            {
                if (this.customers == null)
                    this.customers = new RepositoryBase<Customer>();
                return customers;
            }
        }

        public RepositoryBase<Receipt> GetRepositoryReceipts
        {
            get
            {
                if (this.receipts == null)
                    this.receipts = new RepositoryBase<Receipt>();
                return receipts;
            }
        }

        public RepositoryBase<Invoice> GetRepositoryInvoices
        {
            get
            {
                if (this.invoices == null)
                    this.invoices = new RepositoryBase<Invoice>();
                return invoices;
            }
        }

        public RepositoryBase<Order> GetRepositoryOrders
        {
            get
            {
                if (this.orders == null)
                    this.orders = new RepositoryBase<Order>();
                return orders;
            }
        }

        public RepositoryBase<ProductInventory> GetRepositoryProductInventorys
        {
            get
            {
                if (this.productInventorys == null)
                    this.productInventorys = new RepositoryBase<ProductInventory>();
                return productInventorys;
            }
        }

        public Inventory GetInventory
        {
            get
            {
                if (this.inventory == null)
                    this.inventory = new Inventory();
                return inventory;
            }
        }

        public Inventory GetImportInventory
        {
            get
            {
                if (this.importInventory == null)
                    this.importInventory = new Inventory();
                return importInventory;
            }
        }

        public UnitOfWork()
        {
            inventory = new Inventory();
            importInventory = new Inventory();

            AccountViewModel accountVM = new AccountViewModel();
            accountVM.LoadAll();
            accounts = accountVM.Repo;

            ProductViewModel productVM = new ProductViewModel();
            productVM.LoadAll();
            products = productVM.ItemsByCatRepo;

            ProductViewModel ProductViewModel = new ProductViewModel();
            ProductViewModel.ItemsByCatRepo = products;
            List<Product> productItems = ProductViewModel.ConvertTo1D();

            ReceiptViewModel receiptVM = new ReceiptViewModel();
            receiptVM.Products = productItems;

            receiptVM.LoadAll();
            receipts = new RepositoryBase<Receipt>(receiptVM.Items);

            InvoiceViewModel InvoiceVM = new InvoiceViewModel();
            InvoiceVM.Products = productItems;
            InvoiceVM.LoadAll();
            invoices = new RepositoryBase<Invoice>(InvoiceVM.Items);

            OrderViewModel OrderVM = new OrderViewModel();
            OrderVM.Products = productItems;

            OrderVM.LoadAll();
            orders = new RepositoryBase<Order>(OrderVM.Items);

            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();
            ProductInvoiceVM.Products = productItems;

            ProductInvoiceVM.LoadAll();
            inventory.ProductInvoices = ProductInvoiceVM.Items;

            ProductInvoiceVM.LoadfProductInvoicesOrder();
            inventory.ProductInvoicesOrder = ProductInvoiceVM.Items;
           
            InventoryViewModel InventoryViewModel = new InventoryViewModel();
            InventoryViewModel.Products = productItems;
            InventoryViewModel.Item = inventory;
            InventoryViewModel.LoadAll();

            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            productInventoryVM.Products = productItems;

            productInventoryVM.LoadAll();
            productInventorys = new RepositoryBase<ProductInventory>(productInventoryVM.ItemList.Items);

            ProductInventoryStatusViewModel ProductInventoryStatusVM = new ProductInventoryStatusViewModel();
            ProductInventoryStatusVM.Products = productItems;

            ProductInventoryStatusVM.LoadAll();
            inventory.ProductsStatus = ProductInventoryStatusVM.Items;

            InventoryViewModel.Item = importInventory;
            InventoryViewModel.LoadImportInventory();

            inventory.Receipts = receipts.Gets();
            inventory.Invoices = invoices.Gets();
            inventory.Orders = orders.Gets();
            inventory.Products = productInventorys.Gets();
            
            CustomerViewModel CustomerVM = new CustomerViewModel();
            CustomerVM.LoadAll();
            customers = new RepositoryBase<Customer>(CustomerVM.Items);
        }
    }
}
