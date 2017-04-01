using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Data;
using HomeWallet.Models;
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
            return View();
        }
    }
}