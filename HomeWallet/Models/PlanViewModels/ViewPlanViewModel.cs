using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWallet.Models.PlanViewModels
{
    public class ViewPlanViewModel
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double Amount { get; set; }
        public int DaysLeft { get; set; }
        public double Already { get; set; }
        public double Percent { get; set; }
        public double MoneyLeft { get; set; }
    }
}
