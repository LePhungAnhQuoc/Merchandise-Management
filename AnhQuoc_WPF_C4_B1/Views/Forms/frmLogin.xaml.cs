using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmLogin : Window, INotifyPropertyChanged
    {
        #region GetData
        public Func<MainWindow> getFrmMain;
        public Func<RepositoryBase<AccountInfo>> getAccountRepo;
        #endregion

        #region Fields
        private AccountViewModel AccountVM;
        private AccountInfo findedAccount;
        #endregion

        #region Properties
        private Account _Account;
        public Account Account
        {
            get
            { 
                return _Account;
            }
            set
            {
                _Account = value;
                OnPropertyChanged("Account");
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

        public frmLogin()
        {
            InitializeComponent();
            AccountVM = new AccountViewModel();

            this.DataContext = this;
            Closed += FrmLogin_Closed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccountVM.Repo = getAccountRepo();
            txtUsername.Focus();

            Account = new Account();
           // btnSignIn_Click(null, null);
        }
        
        private void FrmLogin_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        #region Methods
        public void ClearLogin()
        {
            txtUsername.Clear();
            boxPassword.Clear();
            txtUsername.Focus();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            Account.Password = boxPassword.Password;
          //  findedAccount = AccountVM.Repo.GetByIndex(01);
            findedAccount = AccountVM.Find(Account);

            if (findedAccount == null)
            {
                MessageBox.Show("Incorrect username or password", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                ClearLogin();
                return;
            }
            if (findedAccount.Status == false)
            {
                MessageBox.Show("This account is already locked!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                ClearLogin();
            }
            else
            {
                this.Hide();
                getFrmMain().getAccountInfo = () => findedAccount;
                getFrmMain().GoToAccount(this, findedAccount);
            }
        }
        
        private bool IsCheckEnter(TextCompositionEventArgs e)
        {
            char enterKey = Convert.ToChar(13);
            if (e.Text == enterKey.ToString())
            {
                return true;
            }
            return false;
        }

        private void boxPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsCheckEnter(e))
            {
                btnSignIn_Click(this, null);
            }
        }
        #endregion
    }
}
