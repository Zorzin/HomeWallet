using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeWallet.Models
{
    public class CyclicalReceipt
    {
        public int ID {get;set;}
        public int Cycle {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}
        public int ReceiptID {get;set;}

        public Receipt Receipt {get;set;}
    }
}
