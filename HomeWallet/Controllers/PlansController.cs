using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeWallet.Data;
using HomeWallet.Logic.Plan;
using HomeWallet.Models;
using HomeWallet.Models.PlanViewModels;
using Microsoft.AspNetCore.Identity;
using Highsoft.Web.Mvc.Charts;

namespace HomeWallet.Controllers
{
    public class PlansController : Controller
    {
        private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PlansController(ApplicationDbContext context, UserManager<ApplicationUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Plans
        public IActionResult Index()
        {
            var plan = _context.Plans.FirstOrDefault(p => p.UserID == _userManager.GetUserId(HttpContext.User));
            if (plan!=null)
            {
                var percent = PlanView.GetPercentageSpend(_userManager.GetUserId(HttpContext.User), plan.StartDate,
                    plan.EndDate, _context, plan.Amount);
                var daysleft = PlanView.GetDaysLeft(plan.EndDate);
                var model = new ViewPlanViewModel()
                {
                    StartDate = plan.StartDate.ToString("dd-MM-yyyy"),
                    EndDate = plan.EndDate.ToString("dd-MM-yyyy"),
                    Amount = Math.Round(plan.Amount,2),
                    DaysLeft = daysleft,
                    Already = PlanView.GetAlreadySpend(_userManager.GetUserId(HttpContext.User),plan.StartDate,plan.EndDate,_context),
                    MoneyLeft = PlanView.GetMoneyLeft(_userManager.GetUserId(HttpContext.User), plan.StartDate, plan.EndDate, _context,plan.Amount),
                    Percent = percent,
                    Average = PlanView.GetAveragePerDay(_userManager.GetUserId(HttpContext.User), plan.StartDate, plan.EndDate, _context, plan.Amount),
                    Currency = "zł",
                    Id = plan.ID
                };

                List<PieSeriesData> pieData = new List<PieSeriesData>();

                pieData.Add(new PieSeriesData { Name = "Left", Y = 100-percent, Sliced = true, Selected = true, Color = "#222" });
                pieData.Add(new PieSeriesData { Name = "Already", Y = percent, Sliced = true, Selected = true, Color = "white" });
                ViewData["circleData"] = pieData;

                List<PieSeriesData> halfData = new List<PieSeriesData>();
                halfData.Add(new PieSeriesData { Name = "Days behind", Y = PlanView.GetDaysBehind(plan.StartDate, plan.EndDate), Color = "#222" });
                halfData.Add(new PieSeriesData { Name = "Days left", Y = daysleft, Color = "white" });
                ViewData["halfData"] = halfData;
        
                return View(model);   
            }
            return View("Create");
        }

        // GET: Plans/Create
        public IActionResult Create()
        {
            if (CreatePlan.CheckCreatePossibility(_userManager.GetUserId(HttpContext.User), _context))
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: Plans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePlanViewModel plan)
        {
            if (ModelState.IsValid)
            {
                CreatePlan.Create(plan, _userManager.GetUserId(HttpContext.User), _context);
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!CreatePlan.CheckPlanByUserId(_userManager.GetUserId(HttpContext.User), (int) id, _context))
            {
                return RedirectToAction("Index");
            }
            var plan = await _context.Plans.SingleOrDefaultAsync(m => m.ID == id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", plan.UserID);
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Amount,StartDate,EndDate,UserID")] Plan plan)
        {
            if (id != plan.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", plan.UserID);
            return View(plan);
        }

        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Plans.SingleOrDefaultAsync(m => m.ID == id);
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.ID == id);
        }
    }
}
