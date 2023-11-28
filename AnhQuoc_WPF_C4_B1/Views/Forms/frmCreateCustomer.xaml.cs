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
using System.Windows.Shapes;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmCreateCustomer.xaml
    /// </summary>
    public partial class frmCreateCustomer : Window
    {
        #region GetData
        public Func<RepositoryBase<Customer>> getCustomerRepo;
        #endregion

        #region Properties
        public string txtIdCardInput { get; set; }
        public string txtNameInput { get; set; }
        public string txtPhoneInput { get; set; }

        private Customer _newItem;
        public Customer newItem
        {
            get
            {
                return _newItem;
            }
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

        public frmCreateCustomer()
        {
            InitializeComponent();
            
            // Register event
            btnCancel.Click += BtnCancel_Click;
            btnCreate.Click += BtnCreate_Click;
            Loaded += FrmCreateCustomer_Loaded;

            // Receive data
            this.DataContext = this;

            // Settings
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // TabIndex
            txtIdCard.Focus();
            txtIdCard.TabIndex = 0;
            txtName.TabIndex = 1;
            txtPhone.TabIndex = 2;
            btnCreate.TabIndex = 3;
            btnCancel.TabIndex = 4;
        }
       
        private void FrmCreateCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            // Allocate
            newItem = new Customer();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            #region IsCheckEmpty
            if (txtIdCard.Text == string.Empty
                || txtName.Text == string.Empty
                || txtPhone.Text == string.Empty)
            {
                MessageBox.Show(Utilities.GetFormEmptyMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            #endregion

            #region IsCheckValidInput
            try
            {
                var IDCardInput = Convert.ToInt64(txtIdCard.Text);
                var phoneInput = Convert.ToInt32(txtPhone.Text);
            }
            catch
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (txtName.Text.All((c) => c == ' ' || char.IsLetter(c)) == false)
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            #endregion

            newItem.IDCard = txtIdCard.Text;
            // Validate IsExit ID Card
            CustomerViewModel CustomerVM = new CustomerViewModel();
            CustomerVM.Items = getCustomerRepo().Gets();

            Customer itemFinded = CustomerVM.FindByIDCard(newItem.IDCard);
            if (itemFinded == null)
            {
                newItem.Name = txtName.Text;
                newItem.Phone = txtPhone.Text;

                newItem.Point = 0.0;
                newItem.Card = CardType.None;

                this.Close();
                return;
            }
            else
            {
                MessageBox.Show(Utilities.GetIsExistMessage(true, "IDCard id"), "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            newItem = null;
            this.Close();
        }
    }
}
