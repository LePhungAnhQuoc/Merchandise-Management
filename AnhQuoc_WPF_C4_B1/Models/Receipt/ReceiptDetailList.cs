using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class ReceiptDetailList
    {
        public List<ReceiptDetail> ListDetail { get; set; }
        public double TotalQuantity { get; set; }
        public Price TotalPrice { get; set; }

		public int Count {
            get
            {
                if (ListDetail != null)
                    return ListDetail.Count;
                return 0;
            }
        }
        public ReceiptDetailList()
        {
            ListDetail = new List<ReceiptDetail>();
            TotalPrice = new Price();
        }
    }
}
