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
    /// Interaction logic for ucCreateOrderDetails.xaml
    /// </summary>
    public partial class ucCreateOrderDetails : UserControl, INotifyPropertyChanged
    {
        #region getData
        public Func<frmCashier> getfrmCashier;
        public Func<ucCreateOrder> getUcCreateOrder;
        public Func<Order> getOrder;
        public Func<RepositoryBase<ProductInvoiceByCategory>> getProductInvoiceByCatRepo;
        #endregion

        #region Fields
        private ObservableCollection<OrderDetail> OrderDetails;
        private ucOrderDetailPosters ucOrderDetailPosters;

        private List<List<double>> MaxQuantities;
        private List<ProductInvoiceByCategory> getClone;
        #endregion

        #region Properties
        public string txtQuantityInput { get; set; }
		
        public Func<ProductInvoice> _getSelectedProduct;
        public Func<ProductInvoice> getSelectedProduct
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
                    lblProductSelected.Content = value().Product.Name;

                    double totalQuantityMax = getMaxQuantity(value());
                    lblMaxQuantity.Content = totalQuantityMax.ToString();

                    SetQuantityBarState(true);
                    txtQuantity.SelectedText = txtQuantity.Text;


                    ToggleButtonQuantity(value());
                }
                else
                {
                    lblProductSelected.Content = "None";
                    lblMaxQuantity.Content = 0.ToString();

                    SetQuantityBarState(false);
                }
                txtQuantity.Text = 0.ToString();
            }
        }

        private Order _Order;
        public Order Order
        {
            get { return _Order; }
            set
            {
                _Order = value;
                OnPropertyChanged("Order");
            }
        }
        #endregion

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucCreateOrderDetails()
        {
            InitializeComponent();
            
            ucOrderDetailPosters = new ucOrderDetailPosters();
            MaxQuantities = new List<List<double>>();

            Loaded += ucCreateOrderDetails_Loaded;
            txtQuantity.PreviewTextInput += TxtQuantity_PreviewTextInput;
            txtQuantity.PreviewKeyUp += TxtQuantity_PreviewKeyUp;
            btnConfirm.Click += BtnConfirm_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            btnCustomerDetails.Click += BtnCustomerDetails_Click;

            btnIncrease.Click += BtnIncrease_Click;
            btnDecrease.Click += BtnDecrease_Click;
			
			this.DataContext = this;
        }

        private void ucCreateOrderDetails_Loaded(object sender, RoutedEventArgs e)
        {
            Order = getOrder();
            OrderDetails = new ObservableCollection<OrderDetail>();

            ucListProductInvoiceGeneral.getucCreateOrderDetails = () => this;

            //ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            //getClone = ProductByCategoryViewModel.GetClone(getProductInvoiceByCatRepo().Gets());

            getClone = getProductInvoiceByCatRepo().Gets().CloneList();

            var cloneRepo = new RepositoryBase<ProductInvoiceByCategory>(getClone);
            ucListProductInvoiceGeneral.getProductInvoiceByCatRepo = () => cloneRepo;

            foreach (ProductInvoiceByCategory products in getClone)
            {
                List<double> quantitys = new List<double>();
                foreach (ProductInvoice item in products.Products)
                {
                    double quantityValue = item.TotalQuantity;
                    quantitys.Add(quantityValue);
                }
                MaxQuantities.Add(quantitys);
            }

            LoadUcOrderDetails();
            getSelectedProduct = () => null;
        }

        private int getIndex(ProductInvoice productChoosed)
        {
            if (productChoosed.Product.Category == ProductCategory.Electronic)
                return 0;
            if (productChoosed.Product.Category == ProductCategory.Porcelain)
                return 1;
            if (productChoosed.Product.Category == ProductCategory.Food)
                return 2;
            return -1;
        }

        private void LoadUcOrderDetails()
        {
            scrollDetails.Content = ucOrderDetailPosters;

            ucOrderDetailPosters.getList = () => OrderDetails;
            ucOrderDetailPosters.getucCreateOrderDetails = () => this;
        }


        private void TxtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            #region IsCheckNumber
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
            #endregion

            #region IsCheckEnter
            try
            {
                if (Convert.ToChar(e.Text) == Utilities.EnterKey())
                {
                    if (getSelectedProduct == null)
                        return;
                    if (FindOrderDetail(getSelectedProduct()) == null)
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
            #endregion
        }

        private void TxtQuantity_PreviewKeyUp(object sender, KeyEventArgs e)
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


        private QuantityErrorType GetResultQuantity(ProductInvoice productChoosed, out int quantity)
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
            ProductInvoice productChoosed = getSelectedProduct();
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

            int no = OrderDetails.Count;
            OrderDetailViewModel OrderDetailViewModel = new OrderDetailViewModel();
            OrderDetail newItem = new OrderDetail();

            newItem.Id = OrderDetailViewModel.GetId(no + 1);
            newItem.IdOrder = getOrder().Id;
            try
            {
                newItem.Product = productChoosed.Product;
                newItem.Quantity = quantity;
                newItem.Date = DateTime.Now;

                OrderDetailViewModel OrderDetailVM2 = new OrderDetailViewModel();
                OrderDetailVM2.GetTotalPriceSingle(newItem);

                // Update in invoice (Setting after confirm)
                ProductInvoiceViewModel ProductInvoiceViewModel = new ProductInvoiceViewModel();
                ProductInvoiceViewModel.Update(productChoosed, newItem);
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
            ProductInvoice productChoosed = getSelectedProduct();
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

            int no = OrderDetails.Count;
            OrderDetail newItem = FindOrderDetail(productChoosed);

            try
            {
                double quantityBonus = quantity - newItem.Quantity;
                Price priceBonus = new Price();
                priceBonus.In = newItem.TempPrice.In;
                priceBonus.Out = newItem.TempPrice.Out;

                newItem.Quantity = quantity;

                OrderDetailViewModel OrderDetailVM2 = new OrderDetailViewModel();
                OrderDetailVM2.GetTotalPriceSingle(newItem);

                priceBonus.In = newItem.TempPrice.In - priceBonus.In;
                priceBonus.Out = newItem.TempPrice.Out - priceBonus.Out;

                // Update in invoice (Setting after confirm)
                OrderDetail OrderDetailTemp = new OrderDetail();
                OrderDetailTemp.Quantity = quantityBonus;
                OrderDetailTemp.TempPrice = priceBonus;

                ProductInvoiceViewModel ProductInvoiceViewModel = new ProductInvoiceViewModel();
                ProductInvoiceViewModel.Update(productChoosed, OrderDetailTemp);
            }
            catch
            {
                Utilities.CatchError();
            }
            ToggleButtonQuantity(productChoosed);
        }


        private void AddDetail(OrderDetail newItem)
        {
            OrderDetails.Add(newItem);
            getOrder().Details.ListDetail.Add(newItem);

            ucOrderDetailPosters.getList = () => OrderDetails;
        }

        private void DeleteDetail(OrderDetail newItem)
        {
            OrderDetails.Remove(newItem);
            getOrder().Details.ListDetail.Remove(newItem);

            ucOrderDetailPosters.getList = () => OrderDetails;
        }

        public void DeleteDetailItem(OrderDetail detail)
        {
            getUcCreateOrder().Reset(detail);
            DeleteDetail(detail);
            if (getSelectedProduct != null)
                ToggleButtonQuantity(getSelectedProduct());
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            List<OrderDetail> details = OrderDetails.ToList();
            if (details.Count == 0)
            {
                MessageBox.Show("There are no details in the list", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            getUcCreateOrder().Save(details, getClone);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msbResult = MessageBox.Show(Utilities.GetCancelMessage("Order"), "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;
           
            getUcCreateOrder().Reset();
            ExitPage();
        }

        private void BtnCustomerDetails_Click(object sender, RoutedEventArgs e)
        {
            frmCustomerInfomation frmCustomerInfomation = new frmCustomerInfomation();
            frmCustomerInfomation.getCustomer = () => Order.Customer;
            frmCustomerInfomation.ShowDialog();
        }


        private double getMaxQuantity(ProductInvoice productChoosed)
        {
            int index = getIndex(productChoosed);
            List<double> quantitys = MaxQuantities[index];

            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            ProductByCategoryViewModel.ItemInvoices = getClone;
            int indexQuan = ProductByCategoryViewModel.getIndexInList(productChoosed);

            // if (indexQuan == -1)

            double totalQuantityMax = quantitys[indexQuan];
            return totalQuantityMax;
        }

        private void SetQuantityBarState(bool state)
        {
            gdQuantity.IsEnabled = state;
            if (state)
            {
                txtQuantity.Focus();
            }
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

        private string GetErrorQuantityMessage()
        {
            return "Please input quantity greater than 0 and less than product quantity";
        }

        private OrderDetail FindOrderDetail(ProductInvoice productChoosed)
        {
            OrderDetailViewModel OrderDetailViewModel = new OrderDetailViewModel();
            OrderDetailViewModel.ItemList.ListDetail = OrderDetails.ToList();
            return OrderDetailViewModel.FindByIdProduct(productChoosed.Product.Id);
        }

        private void ToggleButtonQuantity(ProductInvoice productChoosed)
        {
            if (FindOrderDetail(productChoosed) == null)
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
            getfrmCashier().getUcPage().gdDisplay.Children.Clear();
            getfrmCashier().getUcPage().gdDisplay.Children.Add(getUcCreateOrder());
        }
    }
}
