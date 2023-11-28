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
    /// Interaction logic for ucCreateReceiptDetails.xaml
    /// </summary>
    public partial class ucCreateReceiptDetails : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<ucCreateReceipt> getucCreateReceipt;
        public Func<RepositoryBase<ProductByCategory>> getProductByCatRepo;
        public Func<Receipt> getReceipt;
        #endregion

        #region Properties
        public string txtQuantityInput { get; set; }

        public Func<Product> _getSelectedProduct;
        public Func<Product> getSelectedProduct
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
                    lblProductSelected.Content = value().Name;
                    txtQuantity.SelectedText = txtQuantity.Text;

                    ToggleButtonQuantity(value());
                }
                else
                {
                    SetQuantityBarState(false);
                    lblProductSelected.Content = "None";
                }
                txtQuantity.Text = 0.ToString();
            }
        }
        #endregion

        #region Fiels
        private ObservableCollection<ReceiptDetail> ReceiptDetails;
        private ucCreateReceipt ucCreateReceipt;
        private ucReceiptDetailPosters ucReceiptDetailPosters;
        #endregion

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucCreateReceiptDetails()
        {
            InitializeComponent();
            Loaded += UcCreateReceiptDetails_Loaded;
            txtQuantity.PreviewTextInput += TxtQuantity_PreviewTextInput;
            txtQuantity.PreviewKeyDown += TxtQuantity_PreviewKeyDown;
            btnConfirm.Click += BtnConfirm_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnIncrease.Click += BtnIncrease_Click;
            btnDecrease.Click += BtnDecrease_Click;

            // Pass data
            this.DataContext = this;
        }

        private void UcCreateReceiptDetails_Loaded(object sender, RoutedEventArgs e)
        {
            ucCreateReceipt = getucCreateReceipt();
            ReceiptDetails = new ObservableCollection<ReceiptDetail>();

            ucListProduct.getucCreateReceiptDetails = () => this;
            ucListProduct.getProductByCatRepo = getProductByCatRepo;
            
            ucReceiptDetailPosters = new ucReceiptDetailPosters();
            ucReceiptDetailPosters.getList = () => ReceiptDetails;
            ucReceiptDetailPosters.getucCreateReceiptDetails = () => this;
            scrollDetails.Content = ucReceiptDetailPosters;

            getSelectedProduct = () => null;
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
            // Is check number
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
                    {
                        return;
                    }
                    if (FindReceiptDetail(getSelectedProduct()) == null)
                        BtnConfirm_Click(null, null);
                    else
                    {
                        BtnUpdate_Click(null, null);
                    }
                }
            }
            catch
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private QuantityErrorType GetResultQuantity(Product productChoosed, out int quantity)
        {
            quantity = 0;
            if (txtQuantity.Text == string.Empty)
            {
                return QuantityErrorType.StringEmpty;
            }
            // Checking format exception
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

            Product productChoosed = getSelectedProduct();
            if (productChoosed == null)
            {
                UtiViewModel.NotifyChooseProduct();
                return;
            }

            QuantityErrorType resultQuantity = GetResultQuantity(productChoosed, out quantity);
            if (resultQuantity != QuantityErrorType.Valid)
            {
                ShowQuantityMessage(resultQuantity);
                return;
            }
            int no = ReceiptDetails.Count;
            ReceiptDetailViewModel ReceiptDetailViewModel = new ReceiptDetailViewModel();
            ReceiptDetail newItem = new ReceiptDetail();
            newItem.Id = ReceiptDetailViewModel.GetId(no + 1);
            newItem.IdReceipt = getReceipt().Id;

            Product product = getSelectedProduct();
            try
            {
                newItem.Product = product;

                newItem.Quantity = quantity;
                newItem.TotalPrice.In = newItem.Quantity * newItem.Product.Price.In;
                newItem.TotalPrice.Out = newItem.Quantity * newItem.Product.Price.Out;
                newItem.Date = DateTime.Now;
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
            Product productChoosed = getSelectedProduct();
            if (productChoosed == null)
            {
                UtiViewModel.NotifyChooseProduct();
                return;
            }
            ReceiptDetail getItem = FindReceiptDetail(productChoosed);

            int quantity = 0;
            QuantityErrorType resultQuantity = GetResultQuantity(productChoosed, out quantity);
            if (resultQuantity != QuantityErrorType.Valid)
            {
                ShowQuantityMessage(resultQuantity);
                return;
            }
            getItem.Quantity = quantity;
            getItem.TotalPrice.In = getItem.Quantity * getItem.Product.Price.In;
            getItem.TotalPrice.Out = getItem.Quantity * getItem.Product.Price.Out;

            ToggleButtonQuantity(productChoosed);
        }


        public void AddDetail(ReceiptDetail newItem)
        {
            ReceiptDetails.Add(newItem);
            getReceipt().Details.ListDetail.Add(newItem);
            ucReceiptDetailPosters.getList = () => ReceiptDetails;
        }

        public void RemoveDetail(ReceiptDetail newItem)
        {
            ReceiptDetails.Remove(newItem);
            getReceipt().Details.ListDetail.Remove(newItem);
            ucReceiptDetailPosters.getList = () => ReceiptDetails;
        }

        public void DeleteDetailItem(ReceiptDetail detail)
        {
            RemoveDetail(detail);
            if (getSelectedProduct != null)
                ToggleButtonQuantity(getSelectedProduct());
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            List<ReceiptDetail> details = ReceiptDetails.ToList();
            if (details.Count == 0)
            {
                MessageBox.Show("There are no details in the list", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult msbResult = MessageBox.Show(Utilities.GetSaveMessage("Receipt"), "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;

            ucCreateReceipt.Save(ReceiptDetails.ToList());
            ExitPage();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msbResult = MessageBox.Show(Utilities.GetCancelMessage("Receipt"), "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;
            ExitPage();
        }


        private string GetErrorQuantityMessage()
        {
            return "Please input quantity greater than 0 and less than product quantity";
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

        private ReceiptDetail FindReceiptDetail(Product productChoosed)
        {
            ReceiptDetailViewModel ReceiptDetailViewModel = new ReceiptDetailViewModel();
            ReceiptDetailViewModel.ItemList.ListDetail = ReceiptDetails.ToList();

            ReceiptDetail getItem = ReceiptDetailViewModel.FindByIdProduct(productChoosed.Id);
            return getItem;
        }

        private void ToggleButtonQuantity(Product productChoosed)
        {
            if (FindReceiptDetail(productChoosed) == null)
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
            getfrmStocker().getUcPage().gdDisplay.Children.Add(ucCreateReceipt);
        }
    }
}
