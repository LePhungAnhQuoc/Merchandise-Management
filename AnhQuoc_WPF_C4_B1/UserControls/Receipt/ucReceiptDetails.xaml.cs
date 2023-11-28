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
    /// Interaction logic for ucReceiptDetails.xaml
    /// </summary>
    public partial class ucReceiptDetails : UserControl, INotifyPropertyChanged
    {
        #region DependencyProperties
        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucReceiptDetails), new UIPropertyMetadata("List Details by Receipt"));
        #endregion

        #region Properties
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

        private Func<ObservableCollection<ReceiptDetail>> _getList;
        public Func<ObservableCollection<ReceiptDetail>> getList
        {
            get { return _getList; }
            set
            {
                _getList = value;
                AddToData();
            }
        }

        private ObservableCollection<ReceiptDetail> _Source;
        public ObservableCollection<ReceiptDetail> Source
        {
            get { return _Source; }
            set
            {
                _Source = value;
                OnPropertyChanged("Source");
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

        public ucReceiptDetails()
        {
            InitializeComponent();
            Loaded += UcReceiptDetails_Loaded;
            this.DataContext = this;
        }

        private void UcReceiptDetails_Loaded(object sender, RoutedEventArgs e)
        {
            AddToData();

            if (lblHeader == string.Empty)
            {
                lblHeaderUI.Visibility = Visibility.Collapsed;
            }
        }

        public void AddToData()
        {
            if (getList != null)
            {
                Source = getList();
                
                dgReceiptDetails.ItemsSource = Source;

                ReceiptDetailViewModel ReceiptDetailViewModel = new ReceiptDetailViewModel();
                TotalQuantity = ReceiptDetailViewModel.GetTotalQuantity(Source.ToList());
                TotalPrice = ReceiptDetailViewModel.GetTotalPrice(Source.ToList());
            }
        }
    }
}
