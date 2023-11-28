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
    /// Interaction logic for ucOrderTable.xaml
    /// </summary>
    public partial class ucOrderTable : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmCashier> getfrmCashier;
        public Func<RepositoryBase<Order>> getOrderRepo;
        #endregion

        #region Fields

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
            }
        }
        #endregion

        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucOrderTable), new UIPropertyMetadata("List Orders"));
        #endregion

        #region Properties
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

        private double _TotalQuantity;
        public double TotalQuantity
        {
            get { return _TotalQuantity; }
            set
            {
                _TotalQuantity = value;
                OnPropertyChanged("TotalQuantity");
            }
        }

        private Price _TotalPrice;
        public Price TotalPrice
        {
            get { return _TotalPrice; }
            set
            {
                _TotalPrice = value;
                OnPropertyChanged("TotalPrice");
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

        public ucOrderTable()
        {
            InitializeComponent();
            Loaded += ucOrderTable_Loaded;            
            btnClear.Click += BtnClear_Click;
            dgOrder.MouseDoubleClick += DgOrder_MouseDoubleClick;
            this.DataContext = this;
        }

        private void ucOrderTable_Loaded(object sender, RoutedEventArgs e)
        {
            Orders = getOrderRepo().Gets();

            OrderViewModel OrderViewModel = new OrderViewModel();
            TotalQuantity = OrderViewModel.GetTotalQuantity(Orders);
            TotalPrice = OrderViewModel.GetTotalPrice(Orders);

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void DgOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnDetails_Click(null, null);
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

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            LoadFormOrderDetail();
        }

        public void LoadFormOrderDetail()
        {
            frmOrder frmOrder = new frmOrder();
            frmOrder.getOrder = () => dgOrder.SelectedItem as Order;
            frmOrder.Show();
        }
    }
}
