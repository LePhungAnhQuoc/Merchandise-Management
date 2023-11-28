using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ucDetailsByIdInvoice.xaml
    /// </summary>
    public partial class ucDetailsByIdInvoice : UserControl
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<RepositoryBase<Invoice>> getInvoiceRepo;
        #endregion

        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucDetailsByIdInvoice), new UIPropertyMetadata("Search Invoice Details by Id"));
        #endregion

        #region Fiels
        private ObservableCollection<InvoiceDetail> InvoiceDetails;
        #endregion

        public ucDetailsByIdInvoice()
        {
            InitializeComponent();
            btnSearch.Click += BtnSearch_Click1;
            Loaded += UcDetailsByIdInvoice_Loaded;
            txtIdInvoice.PreviewTextInput += TxtIdInvoice_PreviewTextInput;
        }

        private void UcDetailsByIdInvoice_Loaded(object sender, RoutedEventArgs e)
        {
            InvoiceDetails = new ObservableCollection<InvoiceDetail>();
            ucInvoiceDetails ucInvoiceDetails = new ucInvoiceDetails();
            ucInvoiceDetails.getList = () => InvoiceDetails;
            scrollDetails.Content = ucInvoiceDetails;

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtIdInvoice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\r")
            {
                BtnSearch_Click1(null, null);
            }
        }
        
        private void BtnSearch_Click1(object sender, RoutedEventArgs e)
        {
            string idInvoice = txtIdInvoice.Text;

            InvoiceViewModel InvoiceViewModel = new InvoiceViewModel();
            InvoiceViewModel.Items = getInvoiceRepo().Gets();

            Invoice newItem = InvoiceViewModel.FindById(idInvoice);
            if (newItem == null)
            {
                MessageBox.Show("Can not find invoice");
                return;
            }
            InvoiceDetails = new ObservableCollection<InvoiceDetail>(newItem.Details.ListDetail);

            ucInvoiceDetails ucInvoiceDetails = new ucInvoiceDetails();
            ucInvoiceDetails.getList = () => InvoiceDetails;
            scrollDetails.Content = ucInvoiceDetails;
        }
    }
}
