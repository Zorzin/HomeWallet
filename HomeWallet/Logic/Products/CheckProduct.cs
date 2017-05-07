using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;

namespace HomeWallet.Logic.Products
{
    public static class CheckProduct
    {
        public static bool CheckById(int productId, string userId, ApplicationDbContext context)
        {
            var product = context.Products.FirstOrDefault(c => c.ID == productId);
            return product.UserID == userId;
        }
    }
}
