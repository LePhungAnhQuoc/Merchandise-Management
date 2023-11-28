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
    /// Interaction logic for ucOrderDetails.xaml
    /// </summary>
    public partial class ucOrderDetails : UserControl, INotifyPropertyChanged
    {
        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucOrderDetails), new UIPropertyMetadata("List Details of Order"));
        #endregion

        #region Properties
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

        private ObservableCollection<OrderDetail> _Source;
        public ObservableCollection<OrderDetail> Source
        {
            get { return _Source; }
            set
            {
                _Source = value;
                OnPropertyChanged("Source");
            }
        }

        #region GetData
        private Func<ObservableCollection<OrderDetail>> _getList;
        public Func<ObservableCollection<OrderDetail>> getList
        {
            get { return _getList; }
            set
            {
                _getList = value;
                OnListChanged();
            }
        }
        #endregion

        #endregion
       
        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucOrderDetails()
        {
            InitializeComponent();
            Loaded += ucOrderDetails_Loaded;
            this.DataContext = this;
        }

        private void ucOrderDetails_Loaded(object sender, RoutedEventArgs e)
        {
            OnListChanged();

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void OnListChanged()
        {
            if (getList == null)
                return;
            
            Source = getList();

            dgOrderDetails.ItemsSource = Source;

            OrderDetailViewModel OrderDetailViewModel = new OrderDetailViewModel();

            TotalQuantity = OrderDetailViewModel.GetTotalQuantity(Source.ToList());
            TotalPrice = OrderDetailViewModel.GetTotalPrice(Source.ToList());
        }
    }
}
