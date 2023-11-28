﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class InvoiceDetail : INotifyPropertyChanged
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

        private string _IdInvoice;
        public string IdInvoice
        {
            get
            {
                return _IdInvoice;
            }
            set
            {
                _IdInvoice = value;
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

        public InvoiceDetail()
        {
            Product = new Product();
            TotalPrice = new Price();
        }
    }
}
