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
    /// Interaction logic for ucDetailsByIdOrder.xaml
    /// </summary>
    public partial class ucDetailsByIdOrder : UserControl
    {
        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucDetailsByIdOrder), new UIPropertyMetadata("Search Order Details by Id"));
        #endregion

        #region GetData
        public Func<frmCashier> getfrmCashier;
        public Func<RepositoryBase<Order>> getOrderRepo;
        #endregion

        #region Fiels
        private ObservableCollection<OrderDetail> OrderDetails;
        #endregion

        public ucDetailsByIdOrder()
        {
            InitializeComponent();
            btnSearch.Click += BtnSearch_Click1;
            Loaded += ucDetailsByIdOrder_Loaded;
            txtIdOrder.PreviewTextInput += TxtIdOrder_PreviewTextInput;
        }

        private void ucDetailsByIdOrder_Loaded(object sender, RoutedEventArgs e)
        {
            OrderDetails = new ObservableCollection<OrderDetail>();
            ucOrderDetails ucOrderDetails = new ucOrderDetails();
            ucOrderDetails.getList = () => OrderDetails;
            scrollDetails.Content = ucOrderDetails;

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtIdOrder_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\r")
            {
                BtnSearch_Click1(null, null);
            }
        }
        
        private void BtnSearch_Click1(object sender, RoutedEventArgs e)
        {
            string idOrder = txtIdOrder.Text;

            OrderViewModel OrderViewModel = new OrderViewModel();
            OrderViewModel.Items = getOrderRepo().Gets();

            Order newItem = OrderViewModel.FindById(idOrder);
            if (newItem == null)
            {
                MessageBox.Show("Can not find Order");
                return;
            }
            OrderDetails = new ObservableCollection<OrderDetail>(newItem.Details.ListDetail);

            ucOrderDetails ucOrderDetails = new ucOrderDetails();
            ucOrderDetails.getList = () => OrderDetails;
            scrollDetails.Content = ucOrderDetails;
        }
    }
}
