using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    public class InventoryViewModel
    {
        public Inventory Item { get; set; }
        public List<Product> Products { get; set; }

        public void Update(Receipt receipt)
        {
            ProductInventoryStatusViewModel productInventoryStatusVM = new ProductInventoryStatusViewModel();
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            productInventoryStatusVM.Items = Item.ProductsStatus;
            productInventoryStatusVM.Update(receipt);

            var itemsInventory = productInventoryVM.ConvertTo(productInventoryStatusVM.Items);

            Item.Products.Clear();
            Item.Products.AddRange(itemsInventory);

            Item.TotalQuantity = productInventoryVM.GetTotalQuantity(Item.Products);
            Item.TotalPrice = productInventoryVM.GetTotalPrice(Item.Products);
        }

        public void UpdateImport(Receipt receipt)
        {
            Update(receipt);
        }
        public void UpdateImport(List<Receipt> lstReceipt)
        {
            foreach (Receipt item in lstReceipt)
            {
                UpdateImport(item);
            }
        }

        public void Update(List<Receipt> lstReceipt)
        {
            foreach (Receipt item in lstReceipt)
            {
                Update(item);
            }
        }

        public void Reset(Invoice newInvoice)
        {
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            productInventoryVM.ItemList.Items = Item.Products;
            foreach (InvoiceDetail item in newInvoice.Details.ListDetail)
            {
                Reset(item);
            }
        }

        public void Reset(InvoiceDetail invoiceDetail)
        {
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            productInventoryVM.ItemList.Items = Item.Products;

            ProductInventory getItem = productInventoryVM.Find(invoiceDetail.Product);

            getItem.Quantity += invoiceDetail.Quantity;
            getItem.Price.In += invoiceDetail.TotalPrice.In;
            getItem.Price.Out += invoiceDetail.TotalPrice.Out;
        }

        public void WriteItem(XmlNode node)
        {
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            productInventoryVM.ItemList.Items = Item.Products;
            XmlNode parentNode = node.SelectSingleNode("Products");

            Utilities.RemoveAllChilds(parentNode);
            productInventoryVM.WriteItems(parentNode, "Product");
            node.Attributes["TotalQuantity"].Value = Item.TotalQuantity.ToString();
            node.Attributes["TotalPriceInput"].Value = Item.TotalPrice.In.ToString();
            node.Attributes["TotalPriceOutput"].Value = Item.TotalPrice.Out.ToString();

            int count = parentNode.ChildNodes.Count;
            parentNode.Attributes["Count"].Value = count.ToString();
        }

        public void LoadItem(XmlNode nodeData)
        {
            Item.TotalQuantity = Convert.ToDouble(nodeData.Attributes["TotalQuantity"].Value);
            Item.TotalPrice.In = Convert.ToDouble(nodeData.Attributes["TotalPriceInput"].Value);
            Item.TotalPrice.Out = Convert.ToDouble(nodeData.Attributes["TotalPriceOutput"].Value);
        }

        public void WriteAll()
        {
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            ProductInventoryStatusViewModel productInventoryStatusVM = new ProductInventoryStatusViewModel();
            
            DataProvider.Instance.Open(Constants.fInventory);
            WriteItem(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            productInventoryVM.ItemList.Items = Item.Products;
            productInventoryVM.WriteAll();

            productInventoryStatusVM.Items = Item.ProductsStatus;
            productInventoryStatusVM.WriteAll();
        }

        public void WriteImportInventory()
        {
            ProductInventoryStatusViewModel productInventoryStatusVM = new ProductInventoryStatusViewModel();
            DataProvider.Instance.Open(Constants.fImportInventory);
            WriteItem(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            productInventoryStatusVM.Items = Item.ProductsStatus;
            productInventoryStatusVM.WriteImport();
        }

        public void LoadAll()
        {
            DataProvider.Instance.Open(Constants.fInventory);
            LoadItem(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public void LoadImportInventory()
        {
            ProductInventoryViewModel productInventoryVM = new ProductInventoryViewModel();
            ProductInventoryStatusViewModel productInventoryStatusVM = new ProductInventoryStatusViewModel();
            productInventoryVM.Products = Products;
            productInventoryStatusVM.Products = Products;

            DataProvider.Instance.Open(Constants.fImportInventory);
            LoadItem(DataProvider.Instance.nodeRoot);

            productInventoryVM.LoadImport(DataProvider.Instance.nodeRoot.FirstChild.ChildNodes);
            Item.Products = productInventoryVM.Items;

            productInventoryStatusVM.LoadImport();
            Item.ProductsStatus = productInventoryStatusVM.Items;

            DataProvider.Instance.Close();
        }

        public void Output()
        {
            ProductInventoryStatusViewModel productInventoryStatusVM = new ProductInventoryStatusViewModel();
            productInventoryStatusVM.Items = Item.ProductsStatus;

            if (productInventoryStatusVM.Items.Count > 0)
            {
                productInventoryStatusVM.OutputAll(true);
            }
            else
            {
                Utilities.NotifyAvailable("Product", "Inventory", false);
            }
        }
    }
}
