using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmReceipt.xaml
    /// </summary>
    public partial class frmReceipt : Window, INotifyPropertyChanged
    {
        #region Getdata
        public Func<Receipt> getReceipt { get; set; }
        #endregion

        #region Fields
        private ucReceiptDetails ucReceiptDetails;
        #endregion

        #region Properties
        private Receipt _Receipt;
        public Receipt Receipt
        {
            get { return _Receipt; }
            set
            {
                _Receipt = value;
                OnPropertyChanged("Receipt");
            }
        }
        #endregion

        public frmReceipt()
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
            Receipt = getReceipt();
            if (Receipt == null)
                return;
            lblReceiptDate.Content = Receipt.Date.ToString(Constants.formatDate);
            
            ucReceiptDetails = new ucReceiptDetails();
            scrollDetails.Content = ucReceiptDetails;

            var getList = new ObservableCollection<ReceiptDetail>(Receipt.Details.ListDetail);
            ucReceiptDetails.getList = () => getList;
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
