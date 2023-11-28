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
    /// Interaction logic for ucInvoiceDetails.xaml
    /// </summary>
    public partial class ucInvoiceDetails : UserControl, INotifyPropertyChanged
    {
        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucInvoiceDetails), new UIPropertyMetadata("List Details of Invoice"));
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

        private Func<ObservableCollection<InvoiceDetail>> _getList;
        public Func<ObservableCollection<InvoiceDetail>> getList
        {
            get { return _getList; }
            set
            {
                _getList = value;
                OnListChanged();
            }
        }

        private ObservableCollection<InvoiceDetail> _Source;
        public ObservableCollection<InvoiceDetail> Source
        {
            get { return _Source; }
            set
            {
                _Source = value;
                OnPropertyChanged("Source");
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

        public ucInvoiceDetails()
        {
            InitializeComponent();
            Loaded += ucInvoiceDetails_Loaded;
            this.DataContext = this;
        }

        private void ucInvoiceDetails_Loaded(object sender, RoutedEventArgs e)
        {
            OnListChanged();

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void OnListChanged()
        {
            if (getList != null)
            {
                Source = getList();
                
                dgInvoiceDetails.ItemsSource = Source;

                InvoiceDetailViewModel InvoiceDetailViewModel = new InvoiceDetailViewModel();
                TotalQuantity = InvoiceDetailViewModel.GetTotalQuantity(Source.ToList());
                TotalPrice = InvoiceDetailViewModel.GetTotalPrice(Source.ToList());
            }
        }
    }
}
