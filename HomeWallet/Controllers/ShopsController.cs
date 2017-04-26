﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeWallet.Data;
using HomeWallet.Logic;
using HomeWallet.Models;
using HomeWallet.Models.ShopViewModels;
using Microsoft.AspNetCore.Identity;

namespace HomeWallet.Controllers
{
    public class ShopsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShopsController(ApplicationDbContext context, UserManager<ApplicationUser>  userManager)
        {
            _context = context;    
            _userManager = userManager;
        }

        // GET: Shops
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
            var shops = _context.Shops.Where(s => s.UserID == _userManager.GetUserId(HttpContext.User));

            if (!string.IsNullOrEmpty(searchString))
            {
                shops = shops.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    shops = shops.OrderByDescending(s => s.Name);
                    break;
                default:
                    shops = shops.OrderBy(s => s.Name);
                    break;
            }

            var pageSize = 10;
            return View(await PaginatedList<Shop>.CreateAsync(shops, page ?? 1, pageSize));
        
        }

        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s=>s.Receipts)
                .ThenInclude(r=>r.ReceiptProducts)
                .ThenInclude(rp=>rp.Product)
                .SingleOrDefaultAsync(m => m.ID == id);
            var products = new List<string>();
            var productsID = new List<int>();
            foreach (var shopReceipt in shop.Receipts)
            {
                foreach (var shopReceiptReceiptProduct in shopReceipt.ReceiptProducts)
                {
                    if (!productsID.Contains(shopReceiptReceiptProduct.ProductID))
                    {
                          productsID.Add(shopReceiptReceiptProduct.ProductID);
                          products.Add(shopReceiptReceiptProduct.Product.Name);
                    }
                }
            }
            
            var model = new ShopDetailsViewModel()
            {
                ID = (int)id,
                Name = shop.Name,
                Products = products
            };

            return View(model);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                shop.UserID = _userManager.GetUserId(HttpContext.User);
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(shop);
        }

        // GET: Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops.SingleOrDefaultAsync(m => m.ID == id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Shop shop)
        {
            if (id != shop.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.ID))
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
            return View(shop);
        }

        // GET: Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.Shops.SingleOrDefaultAsync(m => m.ID == id);
            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ShopExists(int id)
        {
            return _context.Shops.Any(e => e.ID == id);
        }
    }
}
