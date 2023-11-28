using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmCashier.xaml
    /// </summary>
    public partial class frmCashier : Window
    {
        #region Delegates
        public Func<MainWindow> getFrmMain;
        public Func<frmLogin> getFrmLogin;
        public Func<ucPage> getUcPage;
        #endregion

        #region Properties
        public Account Account
        {
            get { return (Account)GetValue(AccountProperty); }
            set { SetValue(AccountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Account.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccountProperty =
            DependencyProperty.Register("Account", typeof(Account), typeof(frmCashier), new UIPropertyMetadata(new Account()));
        #endregion

        #region Methods
        public frmCashier()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += FrmCashier_Loaded;
            //btnLogOut.Click += BtnLogOut_Click;
        }

        private void FrmCashier_Loaded(object sender, RoutedEventArgs e)
        {
            Account = getFrmMain().getAccountInfo().Account;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region TreeViewItem

        private void tvOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmCashier = () => this;
            ucPage.Menus = new List<string>
            {
                "New Order", "View Orders", "Details of Order"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvCreateOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucCreateOrder ucCreateOrder = new ucCreateOrder();
            
            ucCreateOrder.getFrmCashier = () => this;
            ucCreateOrder.getAccount = getFrmMain().getAccountInfo;
            var list = getFrmMain().unitOfWork.GetInventory.ProductInvoicesOrder;
            ucCreateOrder.getProductInvoiceOrderRepo = () => new RepositoryBase<ProductInvoice>(list);
            ucCreateOrder.getCustomerRepo = () => getFrmMain().unitOfWork.GetRepositoryCustomers;
            ucCreateOrder.getOrderRepo = () => getFrmMain().unitOfWork.GetRepositoryOrders;

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucCreateOrder);
        }

        private void tvViewOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucOrderTable ucOrderTable = new ucOrderTable();
            ucOrderTable.getfrmCashier = () => this;
            ucOrderTable.getOrderRepo = () => getFrmMain().unitOfWork.GetRepositoryOrders;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucOrderTable);
        }

        private void tvOrderDetails_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucDetailsByIdOrder ucDetailsByIdOrder = new ucDetailsByIdOrder();
            ucDetailsByIdOrder.lblHeader = string.Empty;
            ucDetailsByIdOrder.getfrmCashier = () => this;
            ucDetailsByIdOrder.getOrderRepo = () => getFrmMain().unitOfWork.GetRepositoryOrders;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucDetailsByIdOrder);
        }


        private void tvCustomer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmCashier = () => this;

            ucPage.Menus = new List<string>
            {
                "Register Customer", "View Customers"
            };

            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvRegisterCustomer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Customer newCustomer = null;

            frmCreateCustomer frmCreateCustomer = new frmCreateCustomer();
            RepositoryBase<Customer> getCustomerRepo = getFrmMain().unitOfWork.GetRepositoryCustomers;
            frmCreateCustomer.getCustomerRepo = () => getCustomerRepo;
            frmCreateCustomer.ShowDialog();

            newCustomer = frmCreateCustomer.newItem;

            if (newCustomer != null)
            {
                // Them du lieu
                getCustomerRepo.Add(newCustomer);

                // Ghi tập tin
                CustomerViewModel CustomerVM = new CustomerViewModel();
                CustomerVM.Items = new List<Customer>() { newCustomer };
                CustomerVM.WriteAll();
            }            
        }

        private void tvViewCustomer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucCustomerTable ucCustomerTable = new ucCustomerTable();
            ucCustomerTable.getCustomerRepo = () => getFrmMain().unitOfWork.GetRepositoryCustomers;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucCustomerTable);
        }

        
        private void tvShowInvoice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInvoiceGeneral ucListProductInvoiceGeneral = new ucListProductInvoiceGeneral();
        
            // Lay du lieu tu invoice & order
            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();
            ProductInvoiceVM.Items = getFrmMain().unitOfWork.GetInventory.ProductInvoicesOrder;

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            var getList = ProductByCategoryViewModel.ConvertProductInvoices(ProductInvoiceVM.Items);

            ucListProductInvoiceGeneral.getProductInvoiceByCatRepo = () => new RepositoryBase<ProductInvoiceByCategory>(getList);

            this.gdView.Children.Clear();
            this.gdView.Children.Add(ucListProductInvoiceGeneral);
        }


        private void tvInvoiceStatus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmCashier = () => this;
            ucPage.Menus = new List<string>
            {
                "Out Of Shelves", "Almost Out Of Shelves"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvOutOfInvoice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInvoiceGeneral ucListProductInvoiceGeneral = new ucListProductInvoiceGeneral();

            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();

            // Lay du lieu tu invoice & order
            ProductInvoiceVM.Items = getFrmMain().unitOfWork.GetInventory.ProductInvoicesOrder;
            ProductInvoiceVM.Items = ProductInvoiceVM.GetOutOf();

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            var getList = ProductByCategoryViewModel.ConvertProductInvoices(ProductInvoiceVM.Items);

            ucListProductInvoiceGeneral.getProductInvoiceByCatRepo = () => new RepositoryBase<ProductInvoiceByCategory>(getList);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProductInvoiceGeneral);
        }

        private void tvAlmostOutOfInvoice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInvoiceGeneral ucListProductInvoiceGeneral = new ucListProductInvoiceGeneral();

            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();

            // Lay du lieu tu invoice & order
            Inventory inventory = getFrmMain().unitOfWork.GetInventory;
            ProductInvoiceVM.Items = inventory.ProductInvoicesOrder;

            ProductInvoiceVM.Items = ProductInvoiceVM.GetAlmostOutOf(inventory.MinQuantity);

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            var getList = ProductByCategoryViewModel.ConvertProductInvoices(ProductInvoiceVM.Items);

            ucListProductInvoiceGeneral.getProductInvoiceByCatRepo = () => new RepositoryBase<ProductInvoiceByCategory>(getList);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProductInvoiceGeneral);
        }


        private void tvRevenueProfit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmCashier = () => this;
            ucPage.Menus = new List<string>
            {
                "View Orders Revenue", "View Products in Orders"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvRevenueProfitOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucOrderRevenueTable ucOrderRevenueTable = new ucOrderRevenueTable();
            ucOrderRevenueTable.getfrmCashier = () => this;
            ucOrderRevenueTable.getOrderRepo = () => getFrmMain().unitOfWork.GetRepositoryOrders;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucOrderRevenueTable);
        }

        private void tvRevenueProfitProductOrder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucProductInvoiceRevenue ucProductInvoiceRevenue = new ucProductInvoiceRevenue();
            ucProductInvoiceRevenue.getOrderRepo = () => getFrmMain().unitOfWork.GetRepositoryOrders;

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucProductInvoiceRevenue);
        }
        #endregion

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            getFrmLogin().ClearLogin();
            getFrmLogin().Show();
            getFrmLogin().txtUsername.Focus();
        }

        public void CheckMenu(string menu)
        {
            switch (menu)
            {
                case "New Order":
                    tvCreateOrder_MouseLeftButtonUp(null, null);
                    break;
                case "View Orders":
                    tvViewOrder_MouseLeftButtonUp(null, null);
                    break;
                case "Details of Order":
                    tvOrderDetails_MouseLeftButtonUp(null, null);
                    break;

                case "Register Customer":
                    tvRegisterCustomer_MouseLeftButtonUp(null, null);
                    break;
                case "View Customers":
                    tvViewCustomer_MouseLeftButtonUp(null, null);
                    break;

                case "Out Of Shelves":
                    tvOutOfInvoice_MouseLeftButtonUp(null, null);
                    break;
                case "Almost Out Of Shelves":
                    tvAlmostOutOfInvoice_MouseLeftButtonUp(null, null);
                    break;

                case "View Orders Revenue":
                    tvRevenueProfitOrder_MouseLeftButtonUp(null, null);
                    break;
                case "View Products in Orders":
                    tvRevenueProfitProductOrder_MouseLeftButtonUp(null, null);
                    break;
            }
        }

        private void tvLogOut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BtnLogOut_Click(null, null);
        }
    }
}
