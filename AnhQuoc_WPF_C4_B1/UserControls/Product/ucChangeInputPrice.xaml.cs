using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for ucChangeInputPrice.xaml
    /// </summary>
    public partial class ucChangeInputPrice : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<int> getNumberOfEmployees;
        public Func<Inventory> getInventory;
        public Func<Inventory> getImportInventory;
        public Func<RepositoryBase<Receipt>> getReceiptRepo;
        public Func<RepositoryBase<Invoice>> getInvoiceRepo;
        public Func<RepositoryBase<Order>> getOrderRepo;
        public Func<List<ProductInvoice>> getProductInvoices;
        public Func<List<ProductInvoice>> getProductInvoicesOrder;

        public Func<RepositoryBase<ProductByCategory>> getProductByCatRepo;
        public Func<RepositoryBase<List<Product>>> getItemsByCatRepo;
        #endregion

        #region Properties
        public Func<Product> _getSelectedProduct;
        public Func<Product> getSelectedProduct
        {
            get { return _getSelectedProduct; }
            set
            {
                _getSelectedProduct = value;
                SelectedProduct = value();
            }
        }

        private Product _SelectedProduct;
        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                if (value != null)
                {
                    gdUpdateBar.IsEnabled = true;
                    txtUpdate.Focus();
                }
                else
                {
                    gdUpdateBar.IsEnabled = false;
                }
                OnPropertyChanged("SelectedProduct");
            }
        }
        #endregion

        #region Fields
        private List<Product> UpdatedProducts;
        private List<Price> OldPrices;
        private List<ProductByCategory> products;
        #endregion

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucChangeInputPrice()
        {
            InitializeComponent();
            Loaded += UcChangeInputPrice_Loaded;
            btnSave.Click += BtnSave_Click;
            btnReset.Click += BtnReset_Click;
            txtUpdate.TextChanged += TxtUpdate_TextChanged;
            txtUpdate.PreviewTextInput += TxtUpdate_PreviewTextInput;

            UpdatedProducts = new List<Product>();
            OldPrices = new List<Price>();

            btnSave.IsEnabled = false;
            btnReset.IsEnabled = false;

            this.DataContext = this;
        }

        private void UcChangeInputPrice_Loaded(object sender, RoutedEventArgs e)
        {
            ucListProduct ucListProduct = new ucListProduct();

            products = getProductByCatRepo().Gets().CloneList();
            ucListProduct.getProductByCatRepo = () => new RepositoryBase<ProductByCategory>(products);
            
            ucListProduct.getucChangeInputPrice = () => this;
            scrollProducts.Content = ucListProduct;

            SelectedProduct = null;
        }

        private void TxtUpdate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char value = '\0';
            try
            {
                value = Convert.ToChar(e.Text);
            }
            catch
            {
                Utilities.CatchError();
            }
            if (!char.IsDigit(value))
            {
                e.Handled = true;
            }
        }


        private void TxtUpdate_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Checking
            if (SelectedProduct == null)
            {
                if (gdUpdateBar.IsEnabled)
                    UtiViewModel.NotifyChooseProduct();
                return;
            }

            // Convert and get value
            double updatedPriceInput = 0;
            try
            {
                updatedPriceInput = Convert.ToDouble(txtUpdate.Text);
            }
            catch
            {
                return;
            }
            
            //  Save old Price
            if (!UpdatedProducts.Contains(SelectedProduct))
            {
                UpdatedProducts.Add(SelectedProduct);

                Price oldPrice = new Price();
                oldPrice.In = SelectedProduct.Price.In;
                oldPrice.Out = SelectedProduct.Price.Out;
                OldPrices.Add(oldPrice);
            }
            else
            {
                btnSave.IsEnabled = true;
                btnReset.IsEnabled = true;
            }
            // Update new price
            int numberOfEmployees = getNumberOfEmployees();
            ProductViewModel ProductVM = new ProductViewModel();
            ProductVM.Item = SelectedProduct;
            
            SelectedProduct.Price.In = updatedPriceInput;
            SelectedProduct.Price.Out = ProductVM.CalculatePriceOutput(numberOfEmployees);

            ProductViewModel ProductViewModel = new ProductViewModel();
            ProductViewModel.Item = SelectedProduct;
            ProductViewModel.OnPriceChangeUI(getInventory(), getImportInventory(), getReceiptRepo().Gets(), getInvoiceRepo().Gets(), getOrderRepo().Gets(),
                getProductInvoices(), getProductInvoicesOrder());
        }
        
        // Lưu xuống Database
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msbResult = MessageBox.Show(Utilities.GetSaveMessage("new price of product"), "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;

            ProductByCategoryViewModel ProductByCategoryVM = new ProductByCategoryViewModel();
            ProductByCategoryVM.UpdateListSource(getProductByCatRepo().Gets(), products);

            ProductViewModel ProductVM = new ProductViewModel();

            // Lưu thay đối trong File Products.xml
            ProductVM.ItemsByCatRepo = getItemsByCatRepo();

            List<ProductCategory> categories = new List<ProductCategory>
            {
                ProductCategory.Electronic,
                ProductCategory.Porcelain,
                ProductCategory.Food
            };
            ProductVM.WriteItemsByCatAll(Constants.fProducts, categories);
            
            // Lưu trong Inventory, ImportInventory, Receipts
            ProductVM.OnPriceChangeDatabase(getInventory(), getImportInventory(), getReceiptRepo().Gets(), getInvoiceRepo().Gets(), getOrderRepo().Gets(),
                getProductInvoices(), getProductInvoicesOrder());
        }

        // Hoàn tác (Trả về giá cũ)
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msbResult = MessageBox.Show("Are you sure to reset all product price", "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;

            int index = 0;
            foreach (Product product in UpdatedProducts)
            {
                product.Price.In = OldPrices[index].In;
                product.Price.Out = OldPrices[index].Out;

                index++;
            }
        }
    }
}
