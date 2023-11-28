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
    /// Interaction logic for ucOrderDetailPosters.xaml
    /// </summary>
    public partial class ucOrderDetailPosters : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<ucCreateOrderDetails> getucCreateOrderDetails;
        #endregion

        #region DependencyProperties
        public bool IsRegisterEvent
        {
            get { return (bool)GetValue(IsRegisterEventProperty); }
            set { SetValue(IsRegisterEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRegisterEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRegisterEventProperty =
            DependencyProperty.Register("IsRegisterEvent", typeof(bool), typeof(ucOrderDetailPosters), new UIPropertyMetadata(true));
        #endregion

        #region Properties
        private Func<ObservableCollection<OrderDetail>> _getList;
        public Func<ObservableCollection<OrderDetail>> getList
        {
            get { return _getList; }
            set
            {
                _getList = value;
                AddToData();
            }
        }

        private ObservableCollection<OrderDetail> _Source;
        public ObservableCollection<OrderDetail> Source
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

        public ucOrderDetailPosters()
        {
            InitializeComponent();
            Loaded += ucOrderDetailPosters_Loaded;
            this.DataContext = this;
        }

        private void ucOrderDetailPosters_Loaded(object sender, RoutedEventArgs e)
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
                    ucOrderDetailPoster newUCItem = new ucOrderDetailPoster();

                    newUCItem.NoIndex = Source.IndexOf(item) + 1;
                    newUCItem.Item = item;
                    newUCItem.Margin = new Thickness(20);

                    if (IsRegisterEvent == true)
                        newUCItem.btnDeleteClick += NewUCItem_btnDeleteClick;
                    wrapDetails.Children.Add(newUCItem);
                }
            }
        }
        
        private void NewUCItem_btnDeleteClick(object sender, OrderDetailRoutedEventArgs e)
        {
            getucCreateOrderDetails().DeleteDetailItem(e.DetailItem);
        }
    }
}
