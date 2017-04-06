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
using Highsoft.Web.Mvc.Charts;
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
            var percentage = DailySummary.GetPercentage(_context,date,_userManager.GetUserId(HttpContext.User));
            List<PieSeriesData> pieData = new List<PieSeriesData>();
            pieData.Add(new PieSeriesData 
            { 
                Name = date.ToString("dd-MM-yyyy"), 
                Y = percentage, 
                Sliced = true, 
                Selected = true, 
                Color="white"
            });
            pieData.Add(new PieSeriesData 
            { 
                Name = "Plan", 
                Y = 100-percentage, 
                Sliced = true, 
                Selected = true, 
                Color="#222"
            });

            ViewData["pieData"] = pieData;

            var model = new DailySummaryViewModel()
            {
                TotalCost = DailySummary.GetAllMoney(_context,date,_userManager.GetUserId(HttpContext.User)),
                Date = date.ToString("dd-MM-yyyy"),
                Percent = percentage,
                Currency = "zł"
            };
            return View(model);
        }
    }
}