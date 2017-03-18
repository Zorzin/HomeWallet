using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models.ReceiptViewModels;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.Home
{
    public static class LoadHomeReceipts
    {
        public static ICollection<DateTime> GetDates(string lastDate, ApplicationDbContext context)
        {
            DateTime last;

            if (lastDate == "0")
            {
              last = DateTime.Today.AddDays(1);
            }
            else
            {
              last = DateTime.Parse(lastDate);
            }
            var dates = context.Receipts
                      .Where(r => r.PurchaseDate < last)
                      .Select(r => r.PurchaseDate)
                      .OrderBy(r => r.Date)
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

        public static List<HomeReceiptViewModel> GetHomeReceiptViewModels(ICollection<DateTime> dates, ApplicationDbContext context)
        {
            var model = new List<HomeReceiptViewModel>();
            foreach (var date in dates.ToList())
            {
                var receipts = context.Receipts
                                .Where(r => r.PurchaseDate == date)
                                .Include(r=>r.ReceiptProducts)
                                .Include(r=>r.Shop)
                                .ToList();
                foreach (var receipt in receipts)
                {
                    var homeReceiptViewModel = new HomeReceiptViewModel { Date = date };
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
                    model.Add(homeReceiptViewModel);
                }
            }
            return model;
        }
    }
}
