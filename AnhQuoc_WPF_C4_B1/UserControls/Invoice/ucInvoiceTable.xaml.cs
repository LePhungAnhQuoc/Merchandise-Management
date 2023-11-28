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
    /// Interaction logic for ucInvoiceTable.xaml
    /// </summary>
    public partial class ucInvoiceTable : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<RepositoryBase<Invoice>> getInvoiceRepo;
        #endregion

        #region Fields
        private List<Invoice> invoices;
        #endregion

        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucInvoiceTable), new UIPropertyMetadata("List Invoices"));
        #endregion

        #region Properties
        private ObservableCollection<Invoice> _InvoicesSource;
        public ObservableCollection<Invoice> InvoicesSource
        {
            get { return _InvoicesSource; }
            set
            {
                _InvoicesSource = value;
                OnPropertyChanged("InvoicesSource");
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

        public ucInvoiceTable()
        {
            InitializeComponent();
            Loaded += ucInvoiceTable_Loaded;
            dgInvoice.MouseDoubleClick += DgInvoice_MouseDoubleClick;
            this.DataContext = this;
        }

        private void ucInvoiceTable_Loaded(object sender, RoutedEventArgs e)
        {
            invoices = getInvoiceRepo().Gets();
            InvoicesSource = new ObservableCollection<Invoice>(invoices);

            dgInvoice.ItemsSource = InvoicesSource;
            
            InvoiceViewModel InvoiceViewModel = new InvoiceViewModel();
            TotalQuantity = InvoiceViewModel.GetTotalQuantity(invoices);
            TotalPrice = InvoiceViewModel.GetTotalPrice(invoices);

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void DgInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnDetails_Click(null, null);
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            LoadFormInvoiceDetail();
        }

        private void LoadFormInvoiceDetail()
        {
            frmInvoice frmInvoice = new frmInvoice();
            frmInvoice.getInvoice = () => dgInvoice.SelectedItem as Invoice;
            frmInvoice.Show();
        }
    }
}
