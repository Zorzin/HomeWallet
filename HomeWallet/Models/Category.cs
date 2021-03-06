using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeWallet.Models
{
    public class Category
    {
        public int ID {get;set;}
        public string Name {get;set;}
        public string UserID {get;set;}
        public virtual ApplicationUser User{get;set;}
        public virtual ICollection<ProductCategory> ProductCategories {get;set;}
        
    }
}
