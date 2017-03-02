using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeWallet.Models
{
    public class ReceiptProduct
    {
        public int ID {get;set;}
        public double Amount {get;set;}
        public double Price {get;set;}
        public int ReceiptID {get;set;}
        public int ProductID {get;set;}

        public Receipt Receipt {get;set;}
        public Product Product {get;set;}
    }
}
