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
    /// Interaction logic for ucInvoiceDetailPoster.xaml
    /// </summary>
    public partial class ucInvoiceDetailPoster : UserControl
    {
        #region DependencyProperties


        public int NoIndex
        {
            get { return (int)GetValue(NoIndexProperty); }
            set { SetValue(NoIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoIndexProperty =
            DependencyProperty.Register("NoIndex", typeof(int), typeof(ucInvoiceDetailPoster), new UIPropertyMetadata(-1));



        public InvoiceDetail Item
        {
            get { return (InvoiceDetail)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(InvoiceDetail), typeof(ucInvoiceDetailPoster), new UIPropertyMetadata(new InvoiceDetail()));


        #endregion

        public event EventHandler<InvoiceDetailRoutedEventArgs> ButtonDeleteClick;
        public ucInvoiceDetailPoster()
        {
            InitializeComponent();
            Loaded += UcInvoiceDetailPoster_Loaded;
            btnDelete.Click += BtnDelete_Click;
        }

        private void UcInvoiceDetailPoster_Loaded(object sender, RoutedEventArgs e)
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
                ButtonDeleteClick.Invoke(this, new InvoiceDetailRoutedEventArgs(Item));
            }
        }
    }
    public class InvoiceDetailRoutedEventArgs: RoutedEventArgs
    {
        public InvoiceDetail InvoiceDetail { get; set; }
        public InvoiceDetailRoutedEventArgs(InvoiceDetail InvoiceDetail)
        {
            this.InvoiceDetail = InvoiceDetail;
        }
    }
}
