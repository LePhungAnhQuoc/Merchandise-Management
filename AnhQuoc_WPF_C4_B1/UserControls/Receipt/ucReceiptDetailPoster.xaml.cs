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
    /// Interaction logic for ucReceiptDetailPoster.xaml
    /// </summary>
    public partial class ucReceiptDetailPoster : UserControl
    {
        #region DependencyProperties


        public int NoIndex
        {
            get { return (int)GetValue(NoIndexProperty); }
            set { SetValue(NoIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoIndexProperty =
            DependencyProperty.Register("NoIndex", typeof(int), typeof(ucReceiptDetailPoster), new UIPropertyMetadata(-1));



        public ReceiptDetail Item
        {
            get { return (ReceiptDetail)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(ReceiptDetail), typeof(ucReceiptDetailPoster), new UIPropertyMetadata(new ReceiptDetail()));


        #endregion

        #region Events
        public event RoutedEventHandler ButtonDeleteClick;
        #endregion

        public ucReceiptDetailPoster()
        {
            InitializeComponent();
            Loaded += UcReceiptDetailPoster_Loaded;
            btnDelete.Click += BtnDelete_Click;
        }

        private void UcReceiptDetailPoster_Loaded(object sender, RoutedEventArgs e)
        {
            if (ButtonDeleteClick == null)
            {
                stkDeleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonDeleteClick != null)
            {
                ButtonDeleteClick.Invoke(sender, new ReceiptDetailRoutedEventHandler(Item));
            }
        }
    }

    public class ReceiptDetailRoutedEventHandler: RoutedEventArgs
    {
        public ReceiptDetail ReceiptDetail { get; set; }
        public ReceiptDetailRoutedEventHandler(ReceiptDetail ReceiptDetail)
        {
            this.ReceiptDetail = ReceiptDetail;
        }
    }
}
