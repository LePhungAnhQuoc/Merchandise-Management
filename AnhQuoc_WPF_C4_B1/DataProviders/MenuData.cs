using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    class MenuData
    {
        public Menu Menu { get; set; }

        public MenuData()
        {
            Menu = new Menu();
        }

        public void Stocker()
        {
            Menu.Items.Clear();
            Menu.Items.Add("View products information");
            Menu.Items.Add("Add new receipt");
            Menu.Items.Add("View list receipt");

            Menu.Items.Add("Add new invoice");
            Menu.Items.Add("View list invoice");

            Menu.Items.Add("View Inventory");
            Menu.Items.Add("View Import Inventory (Receipts)");
            Menu.Items.Add("View Export Inventory (Invoices)");

            Menu.Items.Add("Expire date products");
            Menu.Items.Add("Out of inventory products");
            Menu.Items.Add("Almost out of inventory products");
            Menu.Items.Add("In of inventory products");

            Menu.Items.Add("Update price input");
        }
        public void Cashier()
        {
            Menu.Items.Clear();
            Menu.Items.Add("Add new order");
            Menu.Items.Add("View list order");
            Menu.Items.Add("View list order by date");

            Menu.Items.Add("Register new customer");
            Menu.Items.Add("View list customer");

            Menu.Items.Add("Statistical in invoice");
        }
        public void ProductCategory()
        {
            Menu.Items.Clear();
            Menu.Items.Add("All products");
            Menu.Items.Add("Electronics");
            Menu.Items.Add("Porcelains");
            Menu.Items.Add("Food");
        }
        public void OutputByDate()
        {
            Menu.Items.Clear();
            Menu.Items.Add("Today");
            Menu.Items.Add("Yesterday");
            Menu.Items.Add("Input date");
        }
        public void CashierStatistical()
        {
            Menu.Items.Clear();
            Menu.Items.Add("Display products out of invoice");
            Menu.Items.Add("Display products almost out of invoice");
            Menu.Items.Add("Display products in of invoice");

            Menu.Items.Add("View revenue & profit");
            Menu.Items.Add("View revenue & profit by date");
        }
    }
}
