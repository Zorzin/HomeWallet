using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeWallet.Data;
using HomeWallet.Logic.Products;
using HomeWallet.Models;
using HomeWallet.Models.ProductViewModels;
using HomeWallet.Models.ReceiptViewModels;
using Microsoft.AspNetCore.Identity;

namespace HomeWallet.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ReceiptsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Receipts
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index","Home");
        }

        // GET: Receipts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts
                .Include(r => r.Shop)
                .Include(r => r.User)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }


        public IActionResult CreateCyclical(DateTime date)
        {
            if (date == default(DateTime))
            {
                date = DateTime.Today;
            }
            var model = new CreateCyclicalReceiptViewModel()
            {
                StartDate = date.Date,
                EndDate = date.Date.AddDays(30)
            };
            var userid = _userManager.GetUserId(HttpContext.User);
            ViewData["ShopID"] = new SelectList(_context.Shops.Where(s=>s.UserID==userid), "ID", "Name");
            return View(model);
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCyclical(CreateCyclicalReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                var start = model.StartDate;
                while (start<=model.EndDate)
                {
                    var receipt = CreateProduct.CreateReceipt(model.ShopID, _userManager.GetUserId(HttpContext.User), start, _context);
                    CreateProduct.CreateReceiptProducts(model.Products, receipt, _context);
                    start = start.AddMonths(model.Cycle);
                }
                return RedirectToAction("Index");
            }
            var userid = _userManager.GetUserId(HttpContext.User);
            ViewData["ShopID"] = new SelectList(_context.Shops.Where(s=>s.UserID==userid), "ID", "Name", model.ShopID);
            return View();
        }



        // GET: Receipts/Create
        public IActionResult Create(DateTime date)
        {
            if (date == default(DateTime))
            {
                date = DateTime.Today;
            }
            var model = new CreateReceiptViewModel()
            {
                Date = date.Date
            };
            var userid = _userManager.GetUserId(HttpContext.User);
            ViewData["ShopID"] = new SelectList(_context.Shops.Where(s=>s.UserID==userid), "ID", "Name");
            return View(model);
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateReceiptViewModel model)
        {
          if (ModelState.IsValid)
          {
            var receipt = CreateProduct.CreateReceipt(model.ShopID, _userManager.GetUserId(HttpContext.User), model.Date, _context);
            CreateProduct.CreateReceiptProducts(model.Products, receipt, _context);
            return RedirectToAction("Index");
          }
          var userid = _userManager.GetUserId(HttpContext.User);
          ViewData["ShopID"] = new SelectList(_context.Shops.Where(s => s.UserID == userid), "ID", "Name", model.ShopID);
          return View();
        }


        public PartialViewResult AddProduct()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            ViewData["Products"] = new SelectList(_context.Products.Where(p=>p.UserID ==userid),"ID","Name");
            return PartialView("CreateProduct");
        }
        [HttpPost]
        public ViewResult AddProduct(CreateProductViewModel model)
        {
            var addModel = CreateProduct.CreateModel(model, _userManager.GetUserId(HttpContext.User), _context);
            return View(addModel);
        }
        [HttpPost]
        public ViewResult AddCyclicalProduct(CreateProductViewModel model)
        {
            var addModel = CreateProduct.CreateModel(model, _userManager.GetUserId(HttpContext.User), _context);
            return View(addModel);
        }

        public ViewResult CreateShop()
        {
            return View();
        }

        [HttpPost]
        public string[] AddShop(Shop model)
        {
            var shop = new Shop()
            {
                Name = model.Name,
                UserID = _userManager.GetUserId(HttpContext.User)
            };
            _context.Shops.Add(shop);
            _context.SaveChanges();
            var result = new string[2];
            result[0] = shop.ID.ToString();
            result[1] = shop.Name;
            return result;
        }

        // GET: Receipts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts.SingleOrDefaultAsync(m => m.ID == id);
            if (receipt == null)
            {
                return NotFound();
            }
            ViewData["ShopID"] = new SelectList(_context.Shops, "ID", "Name", receipt.ShopID);
            return View(receipt);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PurchaseDate,ShopID,UserID")] Receipt receipt)
        {
            if (id != receipt.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.ID))
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
            ViewData["ShopID"] = new SelectList(_context.Shops, "ID", "Name", receipt.ShopID);
            return View(receipt);
        }

        // GET: Receipts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipts
                .Include(r => r.Shop)
                .Include(r => r.User)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // POST: Receipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receipt = await _context.Receipts.SingleOrDefaultAsync(m => m.ID == id);
            _context.Receipts.Remove(receipt);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReceiptExists(int id)
        {
            return _context.Receipts.Any(e => e.ID == id);
        }
    }
}
