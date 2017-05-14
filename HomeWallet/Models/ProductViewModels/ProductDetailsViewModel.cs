using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWallet.Models.ProductViewModels
{
    public class ProductDetailsViewModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public double TotalSpent { get; set; }
        public int TimesBought { get; set; }
        public double AmountBought { get; set; }
        public double AverageCost { get; set; }
        public DateTime LastBought { get; set; }
        public double MostBoughtAtOnce { get; set; }
    }

    public class ProductShopChartModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }

    public class ProductShopAverageChartModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Cost { get; set; }
    }
}
