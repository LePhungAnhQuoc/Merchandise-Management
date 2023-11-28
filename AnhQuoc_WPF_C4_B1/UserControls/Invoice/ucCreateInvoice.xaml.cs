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
    /// Interaction logic for ucCreateInvoice.xaml
    /// </summary>
    public partial class ucCreateInvoice : UserControl, INotifyPropertyChanged
    {
        #region GetData
        public Func<frmStocker> getfrmStocker;
        public Func<AccountInfo> getAccount;
        public Func<Inventory> getInventory;
        public Func<RepositoryBase<ProductInventoryByCategory>> getProductInventoryByCatRepo;
        public Func<RepositoryBase<Invoice>> getInvoiceRepo;
        #endregion
        
        #region Properties
        private Invoice _newItem;
        public Invoice newItem
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

        public ucCreateInvoice()
        {
            InitializeComponent();
            Loaded += UcCreateInvoice_Loaded;
            this.DataContext = this;
            btnCreate.Click += BtnCreate_Click;
        }

        private void UcCreateInvoice_Loaded(object sender, RoutedEventArgs e)
        {
            // Cấp phát item mới mỗi khi Load Form Create
            newItem = new Invoice();
            
            CreateItem();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Kiem tra so luong san pham trong kho xuat con khong truoc khi load nghiep vu
            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            if (ProductByCategoryViewModel.IsEmpty(getProductInventoryByCatRepo().Gets()))
            {
                MessageBox.Show(Utilities.GetListEmptyMessage("Product", "Inventory"), "", MessageBoxButton.OK, MessageBoxImage.Information);
                ExitPage();
                return;
            }

            ucCreateInvoiceDetails ucCreateInvoiceDetails = new ucCreateInvoiceDetails();
            ucCreateInvoiceDetails.getProductInventoryByCatRepo = getProductInventoryByCatRepo;
            ucCreateInvoiceDetails.getfrmStocker = getfrmStocker;
            ucCreateInvoiceDetails.getucCreateInvoice = () => this;
            ucCreateInvoiceDetails.getInvoice = () => newItem;

            getfrmStocker().getUcPage().gdDisplay.Children.Clear();
            getfrmStocker().getUcPage().gdDisplay.Children.Add(ucCreateInvoiceDetails);
        }

        public void Reset()
        {
            InventoryViewModel inventoryVM = new InventoryViewModel();
            Inventory inventory = getInventory();
            inventoryVM.Item = inventory;
            inventoryVM.Reset(newItem);

            // Update totalQuantity and Price
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            inventory.TotalQuantity = productInventoryVM.GetTotalQuantity(inventory.Products);
            inventory.TotalPrice = productInventoryVM.GetTotalPrice(inventory.Products);

            newItem = null;
            ExitPage();
        }

        public void Reset(InvoiceDetail detail)
        {
            InventoryViewModel inventoryVM = new InventoryViewModel();
            Inventory inventory = getInventory();
            inventoryVM.Item = inventory;

            inventoryVM.Reset(detail);

            // Update totalQuantity and Price
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            inventory.TotalQuantity = productInventoryVM.GetTotalQuantity(inventory.Products);
            inventory.TotalPrice = productInventoryVM.GetTotalPrice(inventory.Products);
        }

        public void Save(List<InvoiceDetail> details, List<ProductInventoryByCategory> newList)
        {
            InvoiceDetailViewModel InvoiceDetailViewModel = new InvoiceDetailViewModel();
            InvoiceDetailViewModel.ItemList = newItem.Details;
            InvoiceDetailViewModel.GetListInfo();

            newItem.TotalQuantity = newItem.Details.TotalQuantity;
            newItem.TotalPrice = newItem.Details.TotalPrice;
            
            // *** Cập nhật trên giao diện ***

            // Thêm Invoice
            getInvoiceRepo().Add(newItem);

            // Cập nhật trong invoice inventory
            ProductInvoiceViewModel ProductInvoiceVM = new ProductInvoiceViewModel();
            ProductInvoiceVM.Items = getInventory().ProductInvoices;
            ProductInvoiceVM.Update(newItem);

            // Cập nhật trong invoice order inventory
            ProductInvoiceVM.Items = getInventory().ProductInvoicesOrder;
            ProductInvoiceVM.Update(newItem);

            Inventory Inventory = getInventory();
            InventoryViewModel InventoryVM = new InventoryViewModel();
            ProductInventoryViewModel ProductInventoryVM = new ProductInventoryViewModel();
            InventoryVM.Item = Inventory;

            // Cập nhật danh sách nguồn khi lưu dữ liệu
            var getListByCat = ConvertToProductsByCat(Inventory.Products);
            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            ProductByCategoryViewModel.UpdateListSource(getListByCat, newList);

            Inventory.TotalQuantity = ProductInventoryVM.GetTotalQuantity(Inventory.Products);
            Inventory.TotalPrice = ProductInventoryVM.GetTotalPrice(Inventory.Products);

            // *** Ghi dữ liệu xuống tập tin ***
            InvoiceViewModel InvoiceViewModel = new InvoiceViewModel();
            InvoiceViewModel.Items = new List<Invoice> { newItem };
            InvoiceViewModel.WriteAll();

            ProductInvoiceVM.Items = Inventory.ProductInvoices;
            ProductInvoiceVM.WriteAll();
            ProductInvoiceVM.Items = Inventory.ProductInvoicesOrder;
            ProductInvoiceVM.WritefProductInvoicesOrder();

            InventoryVM.Item = Inventory;
            InventoryVM.WriteAll();
        }
       
        public void CreateItem()
        {
            int no = getInvoiceRepo().Length();
            InvoiceViewModel InvoiceViewModel = new InvoiceViewModel();
            string id = InvoiceViewModel.GetId(no + 1);

            newItem = new Invoice();
            newItem.Id = id;
            newItem.User.Name = getAccount().Account.Username;
            newItem.Date = DateTime.Now;
        }

        private void ExitPage()
        {
            newItem = null;
            getfrmStocker().getUcPage().gdDisplay.Children.Clear();
            getfrmStocker().getUcPage().gdDisplay.Children.Add(this);
        }

        private List<ProductInventoryByCategory> ConvertToProductsByCat(List<ProductInventory> source)
        {
            ProductByCategoryViewModel ProductByCategoryViewModel = new ProductByCategoryViewModel();
            return ProductByCategoryViewModel.ConvertProductInventorys(source);
        }
    }
}
