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
    /// Interaction logic for ucProductInvoiceRevenue.xaml
    /// </summary>
    public partial class ucProductInvoiceRevenue : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<RepositoryBase<Order>> getOrderRepo { get; set; }
        #endregion

        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucProductInvoiceRevenue), new UIPropertyMetadata("List Products"));
        #endregion

        #region Properties
        private DateTime? _SelectedDate;
        public DateTime? SelectedDate
        {
            get
            {
                return _SelectedDate;
            }
            set
            {
                _SelectedDate = value;
                OnPropertyChanged("SelectedDate");
                Fillter(SelectedDate);
            }
        }

        private List<Order> _orders;
        public List<Order> orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                OnPropertyChanged("orders");

                // Lấy dữ liệu từ các Orders
                ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();

                List<ProductInvoice> productInvoices = ProductInvoiceVM.GetRevenueFromOrders(orders);
                ProductInvoiceVM.Items = productInvoices;

                ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
                var getList = ProductByCategoryViewModel.ConvertProductInvoices(ProductInvoiceVM.Items);
                Products = new ObservableCollection<ProductInvoiceByCategory>(getList);
            }
        }

        private ProductInvoice _Product;
        public ProductInvoice Product
        {
            get
            {
                return _Product;
            }
            set
            {
                _Product = value;

                if (_Product == null)
                {
                    Revenue = 0;
                    Profit = 0;
                    return;
                }

                ProductInvoiceViewModel ProductInvoiceViewModel = new ProductInvoiceViewModel();
                ProductInvoiceViewModel.Item = _Product;

                Price totalSold = ProductInvoiceViewModel.GetTotalSoldSingle();

                Revenue = ProductInvoiceViewModel.GetRevenueSingle(totalSold);
                Profit = ProductInvoiceViewModel.GetProfitSingle(totalSold);
            }
        }

        private ObservableCollection<ProductInvoiceByCategory> _Products;
        public ObservableCollection<ProductInvoiceByCategory> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged("Products");

                lbCategories.SelectedIndex = 0;
            }
        }

        private double _Revenue;
        public double Revenue
        {
            get { return _Revenue; }
            set
            {
                _Revenue = value;
                OnPropertyChanged("Revenue");
            }
        }

        private double _Profit;
        public double Profit
        {
            get { return _Profit; }
            set
            {
                _Profit = value;
                OnPropertyChanged("Profit");
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

        public ucProductInvoiceRevenue()
        {
            InitializeComponent();
            Loaded += UcListProduct_Loaded;
            lbCategories.SelectionChanged += lbCategories_SelectionChanged;
            btnClear.Click += BtnClear_Click;
            this.DataContext = this;
        }

        private void UcListProduct_Loaded(object sender, RoutedEventArgs e)
        {
            orders = getOrderRepo().Gets();
            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void lbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Product> getList = lbCategories.SelectedValue as List<Product>;
        }

        private void Fillter(DateTime? valuePicker)
        {
            if (valuePicker == null)
            {
                orders = getOrderRepo().Gets();
                return;
            }
            DateTime value2 = Convert.ToDateTime(valuePicker);
            OrderViewModel OrderViewModel = new OrderViewModel();
            OrderViewModel.Items = getOrderRepo().Gets();
            orders = OrderViewModel.FillByDate(value2);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            dateOrder.SelectedDate = null;
        }
    }
}
