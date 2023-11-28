using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class AccountViewModel
    {
        #region Properties
        public RepositoryBase<AccountInfo> Repo { get; set; }
        public AccountInfo Item { get; set; }
        #endregion

        public AccountViewModel()
        {
            Repo = new RepositoryBase<AccountInfo>();
        }

        public List<Account> ConvertListAccount()
        {
            List<Account> lstAcc = new List<Account>();

            foreach (AccountInfo item in Repo.Gets())
            {
                lstAcc.Add(item.Account);
            }
            return lstAcc;
        }

        public AccountInfo Find(Account newAcc)
        {
            foreach (AccountInfo item in Repo.Gets())
            {
                if (newAcc.Username == item.Account.Username
                 && newAcc.Password == item.Account.Password)
                    return item;
            }
            return null;
        }

        public RepositoryBase<AccountInfo> LoadItems(XmlNodeList lstNode)
        {
            Repo = new RepositoryBase<AccountInfo>();
            foreach (XmlNode nodeData in lstNode)
            {
                var value = LoadItem(nodeData);
                Repo.Gets().Add(value);
            }
            return Repo;
        }

        public AccountInfo LoadItem(XmlNode nodeData)
        {
            XmlNode tempNode = null;
            AccountInfo newAcc = new AccountInfo();

            tempNode = nodeData.FirstChild;
            newAcc.Id = tempNode.InnerText;

            tempNode = tempNode.NextSibling;
            newAcc.Account.Username = tempNode.InnerText;

            tempNode = tempNode.NextSibling;
            newAcc.Account.Password = tempNode.InnerText;

            tempNode = tempNode.NextSibling;
            newAcc.Role = Convert.ToInt32(tempNode.InnerText);

            tempNode = tempNode.NextSibling;
            newAcc.Status = Convert.ToBoolean(tempNode.InnerText);
            return newAcc;
        }

        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fAccounts);
            LoadItems(DataProvider.Instance.nodeRoot.ChildNodes);
            DataProvider.Instance.Close();
        }
    }
}
