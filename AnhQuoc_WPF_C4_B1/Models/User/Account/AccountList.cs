using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class AccountList
    {
        public List<AccountInfo> Items { get; set; }
        
        public AccountList()
        {
            Items = new List<AccountInfo>();
        }
    }
}
