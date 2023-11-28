using System;
using System.Collections.Generic;
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
    /// Interaction logic for ucCreateOrder.xaml
    /// </summary>
    public partial class ucCreateOrder : UserControl, INotifyPropertyChanged
    {
        #region getData
        public Func<frmCashier> getFrmCashier;
        public Func<AccountInfo> getAccount;
        public Func<RepositoryBase<ProductInvoice>> getProductInvoiceOrderRepo;
        public Func<RepositoryBase<Customer>> getCustomerRepo;
        public Func<RepositoryBase<Order>> getOrderRepo;
        #endregion

        #region Properties
        private Order _newItem;
        public Order newItem
        {
            get { return _newItem; }
            set
            {
                _newItem = value;
                OnPropertyChanged("newItem");
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

        public ucCreateOrder()
        {
            InitializeComponent();
            this.DataContext = this;
            Loaded += ucCreateOrder_Loaded;
            btnCreate.Click += BtnCreate_Click;
        }

        private void ucCreateOrder_Loaded(object sender, RoutedEventArgs e)
        {
            // Cấp phát 1 newOrder mới (trong method Load())
            newItem = new Order();
            GetInfomation();
        }
        
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Kiem tra so luong san pham trong kho xuat con khong truoc khi load nghiep vu
            ProductInvoiceViewModel ProductInvoiceViewModel = new ProductInvoiceViewModel();
            ProductInvoiceViewModel.Items = getProductInvoiceOrderRepo().Gets();
            if (ProductInvoiceViewModel.IsEmpty())
            {
                MessageBox.Show(Utilities.GetListEmptyMessage("Product", "Invoice"), "", MessageBoxButton.OK, MessageBoxImage.Information);
                ExitPage();
                return;
            }

            #region GetCustomer
            CustomerViewModel customerVM = new CustomerViewModel();
            customerVM.Items = getCustomerRepo().Gets();
            List<Customer> guests = customerVM.FillGuest();
            string userCreated = getAccount().Account.Username;

            if (CreateCustomer(getCustomerRepo(), guests.Count) == false)
            {
                return;
            }
            #endregion

            ucCreateOrderDetails ucCreateOrderDetails = new ucCreateOrderDetails();
        
            ucCreateOrderDetails.getfrmCashier = getFrmCashier;
            ucCreateOrderDetails.getOrder = () => newItem;
            ucCreateOrderDetails.getUcCreateOrder = () => this;


            // Lấy dữ liệu từ Product invoice & order
            var getList = ConvertToByCat(getProductInvoiceOrderRepo().Gets());
            ucCreateOrderDetails.getProductInvoiceByCatRepo = () => new RepositoryBase<ProductInvoiceByCategory>(getList);
            
            getFrmCashier().getUcPage().gdDisplay.Children.Clear();
            getFrmCashier().getUcPage().gdDisplay.Children.Add(ucCreateOrderDetails);
        }

        private List<ProductInvoiceByCategory> ConvertToByCat(List<ProductInvoice> source)
        {
            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            return ProductByCategoryViewModel.ConvertProductInvoices(source);
        }

        public bool CreateCustomer(RepositoryBase<Customer> customerRepo, int numberOfGuests)
        {
            frmGetIDCardCustomer frmGetIDCardCustomer = new frmGetIDCardCustomer();
            frmGetIDCardCustomer.getCustomerRepo = () => customerRepo;
            frmGetIDCardCustomer.ShowDialog();
            Customer customerForm = frmGetIDCardCustomer.newItem;
            
            newItem.Customer = customerForm;

            CustomerViewModel customerVM = new CustomerViewModel();

            // Neu la Guest
            if (newItem.Customer.IsGuest)
                newItem.Customer = customerVM.CreateGuest(numberOfGuests + 1);
            return true;
        }

        private void PrintOrder()
        {
            frmOrder frmOrder = new frmOrder();
            frmOrder.getOrder = () => newItem;
            frmOrder.ShowDialog();
        }

        public void Save(List<OrderDetail> details, List<ProductInvoiceByCategory> newList)
        {
            OrderDetailViewModel OrderDetailViewModel = new OrderDetailViewModel();
            OrderDetailViewModel.ItemList = newItem.Details;
            OrderDetailViewModel.GetListInfo();

            newItem.TotalQuantity = newItem.Details.TotalQuantity;
            newItem.TotalPrice = newItem.Details.TotalPrice;

            MessageBoxResult msbResult = MessageBox.Show(Utilities.GetSaveMessage("Order"), "", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
            if (msbResult == MessageBoxResult.Cancel) return;
            PrintOrder();
            
            /*** Cập nhật trên giao diện ***/
            
            // Thêm Order
            getOrderRepo().Add(newItem);

            // Cập nhật danh sách nguồn
            List<ProductInvoice> source = getProductInvoiceOrderRepo().Gets();

            List<ProductInvoiceByCategory> getList = ConvertToByCat(source);
            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            ProductByCategoryViewModel.UpdateListSource(getList, newList);

            /*** Cập nhật dưới tập tin ***/
            OrderViewModel OrderVM = new OrderViewModel();
            OrderVM.Items = new List<Order>() { newItem };
            OrderVM.WriteAll();

            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();
            ProductInvoiceVM.Items = source;
            ProductInvoiceVM.WritefProductInvoicesOrder();

            ExitPage();
        }
        
        public void Reset()
        {
            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();
            ProductInvoiceVM.Items = getProductInvoiceOrderRepo().Gets();

            ProductInvoiceVM.Reset(newItem);

            newItem = null;
            ExitPage();
        }

        public void Reset(OrderDetail detail)
        {
            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();
            ProductInvoiceVM.Items = getProductInvoiceOrderRepo().Gets();

            ProductInvoiceVM.Reset(detail);
        }

        public void GetInfomation()
        {
            int no = getOrderRepo().Length();
            OrderViewModel OrderViewModel = new OrderViewModel();
            string id = OrderViewModel.GetId(no + 1);
            
            newItem.Id = id;
            newItem.UserCreated.Name = getAccount().Account.Username;
            newItem.Date = DateTime.Now;
        }

        private void ExitPage()
        {
            newItem = null;
            getFrmCashier().getUcPage().gdDisplay.Children.Clear();
            getFrmCashier().getUcPage().gdDisplay.Children.Add(this);
        }
    }
}
