using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class OrderDetail : INotifyPropertyChanged
    {
        private string _Id;
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }

        private string _IdOrder;
        public string IdOrder
        {
            get
            {
                return _IdOrder;
            }
            set
            {
                _IdOrder = value;
                OnPropertyChanged();
            }
        }

        private Product _Product;
        public Product Product
        {
            get
            {
                return _Product;
            }
            set
            {
                _Product = value;
                OnPropertyChanged();
            }
        }

        private double _Quantity;
        public double Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
                OnPropertyChanged();
            }
        }

        public Price _TempPrice;
        public Price TempPrice
        {
            get
            {
                return _TempPrice;
            }
            set
            {
                _TempPrice = value;
                OnPropertyChanged();
            }
        }

        private double _PercentDiscount;
        public double PercentDiscount
        {
            get
            {
                return _PercentDiscount;
            }
            set
            {
                _PercentDiscount = value;
                OnPropertyChanged();
            }
        }

        private double _DiscountPrice;
        public double DiscountPrice
        {
            get
            {
                return _DiscountPrice;
            }
            set
            {
                _DiscountPrice = value;
                OnPropertyChanged();
            }
        }

        public Price _TotalPrice;
        public Price TotalPrice
        {
            get
            {
                return _TotalPrice;
            }
            set
            {
                _TotalPrice = value;
                OnPropertyChanged();
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                OnPropertyChanged();
            }
        }


        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public OrderDetail()
        {
            Product = new Product();
            TempPrice = new Price();
            PercentDiscount = 0.0;
            TotalPrice = new Price();
        }
    }
}
