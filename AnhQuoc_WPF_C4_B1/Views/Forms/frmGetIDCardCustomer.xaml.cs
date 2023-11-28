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
    /// Interaction logic for frmGetIDCardCustomer.xaml
    /// </summary>
    public partial class frmGetIDCardCustomer : Window
    {
        #region GetData
        public Func<RepositoryBase<Customer>> getCustomerRepo;
        #endregion

        #region Properties
        public string txtIDCardInput { get; set; }

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

        public frmGetIDCardCustomer()
        {
            InitializeComponent();
            newItem = null;
            btnNewGuest.Click += BtnNewGuest_Click;
            btnConfirm.Click += BtnConfirm_Click;
            txtIdCard.PreviewTextInput += TxtIdCard_PreviewTextInput;
            txtIdCard.TextChanged += TxtIdCard_TextChanged;
            Loaded += FrmGetIDCardCustomer_Loaded;

            this.DataContext = this;
        }

        private void FrmGetIDCardCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdCard.Focus();
        }

        private void TxtIdCard_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtIdCard.Text == string.Empty)
            {
                btnNewGuest.Visibility = Visibility.Visible;
                btnConfirm.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnNewGuest.Visibility = Visibility.Collapsed;
                btnConfirm.Visibility = Visibility.Visible;
            }
        }

        private void TxtIdCard_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (Convert.ToChar(e.Text) == Utilities.EnterKey())
                {
                    if (txtIdCard.Text == string.Empty)
                    {
                        BtnNewGuest_Click(null, null);
                    }
                    else
                    {
                        BtnConfirm_Click(null, null);
                    }
                }
            }
            catch
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage());
                return;
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            #region IsCheckValidInput
            try
            {
                var idCardInput = Convert.ToInt64(txtIdCard.Text);
            }
            catch
            {
                MessageBox.Show(Utilities.GetCatchErorExceptionMessage(), string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            #endregion
            newItem = new Customer();

            CustomerViewModel customerVM = new CustomerViewModel();
            customerVM.Items = getCustomerRepo().Gets();

            Customer finded = customerVM.FindByIDCard(txtIdCard.Text);
            if (finded == null)
            {
                MessageBox.Show("Not found IDCard!", "", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                newItem = finded;
                newItem.IsGuest = false;
                this.Close();
                return;
            }
        }

        private void BtnNewGuest_Click(object sender, RoutedEventArgs e)
        {
            newItem = new Customer
            {
                IsGuest = true
            };
            this.Close();
        }
    }
}
