using System;
using System.Collections.Generic;
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
    /// Interaction logic for ucOrderDetailPoster.xaml
    /// </summary>
    public partial class ucOrderDetailPoster : UserControl
    {
        #region DependencyProperties
        public int NoIndex
        {
            get { return (int)GetValue(NoIndexProperty); }
            set { SetValue(NoIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoIndexProperty =
            DependencyProperty.Register("NoIndex", typeof(int), typeof(ucOrderDetailPoster), new UIPropertyMetadata(-1));



        public OrderDetail Item
        {
            get { return (OrderDetail)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(OrderDetail), typeof(ucOrderDetailPoster), new UIPropertyMetadata(new OrderDetail()));


        #endregion

        #region Events
        public event EventHandler<OrderDetailRoutedEventArgs> btnDeleteClick;
        #endregion

        public ucOrderDetailPoster()
        {
            InitializeComponent();

            Loaded += UcOrderDetailPoster_Loaded;
            btnDelete.Click += BtnDelete_Click;
        }

        private void UcOrderDetailPoster_Loaded(object sender, RoutedEventArgs e)
        {
            if (btnDeleteClick == null)
            {
                stkButtons.Visibility = Visibility.Collapsed;
            }    
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeleteClick != null)
            {
                btnDeleteClick.Invoke(this, new OrderDetailRoutedEventArgs(Item));
            }
        }
    }

    public class OrderDetailRoutedEventArgs : RoutedEventArgs
    {
        public OrderDetail DetailItem { get; set; }
        public OrderDetailRoutedEventArgs(OrderDetail DetailItem)
        {
            this.DetailItem = DetailItem;
        }
    }
}
