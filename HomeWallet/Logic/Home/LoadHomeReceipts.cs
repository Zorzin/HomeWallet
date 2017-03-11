using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models.ReceiptViewModels;

namespace HomeWallet.Logic.Home
{
    public static class LoadHomeReceipts
    {
        public static IQueryable<DateTime> GetDates(string lastDate, ApplicationDbContext context)
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
            return context.Receipts
                      .Where(r => r.PurchaseDate < last)
                      .Select(r => r.PurchaseDate)
                      .OrderBy(r => r.Date)
                      .Distinct().Take(3);
        }

        public static List<HomeReceiptViewModel> GetHomeReceiptViewModels(IQueryable<DateTime> dates, ApplicationDbContext context)
        {
            var model = new List<HomeReceiptViewModel>();
            foreach (var date in dates)
            {
                var receipts = context.Receipts.Where(r => r.PurchaseDate == date).ToList();
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
