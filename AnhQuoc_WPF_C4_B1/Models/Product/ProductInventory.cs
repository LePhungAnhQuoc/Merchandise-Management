using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductInventory: INotifyPropertyChanged, ICloneable
    {
        private Product _Product;
        public Product Product
        {
            get { return _Product; }
            set
            {
                _Product = value;
                OnPropertyChanged("Product");
            }
        }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private Price _Price;
        public Price Price
        {
            get { return _Price; }
            set
            {
                _Price = value;
                OnPropertyChanged("Price");
            }
        }

        #region PropertyNotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public object Clone()
        {
            var result = this.MemberwiseClone() as ProductInventory;
            result.Product = this.Product.Clone() as Product;
            result.Price = this.Price.Clone() as Price;

            return result;
        }

        public ProductInventory()
        {
            Product = new Product();
            Price = new Price();
        }
    }
}
