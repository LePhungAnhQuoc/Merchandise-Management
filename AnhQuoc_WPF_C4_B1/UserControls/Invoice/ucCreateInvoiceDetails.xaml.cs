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
    /// Interaction logic for ucCreateInvoiceDetails.xaml
    /// </summary>
    public partial class ucCreateInvoiceDetails : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<ucCreateInvoice> getucCreateInvoice;
        public Func<RepositoryBase<ProductInventoryByCategory>> getProductInventoryByCatRepo;
        public Func<Invoice> getInvoice;
        #endregion

        #region Properties
		public string txtQuantityInput { get; set; }
		
        public Func<ProductInventory> _getSelectedProduct;
        public Func<ProductInventory> getSelectedProduct
        {
            get
            {
                return _getSelectedProduct;
            }
            set
            {
                _getSelectedProduct = value;

                if (_getSelectedProduct == null)
                {
                    Utilities.CatchError();
                    return;
                }
                if (value() != null)
                {
                    SetQuantityBarState(true);
                    lblProductSelected.Content = _getSelectedProduct().Product.Name;

                    double totalQuantityMax = getMaxQuantity(value());
                    lblMaxQuantity.Content = totalQuantityMax.ToString();

                    txtQuantity.SelectedText = txtQuantity.Text;

                    ToggleButtonQuantity(value());


                }
                else
                {
                    SetQuantityBarState(false);
                    lblMaxQuantity.Content = 0.ToString();

                    lblProductSelected.Content = "None";
                }
                txtQuantity.Text = 0.ToString();
            }
        }

        #endregion

        #region Fields
        private ObservableCollection<InvoiceDetail> InvoiceDetails;
        private ucCreateInvoice ucCreateInvoice;
        private ucInvoiceDetailPosters ucInvoiceDetailPosters;

        private List<List<double>> MaxQuantities;
        private List<ProductInventoryByCategory> getClone;
        #endregion

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucCreateInvoiceDetails()
        {
            InitializeComponent();
            MaxQuantities = new List<List<double>>();

            Loaded += ucCreateInvoiceDetails_Loaded;
            txtQuantity.PreviewTextInput += TxtQuantity_PreviewTextInput;
            txtQuantity.PreviewKeyDown += TxtQuantity_PreviewKeyDown;
            btnConfirm.Click += BtnConfirm_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnIncrease.Click += BtnIncrease_Click;
            btnDecrease.Click += BtnDecrease_Click;
			
			this.DataContext = this;
        }

        private void ucCreateInvoiceDetails_Loaded(object sender, RoutedEventArgs e)
        {
            ucCreateInvoice = getucCreateInvoice();

            InvoiceDetails = new ObservableCollection<InvoiceDetail>();

            getClone = getProductInventoryByCatRepo().Gets().CloneList();
            var cloneRepo = new RepositoryBase<ProductInventoryByCategory>(getClone);

            ucListProductInventory.getProductInventoryByCatRepo = () => cloneRepo;
            ucListProductInventory.getucCreateInvoiceDetails = () => this;

            ucInvoiceDetailPosters = new ucInvoiceDetailPosters();
            ucInvoiceDetailPosters.getList = () => InvoiceDetails;
            ucInvoiceDetailPosters.getucCreateInvoiceDetails = () => this;
            scrollDetails.Content = ucInvoiceDetailPosters;

            getSelectedProduct = () => null;


            foreach (ProductInventoryByCategory products in getClone)
            {
                List<double> quantitys = new List<double>();
                foreach (ProductInventory item in products.Products)
                {
                    double quantityValue = item.Quantity;
                    quantitys.Add(quantityValue);
                }
                MaxQuantities.Add(quantitys);
            }
        }


        private void TxtQuantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                BtnIncrease_Click(null, null);
            }
            else if (e.Key == Key.Down)
            {
                BtnDecrease_Click(null, null);
            }
        }

        private void TxtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Is check Number
            char character = '\0';
            try
            {
                character = Convert.ToChar(e.Text);
            }
            catch
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Utilities.ValidateNumber(character, 0, 9) != 1)
            {
                e.Handled = true;
            }

            // Is check enter
            try
            {

                if (Convert.ToChar(e.Text) == Utilities.EnterKey())
                {
                    if (getSelectedProduct == null)
                        return;
                    if (FindInvoiceDetail(getSelectedProduct()) == null)
                        BtnConfirm_Click(null, null);
                    else
                        BtnUpdate_Click(null, null);
                }
            }
            catch
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }


        private QuantityErrorType GetResultQuantity(ProductInventory productChoosed, out int quantity)
        {
            quantity = 0;
            if (txtQuantity.Text == string.Empty)
            {
                return QuantityErrorType.StringEmpty;
            }
            try
            {
                quantity = Convert.ToInt32(txtQuantity.Text);
            }
            catch
            {
                return QuantityErrorType.CatchException;
            }

            if (quantity == 0)
            {
                return QuantityErrorType.EqualZero;
            }
            if (quantity < 0)
            {
                return QuantityErrorType.Less;
            }

            double totalQuantityMax = getMaxQuantity(productChoosed);
            if (quantity > totalQuantityMax)
            {
                return QuantityErrorType.Greater;
            }
            return QuantityErrorType.Valid;
        }

        private void BtnIncrease_Click(object sender, RoutedEventArgs e)
        {
            int tempQuantity = 0;
            int quantity = 0;
            QuantityErrorType resultQuantity = GetResultQuantity(getSelectedProduct(), out quantity);

            if (resultQuantity == QuantityErrorType.StringEmpty
                || resultQuantity == QuantityErrorType.CatchException)
            {
                quantity = 0;
                tempQuantity = 0;
            }
            else
            {
                tempQuantity = quantity;
                quantity++;
            }
            txtQuantity.Text = quantity.ToString();

            resultQuantity = GetResultQuantity(getSelectedProduct(), out quantity);
            if (resultQuantity != QuantityErrorType.Valid)
            {
                ShowQuantityMessage(resultQuantity);
                txtQuantity.Text = tempQuantity.ToString();
            }
            txtQuantity.Focus();
        }

        private void BtnDecrease_Click(object sender, RoutedEventArgs e)
        {
            int tempQuantity = 0;
            int quantity = 0;
            QuantityErrorType resultQuantity = GetResultQuantity(getSelectedProduct(), out quantity);

            if (resultQuantity == QuantityErrorType.StringEmpty)
            {
                return;
            }
            if (resultQuantity == QuantityErrorType.CatchException)
            {
                quantity = 0;
                tempQuantity = 0;
            }
            else
            {
                tempQuantity = quantity;
                quantity--;
            }
            txtQuantity.Text = quantity.ToString();

            resultQuantity = GetResultQuantity(getSelectedProduct(), out quantity);
            if (resultQuantity != QuantityErrorType.Valid)
            {
                ShowQuantityMessage(resultQuantity);
                txtQuantity.Text = tempQuantity.ToString();
            }
            txtQuantity.Focus();
        }


        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            int quantity = 0;
            ProductInventory productChoosed = getSelectedProduct();
            if (productChoosed == null)
            {
                MessageBox.Show("Please choose your Product", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            QuantityErrorType resultQuantity = GetResultQuantity(productChoosed, out quantity);
            if (resultQuantity != QuantityErrorType.Valid)
            {
                ShowQuantityMessage(resultQuantity);
                return;
            }

            int no = InvoiceDetails.Count;
            InvoiceDetailViewModel InvoiceDetailViewModel = new InvoiceDetailViewModel();
            InvoiceDetail newItem = new InvoiceDetail();
            newItem.Id = InvoiceDetailViewModel.GetId(no + 1);
            newItem.IdInvoice = getInvoice().Id;

            try
            {
                newItem.Product = productChoosed.Product;
                newItem.Quantity = quantity;
                newItem.TotalPrice.In = newItem.Quantity * newItem.Product.Price.In;
                newItem.TotalPrice.Out = newItem.Quantity * newItem.Product.Price.Out;
                newItem.Date = DateTime.Now;

                // Settings after confirm
                productChoosed.Quantity -= newItem.Quantity;
                productChoosed.Price.In -= newItem.TotalPrice.In;
                productChoosed.Price.Out -= newItem.TotalPrice.Out;
            }
            catch
            {
                Utilities.CatchError();
            }
            AddDetail(newItem);
            ToggleButtonQuantity(productChoosed);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int quantity = 0;
            ProductInventory productChoosed = getSelectedProduct();
            if (productChoosed == null)
            {
                MessageBox.Show("Please choose your Product", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            QuantityErrorType resultQuantity = GetResultQuantity(productChoosed, out quantity);
            if (resultQuantity != QuantityErrorType.Valid)
            {
                ShowQuantityMessage(resultQuantity);
                return;
            }

            InvoiceDetail newItem = FindInvoiceDetail(productChoosed);

            try
            {
                double quantityBonus = quantity - newItem.Quantity;
                Price priceBonus = new Price();
                priceBonus.In = (quantity * newItem.Product.Price.In) - newItem.TotalPrice.In;
                priceBonus.Out = (quantity * newItem.Product.Price.Out) - newItem.TotalPrice.Out;

                newItem.Quantity = quantity;
                newItem.TotalPrice.In = newItem.Quantity * newItem.Product.Price.In;
                newItem.TotalPrice.Out = newItem.Quantity * newItem.Product.Price.Out;

                // Settings after confirm
                productChoosed.Quantity -= quantityBonus;
                productChoosed.Price.In -= priceBonus.In;
                productChoosed.Price.Out -= priceBonus.Out;
            }
            catch
            {
                Utilities.CatchError();
            }
            ToggleButtonQuantity(productChoosed);
        }


        private void AddDetail(InvoiceDetail newItem)
        {
            InvoiceDetails.Add(newItem);
            getInvoice().Details.ListDetail.Add(newItem);

            ucInvoiceDetailPosters.getList = () => InvoiceDetails;
        }

        private void RemoveDetail(InvoiceDetail newItem)
        {
            InvoiceDetails.Remove(newItem);
            getInvoice().Details.ListDetail.Remove(newItem);

            ucInvoiceDetailPosters.getList = () => InvoiceDetails;
        }
        
        public void DeleteDetailItem(InvoiceDetail detail)
        {
            ucCreateInvoice.Reset(detail);
            RemoveDetail(detail);
            if (getSelectedProduct != null)
                ToggleButtonQuantity(getSelectedProduct());
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            List<InvoiceDetail> details = InvoiceDetails.ToList();
            if (details.Count == 0)
            {
                MessageBox.Show("There are no details in the list", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult msbResult = MessageBox.Show(Utilities.GetSaveMessage("Invoice"), "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;

            ucCreateInvoice.Save(InvoiceDetails.ToList(), getClone);
            ExitPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msbResult = MessageBox.Show(Utilities.GetCancelMessage("Invoice"), "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;

            ucCreateInvoice.Reset();
            ExitPage();
        }


        private string GetErrorQuantityMessage()
        {
            return "Please input quantity greater than 0 and less than product quantity";
        }

        private int getIndex(ProductInventory productChoosed)
        {
            if (productChoosed.Product.Category == ProductCategory.Electronic)
                return 0;
            if (productChoosed.Product.Category == ProductCategory.Porcelain)
                return 1;
            if (productChoosed.Product.Category == ProductCategory.Food)
                return 2;
            return -1;
        }

        private double getMaxQuantity(ProductInventory productChoosed)
        {
            int index = getIndex(productChoosed);
            List<double> quantitys = MaxQuantities[index];

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            ProductByCategoryViewModel.ItemInventorys = getClone;
            int indexQuan = ProductByCategoryViewModel.getIndexInList(productChoosed);

            // if (indexQuan == -1)

            double totalQuantityMax = quantitys[indexQuan];
            return totalQuantityMax;
        }

        private void ShowQuantityMessage(QuantityErrorType resultQuantity)
        {
            if (resultQuantity == QuantityErrorType.StringEmpty)
            {
                MessageBox.Show(Utilities.GetFormEmptyMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (resultQuantity == QuantityErrorType.CatchException)
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(GetErrorQuantityMessage(), "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetQuantityBarState(bool state)
        {
            gdQuantity.IsEnabled = state;
            if (state)
            {
                txtQuantity.Focus();
            }
        }

        private InvoiceDetail FindInvoiceDetail(ProductInventory productChoosed)
        {
            InvoiceDetailViewModel InvoiceDetailViewModel = new InvoiceDetailViewModel();
            InvoiceDetailViewModel.ItemList.ListDetail = InvoiceDetails.ToList();
            return InvoiceDetailViewModel.FindByIdProduct(productChoosed.Product.Id);
        }

        private void ToggleButtonQuantity(ProductInventory productChoosed)
        {
            if (FindInvoiceDetail(productChoosed) == null)
            {
                btnConfirm.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
            else
            {
                btnConfirm.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Visible;
            }
        }

        private void ExitPage()
        {
            getfrmStocker().getUcPage().gdDisplay.Children.Clear();
            getfrmStocker().getUcPage().gdDisplay.Children.Add(ucCreateInvoice);
        }
    }
}
