using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWallet.Models.PlanViewModels
{
    public class CreatePlanViewModel
    {
        public double Amount {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}
    }
}
