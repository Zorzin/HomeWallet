using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models.ShopViewModels;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.Shops
{
    public static class ShopDetails
    {

        public static ShopDetailsViewModel GetModel(int shopId, ApplicationDbContext context)
        {
             var shop = context.Shops
                .Include(s=>s.Receipts)
                .ThenInclude(r=>r.ReceiptProducts)
                .ThenInclude(rp=>rp.Product)
                .SingleOrDefault(m => m.ID == shopId);
            if (shop==null)
            {
                return null;
            }
            var products = new List<ShopProductsViewModel>();
            var productsID = new List<int>();
            foreach (var shopReceipt in shop.Receipts)
            {
                foreach (var shopReceiptReceiptProduct in shopReceipt.ReceiptProducts)
                {
                    if (!productsID.Contains(shopReceiptReceiptProduct.ProductID))
                    {
                          productsID.Add(shopReceiptReceiptProduct.ProductID);
                          products.Add(new ShopProductsViewModel()
                          {
                              ID = shopReceiptReceiptProduct.Product.ID,
                              Name = shopReceiptReceiptProduct.Product.Name
                          });
                    }
                }
            }
            var model = new ShopDetailsViewModel()
            {
                ID = shopId,
                Name = shop.Name,
                Products = products
            };

            return model;
        }
    }
}
