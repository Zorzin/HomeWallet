using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeWallet.Data;
using HomeWallet.Logic;
using HomeWallet.Logic.Categories;
using HomeWallet.Models;
using HomeWallet.Models.CategoryViewModels;
using Microsoft.AspNetCore.Identity;

namespace HomeWallet.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoriesController(ApplicationDbContext context, UserManager<ApplicationUser>  userManager)
        {
            _context = context;    
            _userManager = userManager;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var categories = _context.Categories.Where(s => s.UserID == _userManager.GetUserId(HttpContext.User));

            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(s => s.Name);
                    break;
                default:
                    categories= categories.OrderBy(s => s.Name);
                    break;
            }

            var pageSize = 12;
            return View(await PaginatedList<Category>.CreateAsync(categories, page ?? 1, pageSize));
        
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!CheckCategory.CheckById((int)id, _userManager.GetUserId(HttpContext.User),_context))
            {
                return RedirectToAction("Index", "Categories");
            }

            var model = CategoryDetails.GetModel((int)id,_context);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,UserID")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", category.UserID);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!CheckCategory.CheckById((int)id, _userManager.GetUserId(HttpContext.User),_context))
            {
                return RedirectToAction("Index", "Categories");
            }
            var category = await _context.Categories.SingleOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!CheckCategory.CheckById(category.ID, _userManager.GetUserId(HttpContext.User),_context))
            {
                return RedirectToAction("Index", "Categories");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var update = _context.Categories.FirstOrDefault(c=>c.ID == category.ID);
                    update.Name = category.Name;
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!CheckCategory.CheckById((int)id, _userManager.GetUserId(HttpContext.User),_context))
            {
                return RedirectToAction("Index", "Categories");
            }
            var category = await _context.Categories
                .Include(c => c.User)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!CheckCategory.CheckById((int)id, _userManager.GetUserId(HttpContext.User),_context))
            {
                return RedirectToAction("Index", "Categories");
            }
            var category = await _context.Categories.SingleOrDefaultAsync(m => m.ID == id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}
