using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Logic.Home;
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
            var dates = LoadHomeReceipts.GetDates(lastDate,_context);
            var model = LoadHomeReceipts.GetHomeReceiptViewModels(dates, _context);
            return PartialView(model);
        }
    }
}
