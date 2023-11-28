using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class Constants
    {
        #region FileXML
        public static string fReceipts = "Data/Receipt/Receipts.xml";
        public static string fReceiptDetails = "Data/Receipt/ReceiptDetails.xml";
        public static string fInvoices = "Data/Invoice/Invoices.xml";
        public static string fInvoiceDetails = "Data/Invoice/InvoiceDetails.xml";
        public static string fOrders = "Data/Order/Orders.xml";
        public static string fOrderDetails = "Data/Order/OrderDetails.xml";

        public static string fInventory = "Data/Inventory/Inventory.xml";
        public static string fImportInventory = "Data/Inventory/ImportInventory.xml";

        public static string fProducts = "Data/Product/Product/Products.xml";
        public static string fExpDateProducts = "Data/Product/Product/ExpDateProducts.xml";
        public static string fProductImportInventorysStatus = "Data/Product/ProductImportInventorysStatus.xml";
        public static string fProductInventorysStatus = "Data/Product/ProductInventorysStatus.xml";
        public static string fProductInventorys = "Data/Product/ProductInventorys.xml";
        public static string fProductInvoices = "Data/Product/ProductInvoices.xml";
        public static string fProductInvoicesOrder = "Data/Product/ProductInvoicesOrder.xml";

        public static string fAccounts = "Data/User/Accounts.xml";
        public static string fCustomers = "Data/Customer/Customers.xml";
        #endregion

        #region Unit
        public static string unitElectricPower = "kWh";
        public static string unitWarranty = "Months";
        public static string unitCurrency = "đ";
        #endregion
        
        #region FormatString
        public static string formatThousand = "#,##0.##";
        public static string formatWarranty = formatThousand + " " + unitWarranty;
        public static string formatElectricPower = formatThousand + " " + unitElectricPower;
        public static string formatDate = "dd/MM/yyyy";
        public static string formatDateFile = "dd-MM-yyyy";
        public static string formatDateTime = "dd/MM/yyyy HH:mm";
        public static string formatTime = "hh\\:mm";
        public static string formatTimeFile = "hh\\-mm";
        public static string formatCurrency = formatThousand + " " + unitCurrency;
        public static string formatDiscount = "0\\%";
        #endregion

        #region Others
        // xml xpath
        public static string xpathProduct = "Product";

        public static int bufferLength = 150;
        public static string dateNone = "01/01/0001";
        public static string strEsc = "0";
        public static string IDGuest = "000";
        #endregion
    }
}
