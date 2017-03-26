using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models;
using HomeWallet.Models.ReceiptViewModels;
using Microsoft.AspNetCore.Http;

namespace HomeWallet.Logic.Products
{
    public static class CreateProduct
    {
    
        public static void CreateProductCategories(ICollection<int> categories, int productID, ApplicationDbContext context)
        {
            foreach (var category in categories)
            {
              var productcategory = new ProductCategory()
              {
                ProductID = productID,
                CategoryID = category
              };
              context.ProductCategories.Add(productcategory);
              context.SaveChanges();
            }
        }

        public static void CreateReceiptProducts(ICollection<AddProductViewModel> products, Receipt receipt, ApplicationDbContext context )
        {
            foreach(var product in products)
            {
                var receiptproduct = new ReceiptProduct()
                {
                    ReceiptID = receipt.ID,
                    ProductID = product.Product.ID,
                    Amount = product.Amount,
                    Price = product.Price
                };
                context.ReceiptProducts.Add(receiptproduct);
                context.SaveChanges();
            }
        }

        public static Receipt CreateReceipt(int shopID, string userID, DateTime date, ApplicationDbContext context)
        {
            var receipt = new Receipt()
            {
                ShopID = shopID,
                UserID = userID,
                PurchaseDate = date
            };
            context.Receipts.Add(receipt);
            context.SaveChanges();
            return receipt;
        }

        public static Product Create(string name,string userid, ApplicationDbContext context)
        {
            var product = new Product()
            {
                Name = name,
                UserID = userid
            };
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }
    }
}
