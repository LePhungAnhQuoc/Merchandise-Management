using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class Inventory
    {
        public List<Receipt> Receipts { get; set; }
        public List<Invoice> Invoices { get; set; }
        public List<Order> Orders { get; set; }

        public List<ProductInventoryStatus> ProductsStatus { get; set; }
        public List<ProductInventory> Products { get; set; }

        public List<ProductInvoice> ProductInvoices { get; set; }
        public List<ProductInvoice> ProductInvoicesOrder { get; set; }

        public double TotalQuantity { get; set; }
        public Price TotalPrice { get; set; }

        public double MinQuantity { get; set; }

        public Inventory()
        {
            Receipts = new List<Receipt>();
            Invoices = new List<Invoice>();
            Orders = new List<Order>();

            Products = new List<ProductInventory>();
            ProductInvoices = new List<ProductInvoice>();
            ProductsStatus = new List<ProductInventoryStatus>();

            TotalPrice = new Price();
            TotalQuantity = 0.0;
        }
    }
}
