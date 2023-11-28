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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region GetData
        public Func<frmLogin> getFrmLogin;
        public Func<AccountInfo> getAccountInfo { get; set; }
        #endregion

        #region Fields
        public UnitOfWork unitOfWork;
        private frmLogin frmLogin;
        #endregion

        #region Methods
        public void GoToAccount(frmLogin frmLogin, AccountInfo account)
        {
            frmStocker frmStocker = new frmStocker();
            frmStocker.getFrmMain = () => this;
            frmStocker.getFrmLogin = () => frmLogin;

            frmCashier frmCashier = new frmCashier();
            frmCashier.getFrmMain = () => this;
            frmCashier.getFrmLogin = () => frmLogin;

            frmAdmin frmAdmin = new frmAdmin();
            frmAdmin.getFrmMain = () => this;
            frmAdmin.getfrmStocker = () => frmStocker;
            frmAdmin.getfrmCashier = () => frmCashier;
            frmAdmin.getFrmLogin = () => frmLogin;

            switch (account.Role)
            {
                case 1:
                    frmAdmin.Show();
                    break;
                case 2:
                    frmStocker.Show();
                    break;
                case 3:
                    frmCashier.Show();
                    break;
            }
        }
        #endregion

        public MainWindow()
        {
            #region ClearAllFiles
            // Utilities.ClearAllFile();
            #endregion

            unitOfWork = new UnitOfWork();
            InitializeComponent();

            Closed += MainWindow_Closed;
            Loaded += MainWindow_Loaded;

            unitOfWork.GetInventory.MinQuantity = 10;
            unitOfWork.GetImportInventory.MinQuantity = 10;

            ProductViewModel ProductVM = new ProductViewModel();
            ProductVM.ItemsByCatRepo = unitOfWork.GetRepositoryProducts;
            ProductVM.Items = ProductVM.ConvertTo1D();

            // Số lượng tài khoản là số lượng nhân viên trong công ty
            int numberOfEmployees = unitOfWork.GetRepositoryAccounts.Length();
            ProductVM.GetPriceOutput(numberOfEmployees);
            
            frmLogin = new frmLogin();
            frmLogin.getFrmMain = () => this;
            frmLogin.getAccountRepo = () => unitOfWork.GetRepositoryAccounts;

            this.Hide();
            frmLogin.Show();
        }
        
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
