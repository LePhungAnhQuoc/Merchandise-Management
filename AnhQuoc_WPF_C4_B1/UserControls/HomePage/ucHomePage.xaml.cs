using System;
using System.Collections.Generic;
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
    /// Interaction logic for ucHomePage.xaml
    /// </summary>
    public partial class ucHomePage : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<MainWindow> getFrmMain;
        #endregion

        #region Properties
        private int _nEmployees;
        public int nEmployees
        {
            get { return _nEmployees; }
            set
            {
                _nEmployees = value;
                OnPropertyChanged("nEmployees");
            }
        }

        private int _nProducts;
        public int nProducts
        {
            get { return _nProducts; }
            set
            {
                _nProducts = value;
                OnPropertyChanged("nProducts");
            }
        }

        private int _nCustomers;
        public int nCustomers
        {
            get { return _nCustomers; }
            set
            {
                _nCustomers = value;
                OnPropertyChanged("nCustomers");
            }
        }

        private int _nOutOfInventorys;
        public int nOutOfInventorys
        {
            get { return _nOutOfInventorys; }
            set
            {
                _nOutOfInventorys = value;
                OnPropertyChanged("nOutOfInventorys");
            }
        }

        private int _nAlmostOutOfInventorys;
        public int nAlmostOutOfInventorys
        {
            get { return _nAlmostOutOfInventorys; }
            set
            {
                _nAlmostOutOfInventorys = value;
                OnPropertyChanged("nAlmostOutOfInventorys");
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

        public ucHomePage()
        {
            InitializeComponent();
            Loaded += UcHomePage_Loaded;
            this.DataContext = this;
        }

        private void UcHomePage_Loaded(object sender, RoutedEventArgs e)
        {
            nEmployees = getFrmMain().unitOfWork.GetRepositoryAccounts.Length();

            ProductViewModel ProductViewModel = new ProductViewModel();
            ProductViewModel.ItemsByCatRepo = getFrmMain().unitOfWork.GetRepositoryProducts;
            nProducts = ProductViewModel.ConvertTo1D().Count;

            CustomerViewModel CustomerViewModel = new CustomerViewModel();
            CustomerViewModel.Items = getFrmMain().unitOfWork.GetRepositoryCustomers.Gets();
            nCustomers = CustomerViewModel.FillCustomer().Count;

            Inventory Inventory = getFrmMain().unitOfWork.GetInventory;
            ProductInventoryViewModel ProductInventoryViewModel = new ProductInventoryViewModel();
            ProductInventoryViewModel.ItemList.Items = getFrmMain().unitOfWork.GetRepositoryProductInventorys.Gets();
            nOutOfInventorys = ProductInventoryViewModel.FillOutOfInventory().Count;

            nAlmostOutOfInventorys = ProductInventoryViewModel.FillAlmostOutOfInventory(Inventory.MinQuantity).Count;
        }
    }
}
