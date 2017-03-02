using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeWallet.Models
{
    public class Shop
    {
        public int ID {get;set;}
        public string Name {get;set;}

        public ICollection<Receipt> Receipts {get;set;}
    }
}