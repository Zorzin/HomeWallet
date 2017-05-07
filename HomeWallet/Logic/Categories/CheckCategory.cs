using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;

namespace HomeWallet.Logic.Categories
{
    public static class CheckCategory
    {
        public static bool CheckById(int categoryId, string userId, ApplicationDbContext context)
        {
            var category = context.Categories.FirstOrDefault(c => c.ID == categoryId);
            return category.UserID == userId;
        }
    }
}
