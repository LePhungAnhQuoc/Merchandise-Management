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
    /// Interaction logic for ucReceiptTable.xaml
    /// </summary>
    public partial class ucReceiptTable : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmAdmin> getfrmAdmin;
        public Func<frmStocker> getfrmStocker;
        public Func<RepositoryBase<Receipt>> getReceiptRepo;
        #endregion
        
        #region Fiels
        private List<Receipt> receipts;
        private frmReceipt frmReceipt;
        #endregion

        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucReceiptTable), new UIPropertyMetadata("List Receipts"));
        #endregion

        #region Properties
        private ObservableCollection<Receipt> _ReceiptsSource;
        public ObservableCollection<Receipt> ReceiptsSource
        {
            get { return _ReceiptsSource; }
            set
            {
                _ReceiptsSource = value;
                OnPropertyChanged("ReceiptSource");
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

        public ucReceiptTable()
        {
            InitializeComponent();
            Loaded += ucReceiptTable_Loaded;
            dgReceipt.MouseDoubleClick += DgReceipt_MouseDoubleClick;
            this.DataContext = this;
        }

        private void ucReceiptTable_Loaded(object sender, RoutedEventArgs e)
        {
            receipts = getReceiptRepo().Gets();
            ReceiptsSource = new ObservableCollection<Receipt>(receipts);

            AddToDatagrid();

            frmReceipt = new frmReceipt();
            
            ReceiptViewModel ReceiptViewModel = new ReceiptViewModel();
            TotalQuantity = ReceiptViewModel.GetTotalQuantity(receipts);
            TotalPrice = ReceiptViewModel.GetTotalPrice(receipts);
            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void AddToDatagrid()
        {
            dgReceipt.ItemsSource = ReceiptsSource;
        }

        private void DgReceipt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnDetails_Click(null, null);
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            Receipt selectItem = dgReceipt.SelectedItem as Receipt;
            
            frmReceipt frmReceipt = new frmReceipt();
            frmReceipt.getReceipt = () => selectItem;
            frmReceipt.Show();
        }
    }
}
