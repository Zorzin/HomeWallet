using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models.ReceiptViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeWallet.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult GetReceipts(string lastDate)
        {
            var model = new List<HomeReceiptViewModel>();
            DateTime last;

            if (lastDate == "0")
            {
                last = DateTime.Today.AddDays(1);
            }
            else
            {
                last = DateTime.Parse(lastDate);
            }

            var dates = _context.Receipts
                .Where(r => r.PurchaseDate < last)
                .Select(r => r.PurchaseDate)
                .OrderBy(r=>r.Date)
                .Distinct().Take(3);

            foreach (var date in dates)
            {
                var receipts = _context.Receipts.Where(r => r.PurchaseDate == date).ToList();
                foreach (var receipt in receipts)
                {
                    var homeReceiptViewModel = new HomeReceiptViewModel {Date = date};
                    double value =0;
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
            return PartialView(model);
        }
    }
}
