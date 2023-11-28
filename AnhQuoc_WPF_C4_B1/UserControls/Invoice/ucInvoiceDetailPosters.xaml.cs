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
    /// Interaction logic for ucInvoiceDetailPosters.xaml
    /// </summary>
    public partial class ucInvoiceDetailPosters : UserControl, INotifyPropertyChanged
    {
        #region getData
        public Func<ucCreateInvoiceDetails> getucCreateInvoiceDetails;
        #endregion

        #region DependencyProperties
        public bool IsRegisterEvent
        {
            get { return (bool)GetValue(IsRegisterEventProperty); }
            set { SetValue(IsRegisterEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRegisterEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRegisterEventProperty =
            DependencyProperty.Register("IsRegisterEvent", typeof(bool), typeof(ucInvoiceDetailPosters), new UIPropertyMetadata(true));
        #endregion

        #region Properties
        private Func<ObservableCollection<InvoiceDetail>> _getList;
        public Func<ObservableCollection<InvoiceDetail>> getList
        {
            get { return _getList; }
            set
            {
                _getList = value;
                AddToData();
            }
        }

        private ObservableCollection<InvoiceDetail> _Source;
        public ObservableCollection<InvoiceDetail> Source
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

        public ucInvoiceDetailPosters()
        {
            InitializeComponent();
            Loaded += ucInvoiceDetailPosters_Loaded;
            this.DataContext = this;
        }

        private void ucInvoiceDetailPosters_Loaded(object sender, RoutedEventArgs e)
        {
            AddToData();
        }

        public void AddToData()
        {
            if (getList != null)
            {
                Source = getList();
                wrapDetails.Children.Clear();
                foreach (var item in Source)
                {
                    ucInvoiceDetailPoster newUCItem = new ucInvoiceDetailPoster();

                    newUCItem.NoIndex = Source.IndexOf(item) + 1;
                    newUCItem.Item = item;
                    newUCItem.Margin = new Thickness(10);

                    if (IsRegisterEvent == true)
                        newUCItem.ButtonDeleteClick += NewUCItem_ButtonDeleteClick;
                    wrapDetails.Children.Add(newUCItem);
                }
            }
        }

        private void NewUCItem_ButtonDeleteClick(object sender, InvoiceDetailRoutedEventArgs e)
        {
            getucCreateInvoiceDetails().DeleteDetailItem(e.InvoiceDetail);
        }
    }
}
