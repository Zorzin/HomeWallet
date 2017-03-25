using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Logic.Home;
using HomeWallet.Models;
using HomeWallet.Models.ReceiptViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeWallet.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return View();
            }
            return View("MainPage");
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
            var dates = LoadHomeReceipts.GetDates(lastDate,_userManager.GetUserId(HttpContext.User),_context);
            if (dates!=null)
            {
              var model = LoadHomeReceipts.GetHomeReceiptViewModels(dates.ToList(),_userManager.GetUserId(HttpContext.User), _context);
              return PartialView(model);
            }
            return null;
        }
    }
}
