using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AnhQuoc_WPF_C4_B1
{
    public class ProductByCategoryViewModel
    {
        public List<ProductByCategory> Items;
        public List<ProductInvoiceByCategory> ItemInvoices;
        public List<ProductInventoryByCategory> ItemInventorys;

        public ProductInvoiceByCategory FindProductInvoiceByCategory(List<ProductInvoiceByCategory> items, ProductCategory value)
        {
            foreach (ProductInvoiceByCategory item in items)
            {
                if (item.Category == value)
                    return item;
            }
            return null;
        }

        public List<ProductInvoiceByCategory> ConvertProductInvoices(List<ProductInvoice> products)
        {
            List<ProductInvoiceByCategory> result = new List<ProductInvoiceByCategory>();
            foreach (ProductInvoice item in products)
            {
                ProductCategory cateCheck = item.Product.Category;
                ProductInvoiceByCategory itemFinded = FindProductInvoiceByCategory(result, cateCheck);

                if (itemFinded == null)
                {
                    itemFinded = new ProductInvoiceByCategory();
                    result.Add(itemFinded);

                    itemFinded.Category = cateCheck;
                }
                itemFinded.Products.Add(item);
            }
            return result;
        }

        public List<ProductInventoryByCategory> ConvertProductInventorys(List<ProductInventory> products)
        {
            List<ProductInventoryByCategory> result = new List<ProductInventoryByCategory>();
            foreach (ProductInventory item in products)
            {
                ProductCategory cateCheck = item.Product.Category;
                ProductInventoryByCategory itemFinded = FindProductInventoryByCategory(result, cateCheck);

                if (itemFinded == null)
                {
                    itemFinded = new ProductInventoryByCategory();
                    result.Add(itemFinded);

                    itemFinded.Category = cateCheck;
                }
                itemFinded.Products.Add(item);
            }
            return result;
        }

        public List<ProductInventoryStatusByCategory> ConvertProductInventoryStatuss(List<ProductInventoryStatus> products)
        {
            List<ProductInventoryStatusByCategory> result = new List<ProductInventoryStatusByCategory>();
            foreach (ProductInventoryStatus item in products)
            {
                ProductCategory cateCheck = item.Item.Product.Category;
                ProductInventoryStatusByCategory itemFinded = FindProductInventoryStatusByCategory(result, cateCheck);

                if (itemFinded == null)
                {
                    itemFinded = new ProductInventoryStatusByCategory();
                    result.Add(itemFinded);

                    itemFinded.Category = cateCheck;
                }
                itemFinded.Products.Add(item);
            }
            return result;
        }

        public List<ProductInvoice> ConvertBackProductInvoices(List<ProductInvoiceByCategory> productsByCat)
        {
            List<ProductInvoice> result = new List<ProductInvoice>();
            foreach (ProductInvoiceByCategory itemCat in productsByCat)
            {
                foreach (ProductInvoice item in itemCat.Products)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public ProductInventoryByCategory FindProductInventoryByCategory(List<ProductInventoryByCategory> items, ProductCategory value)
        {
            foreach (ProductInventoryByCategory item in items)
            {
                if (item.Category == value)
                    return item;
            }
            return null;
        }

        public ProductInventoryStatusByCategory FindProductInventoryStatusByCategory(List<ProductInventoryStatusByCategory> items, ProductCategory value)
        {
            foreach (ProductInventoryStatusByCategory item in items)
            {
                if (item.Category == value)
                    return item;
            }
            return null;
        }

        public bool IsEmpty()
        {
            bool isEmpty = true;
            if (Items.Count == 0)
            {
                return true;
            }
            foreach (ProductByCategory item in Items)
            {
                if (item.Products.Count > 0)
                {
                    isEmpty = false;
                }
            }
            return isEmpty;
        }

        public bool IsEmpty(List<ProductInventoryByCategory> list)
        {
            bool isEmpty = true;
            if (list.Count == 0)
            {
                return true;
            }
            foreach (ProductInventoryByCategory item in list)
            {
                if (item.Products.Count > 0)
                {
                    foreach (ProductInventory product in item.Products)
                    {
                        if (product.Quantity > 0)
                        {
                            isEmpty = false;
                        }
                    }
                }
            }
            return isEmpty;
        }

        public bool IsEmpty(List<ProductInvoiceByCategory> list)
        {
            bool isEmpty = true;
            if (list.Count == 0)
            {
                return true;
            }
            foreach (ProductInvoiceByCategory item in list)
            {
                if (item.Products.Count > 0)
                {
                    foreach (ProductInvoice product in item.Products)
                    {
                        if (product.TotalQuantity > 0)
                        {
                            isEmpty = false;
                        }
                    }
                }
            }
            return isEmpty;
        }

        public RepositoryBase<ProductByCategory> ConvertTo(List<List<Product>> list)
        {
            RepositoryBase<ProductByCategory> getList = new RepositoryBase<ProductByCategory>();

            int index = 0;
            List<ProductCategory> productsCategory = GetListProductCategory();

            foreach (List<Product> products in list)
            {
                ProductByCategory newItem = new ProductByCategory();

                newItem.Category = productsCategory[index];
                newItem.Products = products;
                getList.Add(newItem);

                index++;
            }
            return getList;
        }

        public List<List<Product>> ConvertBack(RepositoryBase<ProductByCategory> source)
        {
            List<List<Product>> getList = new List<List<Product>>();
            foreach (ProductByCategory productCatItem in source.Gets())
            {
                List<Product> products = productCatItem.Products;
                getList.Add(products);
            }
            return getList;
        }

        public List<ProductCategory> FillCategories(RepositoryBase<ProductByCategory> source)
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (ProductByCategory productCatItem in source.Gets())
            {
                ProductCategory category = productCatItem.Category;
                categories.Add(category);
            }
            return categories;
        }

        public List<ProductCategory> GetListProductCategory()
        {
            List<ProductCategory> result = new List<ProductCategory>();
            DataProvider.Instance.Open(Constants.fProducts);
            XmlNodeList lstCategoriesNode = DataProvider.Instance.nodeRoot.ChildNodes;

            foreach (XmlNode catNode in lstCategoriesNode)
            {
                string categoryName = catNode.Name;
                ProductCategory category = (ProductCategory)Enum.Parse(typeof(ProductCategory), categoryName);
                result.Add(category);
            }
            DataProvider.Instance.Close();
            return result;
        }

        public RepositoryBase<ProductByCategory> FillExpDate(List<ProductByCategory> list)
        {
            RepositoryBase<ProductByCategory> result = new RepositoryBase<ProductByCategory>();
            foreach (ProductByCategory item in list)
            {
                if (item.Category == ProductCategory.Food)
                {
                    ProductByCategory newItem = new ProductByCategory();
                    newItem.Category = item.Category;

                    foreach (Product subItem in item.Products)
                    {
                        if (subItem.ExpDate.Date < DateTime.Now.Date)
                        {
                            newItem.Products.Add(subItem);
                        }
                    }
                    result.Add(newItem);
                }
            }
            return result;
        }


        public int getIndexInList(ProductInvoice product)
        {
            foreach (var products in ItemInvoices)
            {
                List<ProductInvoice> productList = products.Products;
                int index = productList.IndexOf(product);
                if (index != -1)
                    return index;
            }
            return -1;
        }

        public int getIndexInList(ProductInventory product)
        {
            foreach (var products in ItemInventorys)
            {
                List<ProductInventory> productList = products.Products;
                int index = productList.IndexOf(product);
                if (index != -1)
                    return index;
            }
            return -1;
        }

        //public List<ProductInvoiceByCategory> GetClone(List<ProductInvoiceByCategory> source)
        //{
        //    List<ProductInvoiceByCategory> dest = new List<ProductInvoiceByCategory>();

        //    foreach (ProductInvoiceByCategory itemSource in source)
        //    {
        //        ProductInvoiceByCategory newDestItem = new ProductInvoiceByCategory();

        //        newDestItem.Category = itemSource.Category;
        //        foreach (ProductInvoice productSource in itemSource.Products)
        //        {
        //            ProductInvoice newDestProduct = new ProductInvoice();

        //            newDestProduct.TotalQuantity = productSource.TotalQuantity;
        //            newDestProduct.TotalPrice = new Price();
        //            newDestProduct.TotalPrice.In = productSource.TotalPrice.In;
        //            newDestProduct.TotalPrice.Out = productSource.TotalPrice.Out;

        //            newDestItem.Products.Add(newDestProduct);
        //        }
        //        dest.Add(newDestItem);
        //    }
        //    return dest;
        //}

        public void UpdateListSource(List<ProductByCategory> source, List<ProductByCategory> listToUpdate)
        {
            int index = 0;
            foreach (ProductByCategory itemUpdate in listToUpdate)
            {
                ProductByCategory itemSource = source[index];

                List<Product> lstProductSource = itemSource.Products;
                int indexProduct = 0;
                foreach (Product productUpdate in itemUpdate.Products)
                {
                    Product productSource = lstProductSource[indexProduct];

                    productSource.Price.In = productUpdate.Price.In;
                    productSource.Price.Out = productUpdate.Price.Out;

                    indexProduct++;
                }
                index++;
            }
        }

        public void UpdateListSource(List<ProductInvoiceByCategory> source, List<ProductInvoiceByCategory> listToUpdate)
        {
            int index = 0;
            foreach (ProductInvoiceByCategory itemUpdate in listToUpdate)
            {
                ProductInvoiceByCategory itemSource = source[index];

                List<ProductInvoice> lstProductSource = itemSource.Products;
                int indexProduct = 0;
                foreach (ProductInvoice productUpdate in itemUpdate.Products)
                {
                    ProductInvoice productSource = lstProductSource[indexProduct];

                    productSource.TotalQuantity = productUpdate.TotalQuantity;
                    productSource.TotalPrice.In = productUpdate.TotalPrice.In;
                    productSource.TotalPrice.Out = productUpdate.TotalPrice.Out;

                    indexProduct++;
                }
                index++;
            }
        }

        public void UpdateListSource(List<ProductInventoryByCategory> source, List<ProductInventoryByCategory> listToUpdate)
        {
            int index = 0;
            foreach (ProductInventoryByCategory itemUpdate in listToUpdate)
            {
                ProductInventoryByCategory itemSource = source[index];

                List<ProductInventory> lstProductSource = itemSource.Products;
                int indexProduct = 0;
                foreach (ProductInventory productUpdate in itemUpdate.Products)
                {
                    ProductInventory productSource = lstProductSource[indexProduct];

                    productSource.Quantity = productUpdate.Quantity;
                    productSource.Price.In = productUpdate.Price.In;
                    productSource.Price.Out = productUpdate.Price.Out;

                    indexProduct++;
                }
                index++;
            }
        }

    }
}
