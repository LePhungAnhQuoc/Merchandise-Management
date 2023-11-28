﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for frmOrderGeneral.xaml
    /// </summary>
    public partial class frmOrderGeneral : Window, INotifyPropertyChanged
    {
        #region Getdata
        public Func<Order> getOrder { get; set; }
        #endregion

        #region Fields
        private ucOrderDetailPosters ucOrderDetailPosters;
        #endregion

        #region Properties
        private Order _Order;
        public Order Order
        {
            get { return _Order; }
            set
            {
                _Order = value;
                OnPropertyChanged("Order");
            }
        }
        #endregion

        public frmOrderGeneral()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Order = getOrder();
            if (Order == null)
                return;
            lblOrderDate.Content = Order.Date.ToString(Constants.formatDate);
            
            ucOrderDetailPosters = new ucOrderDetailPosters();
            scrollDetails.Content = ucOrderDetailPosters;

            var getList = new ObservableCollection<OrderDetail>(Order.Details.ListDetail);
            ucOrderDetailPosters.IsRegisterEvent = false;
            ucOrderDetailPosters.getList = () => getList;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
