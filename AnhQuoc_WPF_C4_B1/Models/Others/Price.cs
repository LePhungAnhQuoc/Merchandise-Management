using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class Price: INotifyPropertyChanged, ICloneable
    {
        private double _In;  // PriceInput
        public double In
        {
            get
            {
                return _In;
            }
            set
            {
                _In = value;
                OnPropertyChanged("In");
            }
        }

        private double _Out;// PriceOutput
        public double Out
        {
            get
            {
                return _Out;
            }
            set
            {
                _Out = value;
                OnPropertyChanged("Out");
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
            var result = this.MemberwiseClone() as Price;
            return result;
        }

        public Price()
        {
        }
        public Price(double priceInput, double priceOutput)
        {
            In = priceInput;
            Out = priceOutput;
        }
    }
}
