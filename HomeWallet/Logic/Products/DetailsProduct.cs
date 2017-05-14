using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Highsoft.Web.Mvc.Charts;
using HomeWallet.Data;
using HomeWallet.Models.ProductViewModels;
using Microsoft.EntityFrameworkCore;

namespace HomeWallet.Logic.Products
{
    public static class DetailsProduct
    {
        public static ProductDetailsViewModel GetModel(int id, ApplicationDbContext context, DateTime startDate, DateTime endDate)
        {
            var product = context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefault(p => p.ID == id);
            var model = new ProductDetailsViewModel()
            {
                ID = product.ID,
                Name = product.Name,
                ProductCategories = product.ProductCategories,
                TotalSpent = GetTotalSpent(id,context,startDate, endDate),
                TimesBought = GetTimesBought(id,context,startDate,endDate),
                AmountBought = GetAmountBought(id,context,startDate,endDate),
                AverageCost = GetAverageCost(id,context,startDate,endDate),
                LastBought = GetLastBought(id,context),
                MostBoughtAtOnce = GetMostBoughtAtOnce(id,context, startDate, endDate)
            };
            return model;
        }

        public static double GetTotalSpent(int id, ApplicationDbContext context, DateTime startDate, DateTime endDate)
        {
            var receipts =
                context.ReceiptProducts.Where(
                    rp => rp.ProductID == id 
                          && rp.Receipt.PurchaseDate >= startDate 
                          && rp.Receipt.PurchaseDate <= endDate);
            double result = 0;
            foreach (var receipt in receipts)
            {
                result += receipt.Amount * receipt.Price;
            }

            return Math.Round(result,1);
        }

        public static int GetTimesBought(int id, ApplicationDbContext context, DateTime startDate, DateTime endDate)
        {
            var receipts =
                context.ReceiptProducts.Where(
                    rp => rp.ProductID == id 
                          && rp.Receipt.PurchaseDate >= startDate 
                          && rp.Receipt.PurchaseDate <= endDate).Select(rp=>rp.Receipt.ID).Distinct().Count();
            return receipts;
        }

        public static double GetAmountBought(int id, ApplicationDbContext context, DateTime startDate, DateTime endDate)
        {
            var receipts =
                context.ReceiptProducts.Where(
                    rp => rp.ProductID == id
                          && rp.Receipt.PurchaseDate >= startDate
                          && rp.Receipt.PurchaseDate <= endDate);
            double result=0;
            foreach (var receipt in receipts)
            {
                result += receipt.Amount;
            }
            return Math.Round(result,1);
        }
        
        public static double GetAverageCost(int id, ApplicationDbContext context, DateTime startDate, DateTime endDate)
        {
            var totalcost = GetTotalSpent(id, context, startDate, endDate);
            var totalbought = GetAmountBought(id, context, startDate, endDate);
            return Math.Round(totalcost / totalbought,1);
        }

        public static DateTime GetLastBought(int id, ApplicationDbContext context)
        {
            var receipts = context.ReceiptProducts.Where(rp => rp.ProductID == id).Select(rp=>rp.Receipt.PurchaseDate).OrderByDescending(r=>r.Date).Distinct().Take(1).ToList();
            return receipts.ElementAt(0);
        }

        public static double GetMostBoughtAtOnce(int id, ApplicationDbContext context, DateTime startDate,
            DateTime endDate)
        {
            var receipts =
                context.ReceiptProducts.Where(
                    rp => rp.ProductID == id
                          && rp.Receipt.PurchaseDate >= startDate
                          && rp.Receipt.PurchaseDate <= endDate).Distinct().OrderByDescending(p=>p.Amount).Select(p=>p.Amount).Take(1).ToList();
            var result = receipts.ElementAt(0);
            return Math.Round(result,1);
        }
    
        public static List<ColumnSeriesData> GetShopTimesChartData(int id, ApplicationDbContext context, DateTime startDate,
            DateTime endDate)
        {
            var result = new List<ColumnSeriesData>();
            var shops = new List<ProductShopChartModel>();
            var receipts = context.ReceiptProducts
                .Include(rp => rp.Receipt)
                .ThenInclude(r => r.Shop)
                .Where(rp => rp.ProductID == id
                             && rp.Receipt.PurchaseDate >= startDate
                             && rp.Receipt.PurchaseDate <= endDate);
            foreach (var receipt in receipts)
            {
                var shop = receipt.Receipt.Shop.ID;
                var model = shops.FirstOrDefault(s => s.ID == shop);
                if ( model!=null)
                {
                    model.Amount += receipt.Amount;
                }
                else
                {
                    shops.Add(new ProductShopChartModel()
                    {
                        ID = receipt.Receipt.ShopID,
                        Name = receipt.Receipt.Shop.Name,
                        Amount = receipt.Amount
                    });
                }
            }
            foreach (var shop in shops)
            {
                result.Add(new ColumnSeriesData()
                {
                    Name = shop.Name,
                    Y = shop.Amount
                });
            }
            return result;
        }

        public static List<ColumnSeriesData> GetShopAverageChartData(int id, ApplicationDbContext context, DateTime startDate,
                DateTime endDate)
        {
            var result = new List<ColumnSeriesData>();
            var shops = new List<ProductShopAverageChartModel>();
            var receipts = context.ReceiptProducts
                .Include(rp => rp.Receipt)
                .ThenInclude(r => r.Shop)
                .Where(rp => rp.ProductID == id
                             && rp.Receipt.PurchaseDate >= startDate
                             && rp.Receipt.PurchaseDate <= endDate);
            foreach (var receipt in receipts)
            {
                var shop = receipt.Receipt.Shop.ID;
                var model = shops.FirstOrDefault(s => s.ID == shop);
                if (model != null)
                {
                    model.Amount += receipt.Amount;
                    model.Cost += receipt.Price * receipt.Amount;
                }
                else
                {
                    shops.Add(new ProductShopAverageChartModel()
                    {
                        ID = receipt.Receipt.ShopID,
                        Name = receipt.Receipt.Shop.Name,
                        Amount = receipt.Amount,
                        Cost = receipt.Price * receipt.Amount
                    });
                }
            }
            foreach (var shop in shops)
            {
                result.Add(new ColumnSeriesData()
                {
                    Name = shop.Name,
                    Y = shop.Cost/shop.Amount
                });
            }
            return result;
        }

        public static List<PieSeriesData> GetPlanChartData(int id, ApplicationDbContext context, DateTime startDate,
                DateTime endDate, string userId)
        {
            var plans = context.Plans.Where(p => p.UserID == userId && p.EndDate <= endDate && p.StartDate >= startDate);
            double totalplans = 0;
            foreach (var plan in plans)
            {
                totalplans += plan.Amount;
            }

            var totalproduct = GetTotalSpent(id, context, startDate, endDate);

            var result = new List<PieSeriesData>()
            {
                new PieSeriesData()
                {
                    Name = context.Products.FirstOrDefault(p=>p.ID == id).Name,
                    Y = totalproduct,
                    Sliced = true,
                    Selected = true,
                    Color = "#222"
                },
                new PieSeriesData()
                {
                    Name = "Other products",
                    Y = totalplans - totalproduct,
                    Sliced = true,
                    Selected = true,
                    Color = "white"
                }
            };

            return result;
        }
    }
}
