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
    /// Interaction logic for ucListProductInvoiceGeneral.xaml
    /// </summary>
    public partial class ucListProductInvoiceGeneral : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<RepositoryBase<ProductInvoiceByCategory>> getProductInvoiceByCatRepo { get; set; }
        public Func<ucCreateOrderDetails> getucCreateOrderDetails { get; set; }
        #endregion

        #region Properties
        private ObservableCollection<ProductInvoiceByCategory> _Products;
        public ObservableCollection<ProductInvoiceByCategory> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged("Products");

                lbCategories.SelectedIndex = 0;
            }
        }
        #endregion

        #region DependencyProperties
        public double DataGridHeight
        {
            get { return (double)GetValue(DataGridHeightProperty); }
            set { SetValue(DataGridHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataGridHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataGridHeightProperty =
            DependencyProperty.Register("DataGridHeight", typeof(double), typeof(ucListProductInvoiceGeneral), new UIPropertyMetadata(400.0));


        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucListProductInvoiceGeneral), new UIPropertyMetadata("List Products"));
        #endregion

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ucListProductInvoiceGeneral()
        {
            InitializeComponent();  
            Loaded += UcListProduct_Loaded;
            dgProducts.SelectionChanged += DgProducts_SelectionChanged;
            this.DataContext = this;
        }

        private void DgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (getucCreateOrderDetails != null)
                getucCreateOrderDetails().getSelectedProduct = () => dgProducts.SelectedItem as ProductInvoice;
        }

        private void UcListProduct_Loaded(object sender, RoutedEventArgs e)
        {
            Products = new ObservableCollection<ProductInvoiceByCategory>(getProductInvoiceByCatRepo().Gets());

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }
    }
}
