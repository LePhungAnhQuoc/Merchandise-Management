using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmInvoice.xaml
    /// </summary>
    public partial class frmInvoice : Window, INotifyPropertyChanged
    {
        #region Getdata
        public Func<Invoice> getInvoice { get; set; }
        #endregion

        #region Fields
        private ucInvoiceDetails ucInvoiceDetails;
        #endregion

        #region Properties
        private Invoice _Invoice;
        public Invoice Invoice
        {
            get { return _Invoice; }
            set
            {
                _Invoice = value;
                OnPropertyChanged("Invoice");
            }
        }
        #endregion

        public frmInvoice()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Invoice = getInvoice();
            if (Invoice == null)
                return;
            lblInvoiceDate.Content = Invoice.Date.ToString(Constants.formatDate);
            
            ucInvoiceDetails = new ucInvoiceDetails();
            scrollDetails.Content = ucInvoiceDetails;

            var getList = new ObservableCollection<InvoiceDetail>(Invoice.Details.ListDetail);
            ucInvoiceDetails.getList = () => getList;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
