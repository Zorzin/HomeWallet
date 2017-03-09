using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeWallet.Data;
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
            var applicationDbContext = _context.Receipts.Include(r => r.Shop).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
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

        // GET: Receipts/Create
        public IActionResult Create()
        {
            ViewData["ShopID"] = new SelectList(_context.Shops, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                var receipt = new Receipt()
                {
                    ShopID = model.ShopID,
                    UserID = _userManager.GetUserId(HttpContext.User),
                    PurchaseDate = model.Date
                };
                _context.Receipts.Add(receipt);
                await _context.SaveChangesAsync();

                foreach(var product in model.Products)
                {
                    var receiptproduct = new ReceiptProduct()
                    {
                        ReceiptID = receipt.ID,
                        ProductID = product.Product.ID,
                        Amount = product.Amount,
                        Price = product.Price
                    };
                    _context.ReceiptProducts.Add(receiptproduct);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            ViewData["ShopID"] = new SelectList(_context.Shops, "ID", "ID", model.ShopID);
            return View();
        }


        public PartialViewResult AddProduct()
        {
          return PartialView("CreateProduct");
        }
    [HttpPost]
        public async Task<ViewResult> AddProduct(CreateProductViewModel model)
        {
            Product product;
            if (model.isNew)
            {
                product = new Product()
                {
                    Name = model.Name
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                model.ProductID = product.ID;
                foreach(var category in model.Categories)
                {
                    var productcategory = new ProductCategory()
                    {
                        ProductID = product.ID,
                        CategoryID = category
                    };
                    _context.ProductCategories.Add(productcategory);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                product = _context.Products.FirstOrDefault(p => p.ID == model.ProductID);
            }
            var addModel = new AddProductViewModel()
            {
                Product = product,
                Amount = model.Amount,
                Price = model.Price
            };
            return View(addModel);
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
            ViewData["ShopID"] = new SelectList(_context.Shops, "ID", "ID", receipt.ShopID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", receipt.UserID);
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
            ViewData["ShopID"] = new SelectList(_context.Shops, "ID", "ID", receipt.ShopID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", receipt.UserID);
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
