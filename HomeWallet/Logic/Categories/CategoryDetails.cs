using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models.CategoryViewModels;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.Categories
{
    public static class CategoryDetails
    {
        public static CategoryDetailsViewModel GetModel(int categoryId, ApplicationDbContext context)
        {
            var category =  context.Categories
                .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .SingleOrDefault(m => m.ID == categoryId);
            if (category==null)
            {
                return null;
            }
            var products = new List<CategoryProductsViewModel>();
            foreach (var productCategory in category.ProductCategories)
            {
              products.Add(new CategoryProductsViewModel()
              {
                ID = productCategory.Product.ID,
                Name = productCategory.Product.Name
              });
            }

            var model = new CategoryDetailsViewModel()
            {
              ID = categoryId,
              Name = category.Name,
              Products = products
            };
            return model;
        }
  }
}
