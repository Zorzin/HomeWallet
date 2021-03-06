using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeWallet.Models
{
    public class Plan
    {
        public int ID {get;set;}
        public double Amount {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}
        public string UserID {get;set;}

        public ApplicationUser User {get;set;}
    }
}
