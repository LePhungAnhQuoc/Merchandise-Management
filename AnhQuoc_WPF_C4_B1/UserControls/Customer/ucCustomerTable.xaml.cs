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
    /// Interaction logic for ucCustomerTable.xaml
    /// </summary>
    public partial class ucCustomerTable : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<RepositoryBase<Customer>> getCustomerRepo;
        #endregion

        #region Fields

        private List<Customer> _Customers;
        public List<Customer> Customers
        {
            get
            {
                return _Customers;
            }
            set
            {
                _Customers = value;
                CustomersSource = new ObservableCollection<Customer>(_Customers);
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
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucCustomerTable), new UIPropertyMetadata("List Customers"));
        #endregion

        #region Properties
        private ObservableCollection<Customer> _CustomersSource;
        public ObservableCollection<Customer> CustomersSource
        {
            get { return _CustomersSource; }
            set
            {
                _CustomersSource = value;
                OnPropertyChanged("CustomersSource");

                dgCustomer.ItemsSource = _CustomersSource;
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
        #endregion

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucCustomerTable()
        {
            InitializeComponent();
            Loaded += ucCustomerTable_Loaded;
            this.DataContext = this;
        }
        
        private void ucCustomerTable_Loaded(object sender, RoutedEventArgs e)
        {
            Customers = getCustomerRepo().Gets();

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }
    }
}
