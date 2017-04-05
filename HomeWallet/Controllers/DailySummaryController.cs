using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Logic.DailySummary;
using HomeWallet.Models;
using HomeWallet.Models.DailySummaryViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeWallet.Controllers
{
    public class DailySummaryController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DailySummaryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: DailySummary
        public ActionResult Index(DateTime date)
        {
            var model = new DailySummaryViewModel()
            {
                TotalCost = DailySummary.GetAllMoney(_context,date,_userManager.GetUserId(HttpContext.User)),
                Percent = DailySummary.GetPercentage(_context,date,_userManager.GetUserId(HttpContext.User))
            };
            return View(model);
        }
    }
}