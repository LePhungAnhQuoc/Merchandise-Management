using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AnhQuoc_WPF_C4_B1
{
    public interface IProductViewModel
    {
        List<string> GetFields(bool displayNo = false);
        List<int> GetLengths(bool displayNo = false);
        List<object> GetRecords(int no, Product item);
        void WriteItem(Product product, XmlNode newNode);
        Product LoadItem(XmlNode nodeItem);
    }
}
