using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeWallet.Models
{
    public class Product
    {
        public int ID {get;set;}
        public string Name {get;set;}
        public int ProductCategoryID {get;set;}

        public virtual ICollection<ProductCategory> ProductCategories {get;set;}
    }
}
