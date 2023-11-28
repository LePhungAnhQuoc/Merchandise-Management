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
    /// Interaction logic for ucCreateReceipt.xaml
    /// </summary>
    public partial class ucCreateReceipt : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<AccountInfo> getAccount;
        public Func<Inventory> getInventory;
        public Func<Inventory> getImportInventory;
        public Func<RepositoryBase<Receipt>> getReceiptRepo;
        public Func<RepositoryBase<ProductByCategory>> getProductByCatRepo;
        #endregion

        #region Properties
        private Receipt _newItem;
        public Receipt newItem
        {
            get { return _newItem; }
            set
            {
                _newItem = value;
                OnPropertyChanged("newItem");
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

        public ucCreateReceipt()
        {
            InitializeComponent();
            Loaded += UcCreateReceipt_Loaded;
            this.DataContext = this;
            btnCreate.Click += BtnCreate_Click;
        }

        private void UcCreateReceipt_Loaded(object sender, RoutedEventArgs e)
        {
            // Cấp phát Item mới, mỗi khi Load Form
            newItem = new Receipt();
          
            CreateItem();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Kiem tra san pham cua danh sach cac mat hang
            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            ProductByCategoryViewModel.Items = getProductByCatRepo().Gets();
            if (ProductByCategoryViewModel.IsEmpty())
            {
                MessageBox.Show(Utilities.GetListEmptyMessage("Product"), "", MessageBoxButton.OK, MessageBoxImage.Information);
                ExitPage();
                return;
            }

            ucCreateReceiptDetails ucCreateReceiptDetails = new ucCreateReceiptDetails();
            ucCreateReceiptDetails.getProductByCatRepo = getProductByCatRepo;
            ucCreateReceiptDetails.getfrmStocker = getfrmStocker;
            ucCreateReceiptDetails.getucCreateReceipt = () => this;
            ucCreateReceiptDetails.getReceipt = () => newItem;

            getfrmStocker().getUcPage().gdDisplay.Children.Clear();
            getfrmStocker().getUcPage().gdDisplay.Children.Add(ucCreateReceiptDetails);
        }

        public void Save(List<ReceiptDetail> details)
        {
            #region Settings
            ReceiptDetailViewModel ReceiptDetailViewModel = new ReceiptDetailViewModel();
            ReceiptDetailViewModel.ItemList = newItem.Details;
            ReceiptDetailViewModel.GetListInfo();
            newItem.TotalQuantity = newItem.Details.TotalQuantity;
            newItem.TotalPrice = newItem.Details.TotalPrice;
            #endregion

            /*** Cập nhật trên giao diện ***/

            // Thêm Receipt
            getReceiptRepo().Add(newItem);
            
            Inventory Inventory = getInventory();
            Inventory ImportInventory = getImportInventory();

            // Cập nhật trong Inventory
            InventoryViewModel InventoryVM = new InventoryViewModel();
            InventoryVM.Item = Inventory;
            InventoryVM.Update(newItem);

            // Cập nhật trong Import Inventory
            InventoryVM.Item = ImportInventory;
            InventoryVM.UpdateImport(newItem);

            SaveData();
        }

        public void SaveData()
        {
            ReceiptViewModel ReceiptViewModel = new ReceiptViewModel();
            ReceiptViewModel.Items = new List<Receipt> { newItem };
            ReceiptViewModel.WriteAll();

            InventoryViewModel InventoryVM = new InventoryViewModel();
            InventoryVM.Item = getInventory();
            InventoryVM.WriteAll();

            InventoryVM.Item = getImportInventory();
            InventoryVM.WriteImportInventory();
        }

        public void CreateItem()
        {
            int no = getReceiptRepo().Length();
            ReceiptViewModel ReceiptViewModel = new ReceiptViewModel();
            string id = ReceiptViewModel.GetId(no + 1);

            newItem.Id = id;
            newItem.User.Name = getAccount().Account.Username;
            newItem.Date = DateTime.Now;
        }

        public void ExitPage()
        {
            newItem = null;
            getfrmStocker().getUcPage().gdDisplay.Children.Clear();
            getfrmStocker().getUcPage().gdDisplay.Children.Add(this);
        }
    }
}
