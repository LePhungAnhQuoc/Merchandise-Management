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
    /// Interaction logic for ucOrderRevenueTable.xaml
    /// </summary>
    public partial class ucOrderRevenueTable : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmCashier> getfrmCashier;
        public Func<RepositoryBase<Order>> getOrderRepo;
        #endregion

        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucOrderRevenueTable), new UIPropertyMetadata("List Orders"));
        #endregion

        #region Properties
        private List<Order> _Orders;
        public List<Order> Orders
        {
            get
            {
                return _Orders;
            }
            set
            {
                _Orders = value;
                OrdersSource = new ObservableCollection<Order>(_Orders);

                OrderViewModel OrderViewModel = new OrderViewModel();
                OrderViewModel.Items = _Orders;

                Price totalSold = OrderViewModel.GetTotalSold();

                Revenue = OrderViewModel.GetRevenue(totalSold);
                Profit = OrderViewModel.GetProfit(totalSold);
            }
        }

        private ObservableCollection<Order> _OrdersSource;
        public ObservableCollection<Order> OrdersSource
        {
            get { return _OrdersSource; }
            set
            {
                _OrdersSource = value;
                OnPropertyChanged("OrdersSource");

                dgOrder.ItemsSource = _OrdersSource;
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
        #endregion

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucOrderRevenueTable()
        {
            InitializeComponent();
            Loaded += ucOrderRevenueTable_Loaded;
            dgOrder.MouseDoubleClick += DgOrder_MouseDoubleClick;
            btnClear.Click += BtnClear_Click;
            this.DataContext = this;
        }

        private void ucOrderRevenueTable_Loaded(object sender, RoutedEventArgs e)
        {
            Orders = getOrderRepo().Gets();

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void Fillter(DateTime? valuePicker)
        {
            if (valuePicker == null)
            {
                Orders = getOrderRepo().Gets();
                return;
            }
            DateTime value2 = Convert.ToDateTime(valuePicker);
            OrderViewModel OrderViewModel = new OrderViewModel();
            OrderViewModel.Items = getOrderRepo().Gets();
            Orders = OrderViewModel.FillByDate(value2);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            dateOrder.SelectedDate = null;
        }

        private void DgOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnDetails_Click(null, null);
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            LoadFormOrderDetail();
        }

        private void LoadFormOrderDetail()
        {
            frmOrder frmOrder = new frmOrder();
            frmOrder.getOrder = () => dgOrder.SelectedItem as Order;
            frmOrder.Show();
        }
    }
}
