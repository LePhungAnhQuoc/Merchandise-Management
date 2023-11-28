using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmStocker.xaml
    /// </summary>
    public partial class frmStocker : Window
    {
        #region GetData
        public Func<MainWindow> getFrmMain;
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
            DependencyProperty.Register("Account", typeof(Account), typeof(frmStocker), new UIPropertyMetadata(new Account()));
        #endregion

        public frmStocker()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += FrmStocker_Loaded;
            //btnLogOut.Click += BtnLogOut_Click;
            Closed += FrmStocker_Closed;
        }

        private void FrmStocker_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void FrmStocker_Loaded(object sender, RoutedEventArgs e)
        {
            Account = getFrmMain().getAccountInfo().Account;
        }
        
        #region TreeViewItem
        private void tvProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmStocker = () => this;
            ucPage.Menus = new List<string>
            {
                "All Product", "Expire Date"
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


        private void tvReceipt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmStocker = () => this;
            ucPage.Menus = new List<string>
            {
                "New Receipt", "View Receipts", "Details of Receipt"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvCreateReceipt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucCreateReceipt ucCreateReceipt = new ucCreateReceipt();
            ucCreateReceipt.getfrmStocker = () => this;
            ucCreateReceipt.getAccount = getFrmMain().getAccountInfo;
            ucCreateReceipt.getInventory = () => getFrmMain().unitOfWork.GetInventory;
            ucCreateReceipt.getImportInventory = () => getFrmMain().unitOfWork.GetImportInventory;
            ucCreateReceipt.getReceiptRepo = () => getFrmMain().unitOfWork.GetRepositoryReceipts;
            ProductByCategoryViewModel ProductByCategoryVM = new ProductByCategoryViewModel();
            RepositoryBase<ProductByCategory> productByCatRepo = ProductByCategoryVM.ConvertTo(getFrmMain().unitOfWork.GetRepositoryProducts.Gets());
            ucCreateReceipt.getProductByCatRepo = () => productByCatRepo;

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucCreateReceipt);
        }

        private void tvViewReceipt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucReceiptTable ucReceiptTable = new ucReceiptTable();
            ucReceiptTable.getfrmStocker = () => this;
            ucReceiptTable.getReceiptRepo = () => getFrmMain().unitOfWork.GetRepositoryReceipts;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucReceiptTable);
        }

        private void tvReceiptDetails_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucDetailsByIdReceipt ucDetailsByIdReceipt = new ucDetailsByIdReceipt();
            ucDetailsByIdReceipt.lblHeader = string.Empty;
            ucDetailsByIdReceipt.getfrmStocker = () => this;
            ucDetailsByIdReceipt.GetReceiptRepo = () => getFrmMain().unitOfWork.GetRepositoryReceipts;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucDetailsByIdReceipt);
        }


        private void tvInvoice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmStocker = () => this;
            ucPage.Menus = new List<string>
            {
                "New Invoice", "View Invoices", "Details of Invoice"
            };
            gdView.Children.Clear();
            gdView.Children.Add(ucPage);
        }

        private void tvCreateInvoice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucCreateInvoice ucCreateInvoice = new ucCreateInvoice();
            ucCreateInvoice.getfrmStocker = () => this;
            ucCreateInvoice.getAccount = getFrmMain().getAccountInfo;
            ucCreateInvoice.getInventory = () => getFrmMain().unitOfWork.GetInventory;
            ucCreateInvoice.getInvoiceRepo = () => getFrmMain().unitOfWork.GetRepositoryInvoices;

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            RepositoryBase<ProductInventory> ProductInventoryRepo = getFrmMain().unitOfWork.GetRepositoryProductInventorys;
            List<ProductInventoryByCategory> ProductInventoryByCats = ProductByCategoryViewModel.ConvertProductInventorys(ProductInventoryRepo.Gets());
            ucCreateInvoice.getProductInventoryByCatRepo = () => new RepositoryBase<ProductInventoryByCategory>(ProductInventoryByCats);

            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucCreateInvoice);
        }

        private void tvViewInvoice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucInvoiceTable ucInvoiceTable = new ucInvoiceTable();
            ucInvoiceTable.getfrmStocker = () => this;
            ucInvoiceTable.getInvoiceRepo = () => getFrmMain().unitOfWork.GetRepositoryInvoices;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucInvoiceTable);
        }

        private void tvInvoiceDetails_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucDetailsByIdInvoice ucDetailsByIdInvoice = new ucDetailsByIdInvoice();
            ucDetailsByIdInvoice.lblHeader = string.Empty;
            ucDetailsByIdInvoice.getfrmStocker = () => this;
            ucDetailsByIdInvoice.getInvoiceRepo = () => getFrmMain().unitOfWork.GetRepositoryInvoices;
            getUcPage().gdDisplay.Children.Clear();
            getUcPage().gdDisplay.Children.Add(ucDetailsByIdInvoice);
        }


        private void tvInventory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ucPage ucPage = new ucPage();
            ucPage.getfrmMain = getFrmMain;
            ucPage.getfrmStocker = () => this;
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
            ucPage.getfrmStocker = () => this;
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
        #endregion

        private void tvLogOut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            getFrmLogin().Show();
            getFrmLogin().txtUsername.Focus();
            getFrmLogin().ClearLogin();
        }

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

                case "New Receipt":
                    tvCreateReceipt_MouseLeftButtonUp(null, null);
                    break;
                case "View Receipts":
                    tvViewReceipt_MouseLeftButtonUp(null, null);
                    break;
                case "Details of Receipt":
                    tvReceiptDetails_MouseLeftButtonUp(null, null);
                    break;

                case "New Invoice":
                    tvCreateInvoice_MouseLeftButtonUp(null, null);
                    break;
                case "View Invoices":
                    tvViewInvoice_MouseLeftButtonUp(null, null);
                    break;
                case "Details of Invoice":
                    tvInvoiceDetails_MouseLeftButtonUp(null, null);
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
            }
        }
    }
}
