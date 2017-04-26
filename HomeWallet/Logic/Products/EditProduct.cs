using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.Products
{
    public static class EditProduct
    {
        public static void DeleteOldCategories(Product product, ICollection<int> newCategories, ApplicationDbContext context )
        {
            var oldcategories = product.ProductCategories.Select(pc => pc.CategoryID);
            if (newCategories != null)
            {
                foreach (var oldcategory in oldcategories)
                {
                    if (!newCategories.Contains(oldcategory))
                    {
                        var pc =
                            context.ProductCategories.FirstOrDefault(
                                p => p.ProductID == product.ID && p.CategoryID == oldcategory);
                        context.ProductCategories.Remove(pc);
                    }
                }
            }
            else
            {
                foreach (var oldcategory in oldcategories)
                {
                    var pc = context.ProductCategories.FirstOrDefault(
                                p => p.ProductID == product.ID && p.CategoryID == oldcategory);
                    context.ProductCategories.Remove(pc);
                }
            }
        }

        public static void AddNewCategories(Product product, ICollection<int> newCategories, ApplicationDbContext context)
        {
            var oldcategories = product.ProductCategories.Select(pc => pc.CategoryID);
            if (newCategories!= null)
            {
                foreach (var category in newCategories)
                {
                    if (!oldcategories.Contains(category))
                    {
                        var productcategory = new ProductCategory
                        {
                            CategoryID = category,
                            ProductID = product.ID
                        };
                        context.ProductCategories.Add(productcategory);
                    }
                }
            }
        }
      
    }
    
    
}
