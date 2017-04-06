using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.DailySummary
{
    public static class DailySummary
    {
        public static double GetAllMoney(ApplicationDbContext context, DateTime date, string userid)
        {
            var receipts = context.Receipts.Where(r=>r.PurchaseDate == date && r.UserID == userid).Include(r=>r.ReceiptProducts).ToList();
            double result = 0;

            foreach(var receipt in receipts)
            {
                foreach(var productreceipt in receipt.ReceiptProducts)
                {
                    result += productreceipt.Amount*productreceipt.Price;
                }
            }
            return result;
        }

        public static double GetPercentage(ApplicationDbContext context, DateTime date, string userid)
        {
            var total = GetAllMoney(context,date,userid);
            var plan = context.Plans.FirstOrDefault(p=>p.UserID == userid && p.StartDate<= date && p.EndDate>= date);
            if(plan!=null)
            {
                return Math.Round(100*total/plan.Amount,1);
            }
            return -1;
        }

    }
}