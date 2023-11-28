using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInvoice : INotifyPropertyChanged, ICloneable
    {
        public Product Product { get; set; }

        public double PreviousQuantity { get; set; }
        public Price PreviousAmount { get; set; }
        public DateTime PreviousDate { get; set; }

        public double RecentQuantity { get; set; }
        public Price RecentAmount { get; set; }
        public DateTime RecentDate { get; set; }

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

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            var result = this.MemberwiseClone() as ProductInvoice;
            result.Product = this.Product.Clone() as Product;

            result.PreviousAmount = this.PreviousAmount.Clone() as Price;
            result.RecentAmount = this.RecentAmount.Clone() as Price;
            result.TotalPrice = this.TotalPrice.Clone() as Price;

            return result;
        }
        #endregion

        public ProductInvoice()
        {
            Product = new Product();
            PreviousAmount = new Price();
            RecentAmount = new Price();
            TotalPrice = new Price();
        }
    }
}
