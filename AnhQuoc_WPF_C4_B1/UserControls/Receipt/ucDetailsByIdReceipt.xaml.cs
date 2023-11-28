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
    /// Interaction logic for ucDetailsByIdReceipt.xaml
    /// </summary>
    public partial class ucDetailsByIdReceipt : UserControl
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<RepositoryBase<Receipt>> GetReceiptRepo;
        #endregion

        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucDetailsByIdReceipt), new UIPropertyMetadata("Search Receipt Details by Id"));
        #endregion

        #region Fields
        private ObservableCollection<ReceiptDetail> ReceiptDetails;
        #endregion

        public ucDetailsByIdReceipt()
        {
            InitializeComponent();
            btnSearch.Click += BtnSearch_Click;
            Loaded += UcDetailsByIdReceipt_Loaded;
            txtIdReceipt.PreviewKeyDown += TxtIdReceipt_PreviewKeyDown;
        }

        private void UcDetailsByIdReceipt_Loaded(object sender, RoutedEventArgs e)
        {
            ReceiptDetails = new ObservableCollection<ReceiptDetail>();
            ucReceiptDetails ucReceiptDetails = new ucReceiptDetails();
            ucReceiptDetails.getList = () => ReceiptDetails;
            scrollDetails.Content = ucReceiptDetails;

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtIdReceipt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch_Click(null, null);
            }
        }
        
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string idReceipt = txtIdReceipt.Text;

            ReceiptViewModel ReceiptViewModel = new ReceiptViewModel();
            ReceiptViewModel.Items = GetReceiptRepo().Gets();

            Receipt newItem = ReceiptViewModel.FindById(idReceipt);
            if (newItem == null)
            {
                MessageBox.Show("Can not find receipt");
                return;
            }

            ReceiptDetails = new ObservableCollection<ReceiptDetail>(newItem.Details.ListDetail);

            ucReceiptDetails ucReceiptDetails = new ucReceiptDetails();
            ucReceiptDetails.getList = () => ReceiptDetails;
            scrollDetails.Content = ucReceiptDetails;
        }
    }
}
