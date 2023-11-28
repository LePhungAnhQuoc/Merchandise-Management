using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmAdmin.xaml
    /// </summary>
    public partial class frmAdmin : Window
    {
        #region GetData
        public Func<MainWindow> getFrmMain;
        public Func<frmStocker> getfrmStocker;
        public Func<frmCashier> getfrmCashier;
        public Func<frmLogin> getFrmLogin;
        public Func<ucPage> getUcPage;
        #endregion

        #region DependencyProperties
        public Account Account
        {
            get { return (Account)GetValue(AccountProperty); }
            set { SetValue(AccountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Account.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccountProperty =
            DependencyProperty.Register("Account", typeof(Account), typeof(frmAdmin), new UIPropertyMetadata(new Account()));
        #endregion

        public frmAdmin()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += FrmAdmin_Loaded;
            //btnLogOut.Click += BtnLogOut_Click;
            Closed += FrmAdmin_Closed;
        }

        private void FrmAdmin_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void FrmAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            Account = getFrmMain().getAccountInfo().Account;
        }
        
        #region TreeViewItem
        private void tvHomePage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucHomePage ucHomePage = new ucHomePage();
            ucHomePage.getFrmMain = getFrmMain;
            gdView.Children.Clear();
            gdView.Children.Add(ucHomePage);
        }

        private void tvProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmStocker = getfrmStocker;
            
            ///
            ucPage.getfrmAdmin = () => this;

            ucPage.Menus = new List<string>
            {
                "All Product", "Expire Date", "Update Price"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvViewProducts_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProduct ucListProduct = new ucListProduct();

            ProductByCategoryViewModel ProductByCategoryVM = new ProductByCategoryViewModel();
            RepositoryBase<ProductByCategory> productByCatRepo = ProductByCategoryVM.ConvertTo(getFrmMain().unitOfWork.GetRepositoryProducts.Gets());
            ucListProduct.getProductByCatRepo = () => productByCatRepo;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProduct);
        }

        private void tvExpDateProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProduct ucListProduct = new ucListProduct();
            ProductByCategoryViewModel ProductByCategoryVM = new ProductByCategoryViewModel();

            RepositoryBase<ProductByCategory> productByCatRepo = ProductByCategoryVM.ConvertTo(getFrmMain().unitOfWork.GetRepositoryProducts.Gets());
            productByCatRepo = ProductByCategoryVM.FillExpDate(productByCatRepo.Gets());

            var getList = ProductByCategoryVM.ConvertBack(productByCatRepo);
            List<ProductCategory> categories = ProductByCategoryVM.FillCategories(productByCatRepo);

            ProductViewModel ProductViewModel = new ProductViewModel();
            ProductViewModel.ItemsByCatRepo = new RepositoryBase<List<Product>>(getList);
            ProductViewModel.WriteItemsByCatAll(Constants.fExpDateProducts, categories);

            ucListProduct.getProductByCatRepo = () => productByCatRepo;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProduct);
        }

        private void tvUpdatePrice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucChangeInputPrice ucChangeInputPrice = new ucChangeInputPrice();
            ucChangeInputPrice.getfrmStocker = getfrmStocker;
            ucChangeInputPrice.getNumberOfEmployees = () => getFrmMain().unitOfWork.GetRepositoryAccounts.Length();

            ucChangeInputPrice.getReceiptRepo = () => getFrmMain().unitOfWork.GetRepositoryReceipts;
            ucChangeInputPrice.getInvoiceRepo = () => getFrmMain().unitOfWork.GetRepositoryInvoices;
            ucChangeInputPrice.getOrderRepo = () => getFrmMain().unitOfWork.GetRepositoryOrders;

            ucChangeInputPrice.getInventory = () => getFrmMain().unitOfWork.GetInventory;
            ucChangeInputPrice.getImportInventory = () => getFrmMain().unitOfWork.GetImportInventory;
            ucChangeInputPrice.getProductInvoices = () => getFrmMain().unitOfWork.GetInventory.ProductInvoices;
            ucChangeInputPrice.getProductInvoicesOrder = () => getFrmMain().unitOfWork.GetInventory.ProductInvoicesOrder;

            ProductByCategoryViewModel ProductByCategoryVM = new ProductByCategoryViewModel();
            RepositoryBase<ProductByCategory> productByCatRepo = ProductByCategoryVM.ConvertTo(getFrmMain().unitOfWork.GetRepositoryProducts.Gets());

            ucChangeInputPrice.getProductByCatRepo = () => productByCatRepo;
            ucChangeInputPrice.getItemsByCatRepo = () => getFrmMain().unitOfWork.GetRepositoryProducts;

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucChangeInputPrice);
        }
        

        private void tvInventory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmStocker = getfrmStocker;
            ucPage.Menus = new List<string>
            {
                "Inventory", "Import", "Export"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvShowInventory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInventory ucListProductInventory = new ucListProductInventory();
            var getOriginRepo = getFrmMain().unitOfWork.GetRepositoryProductInventorys;

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            
            var getList = ProductByCategoryViewModel.ConvertProductInventorys(getOriginRepo.Gets());

            ucListProductInventory.getProductInventoryByCatRepo = () => new RepositoryBase<ProductInventoryByCategory>(getList);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProductInventory);
        }

        private void tvImportInventory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInventoryStatus ucListProductInventoryStatus = new ucListProductInventoryStatus();

            var getOriginalRepo = new RepositoryBase<ProductInventoryStatus>(getFrmMain().unitOfWork.GetImportInventory.ProductsStatus);

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            var getList = ProductByCategoryViewModel.ConvertProductInventoryStatuss(getOriginalRepo.Gets());

            ucListProductInventoryStatus.getProductInventoryStatusByCatRepo = () => new RepositoryBase<ProductInventoryStatusByCategory>(getList);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProductInventoryStatus);
        }

        private void tvExportInventory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInvoice ucListProductInvoice = new ucListProductInvoice();

            var getOriginRepo = new RepositoryBase<ProductInvoice>(getFrmMain().unitOfWork.GetInventory.ProductInvoices);

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            var getList = ProductByCategoryViewModel.ConvertProductInvoices(getOriginRepo.Gets());

            ucListProductInvoice.getProductInvoiceByCatRepo = () => new RepositoryBase<ProductInvoiceByCategory>(getList);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProductInvoice);
        }


        private void tvInventoryStatus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmStocker = getfrmStocker;
            ucPage.Menus = new List<string>
            {
                "Out Of Inventory", "Almost Out Of"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvOutOfInventory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInventory ucListProductInventory = new ucListProductInventory();

            ProductInventoryViewModel ProductInventoryVM = new ProductInventoryViewModel();
            ProductInventoryVM.ItemList.Items = getFrmMain().unitOfWork.GetRepositoryProductInventorys.Gets();
            ProductInventoryVM.ItemList.Items = ProductInventoryVM.FillOutOfInventory();

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            var getList = ProductByCategoryViewModel.ConvertProductInventorys(ProductInventoryVM.ItemList.Items);

            ucListProductInventory.getProductInventoryByCatRepo = () => new RepositoryBase<ProductInventoryByCategory>(getList);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProductInventory);
        }

        private void tvAlmostOutOfInventory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucListProductInventory ucListProductInventory = new ucListProductInventory();

            ProductInventoryViewModel ProductInventoryVM = new ProductInventoryViewModel();
            ProductInventoryVM.ItemList.Items = getFrmMain().unitOfWork.GetRepositoryProductInventorys.Gets();
            ProductInventoryVM.ItemList.Items = ProductInventoryVM.FillAlmostOutOfInventory(getFrmMain().unitOfWork.GetInventory.MinQuantity);

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            var getList = ProductByCategoryViewModel.ConvertProductInventorys(ProductInventoryVM.ItemList.Items);

            ucListProductInventory.getProductInventoryByCatRepo = () => new RepositoryBase<ProductInventoryByCategory>(getList);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucListProductInventory);
        }


        private void tvRevenueProfit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmCashier = getfrmCashier;
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
            ucOrderRevenueTable.getfrmCashier = getfrmCashier;
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

        public void CheckMenu(string menu)
        {
            switch (menu)
            {
                case "All Product":
                    tvViewProducts_MouseLeftButtonUp(null, null);
                    break;
                case "Expire Date":
                    tvExpDateProduct_MouseLeftButtonUp(null, null);
                    break;
                case "Update Price":
                    tvUpdatePrice_MouseLeftButtonUp(null, null);
                    break;

                case "Inventory":
                    tvShowInventory_MouseLeftButtonUp(null, null);
                    break;
                case "Import":
                    tvImportInventory_MouseLeftButtonUp(null, null);
                    break;
                case "Export":
                    tvExportInventory_MouseLeftButtonUp(null, null);
                    break;

                case "Out Of Inventory":
                    tvOutOfInventory_MouseLeftButtonUp(null, null);
                    break;
                case "Almost Out Of":
                    tvAlmostOutOfInventory_MouseLeftButtonUp(null, null);
                    break;

                case "View Orders Revenue":
                    tvRevenueProfitOrder_MouseLeftButtonUp(null, null);
                    break;
                case "View Products in Orders":
                    tvRevenueProfitProductOrder_MouseLeftButtonUp(null, null);
                    break;
            }
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            getFrmLogin().Show();
            getFrmLogin().txtUsername.Focus();
            getFrmLogin().ClearLogin();
        }

        private void tvLogOut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            getFrmLogin().Show();
            getFrmLogin().txtUsername.Focus();
            getFrmLogin().ClearLogin();
        }
    }
}
