using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;

namespace HomeWallet.Logic.Shops
{
    public static class CheckShop
    {
      public static bool CheckById(int shopId, string userId, ApplicationDbContext context)
        {
            var shop = context.Shops.FirstOrDefault(s => s.ID == shopId);
            return shop.UserID == userId;
        }
    }
}
