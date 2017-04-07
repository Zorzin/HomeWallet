using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.Plan
{
    public static class PlanView
    {
        public static double GetAlreadySpend(string userid, DateTime startDate, DateTime endDate, ApplicationDbContext context)
        {
            var receipts = context.Receipts.Where(r => r.UserID == userid).Include(r=>r.ReceiptProducts).ToList();
            receipts = receipts.Where(r => r.PurchaseDate >= startDate && r.PurchaseDate < endDate).ToList();
            return receipts.SelectMany(receipt => receipt.ReceiptProducts).Sum(product => product.Amount * product.Price);
        }

        public static double GetPercentageSpend(string userid, DateTime startDate, DateTime endDate, ApplicationDbContext context, double plan)
        {
            var already = GetAlreadySpend(userid, startDate,endDate, context);
            var result = 100 * already / plan;
            return result;
        }

        public static double GetMoneyLeft(string userid, DateTime startDate, DateTime endDate, ApplicationDbContext context, double plan)
        {
            var already = GetAlreadySpend(userid, startDate, endDate, context);
            return plan - already;
        }

        public static int GetDaysLeft(DateTime end)
        {
            return (end - DateTime.Today).Days;
        }
        
        public static int GetDaysBehind(DateTime start, DateTime end)
        {
            var left = GetDaysLeft(end);
            var alldays = (end - start).Days;
            return alldays - left;
        }
        public static double GetAveragePerDay(string userid, DateTime startDate, DateTime endDate, ApplicationDbContext context, double plan)
        {
            var already = GetMoneyLeft(userid,startDate,endDate,context,plan);
            var days = GetDaysLeft(endDate);
            return Math.Round(already / days,2);
        }
    }
}
