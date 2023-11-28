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
    /// Interaction logic for frmCustomerInfomation.xaml
    /// </summary>
    public partial class frmCustomerInfomation : Window, INotifyPropertyChanged
    {
        #region GetData
        public Func<Customer> getCustomer;
        #endregion

        #region Properties
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

        public frmCustomerInfomation()
        {
            InitializeComponent();
            this.DataContext = this;
            newItem = new Customer();

            btnBack.Click += BtnBack_Click;
            Loaded += FrmCustomerInfomation_Loaded;
        }

        private void FrmCustomerInfomation_Loaded(object sender, RoutedEventArgs e)
        {
            btnBack.Focus();

            newItem = getCustomer();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
