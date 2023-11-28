using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class Order: INotifyPropertyChanged
    {
        public string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private Customer _Customer;
        public Customer Customer
        {
            get { return _Customer; }
            set
            {
                _Customer = value;
                OnPropertyChanged("Customer");
            }
        }

        private User _UserCreated;
        public User UserCreated
        {
            get { return _UserCreated; }
            set
            {
                _UserCreated = value;
                OnPropertyChanged("UserCreated");
            }
        }

        private OrderDetailList _Details;
        public OrderDetailList Details
        {
            get { return _Details; }
            set
            {
                _Details = value;
                OnPropertyChanged("Details");
            }
        }

        private double _TotalQuantity;
        public double TotalQuantity
        {
            get { return _TotalQuantity; }
            set
            {
                _TotalQuantity = value;
                OnPropertyChanged("TotalQuantity");
            }
        }

        private Price _TotalPrice;
        public Price TotalPrice
        {
            get { return _TotalPrice; }
            set
            {
                _TotalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                OnPropertyChanged("Date");
            }
        }

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public Order()
        {
            Customer = new Customer();
            UserCreated = new User();
            TotalPrice = new Price();
            Details = new OrderDetailList();
        }
    }
}
