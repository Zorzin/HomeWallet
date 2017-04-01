using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models;
using HomeWallet.Models.PlanViewModels;

namespace HomeWallet.Logic.Plan
{
    public static class CreatePlan
    {
        public static void Create(CreatePlanViewModel model, string userid, ApplicationDbContext context)
        {
            var plan = new Models.Plan()
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Amount = model.Amount,
                UserID = userid
            };
            context.Plans.Add(plan);
            context.SaveChanges();
        }

        public static bool CheckPlanByUserId(string userid,int planid, ApplicationDbContext context)
        {
            var lastplan = context.Plans.Max(p => p.ID);
            if (lastplan<planid)
            {
                return false;
            }
            var plan = context.Plans.FirstOrDefault(p => p.ID == planid);
            return plan.UserID == userid;
        }
        
        public static bool CheckCreatePossibility(string userid, ApplicationDbContext context)
        {
            var plan = context.Plans.Where(p => p.UserID == userid && p.EndDate > DateTime.Today);
            return plan ==null;
        }
    }
}
