using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models.ReceiptViewModels;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.Home
{
    public static class LoadHomeReceipts
    {
        public static ICollection<DateTime> GetDates(string lastDate, string userId, ApplicationDbContext context)
        {
            DateTime last;

            if (lastDate == "0")
            {
              last = DateTime.Today.AddDays(1);
            }
            else
            {
              last = DateTime.ParseExact(lastDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var dates = context.Receipts
                      .Where(r => r.PurchaseDate < last && r.UserID == userId)
                      .Select(r => r.PurchaseDate)
                      .OrderByDescending(r => r.Date)
                      .Distinct().ToList();
            if (dates.Count()>3)
            {
                return dates.Take(3).ToList();
            }
            if (!dates.Any())
            {
                return null;
            }
            return dates;
        }

        public static List<HomeReceiptViewModel> GetHomeReceiptViewModels(ICollection<DateTime> dates,string userId, ApplicationDbContext context)
        {
            var model = new List<HomeReceiptViewModel>();
            foreach (var date in dates.ToList())
            {
                var receipts = context.Receipts
                                .Where(r => r.PurchaseDate == date && r.UserID == userId)
                                .Include(r=>r.ReceiptProducts)
                                .Include(r=>r.Shop)
                                .ToList();
                var homeReceiptViewModel = new HomeReceiptViewModel { Date = date };
                foreach (var receipt in receipts)
                {
                    double value = 0;
                    foreach (var receiptProduct in receipt.ReceiptProducts)
                    {
                      value += (receiptProduct.Price * receiptProduct.Amount);
                    }
                    var showReceiptViewModel = new ShowReceiptViewModel()
                    {
                      ID = receipt.ID,
                      Shop = receipt.Shop.Name,
                      Value = value
                    };
                    homeReceiptViewModel.Receipts.Add(showReceiptViewModel);
                }
                model.Add(homeReceiptViewModel);
            }
            return model;
        }
    }
}
