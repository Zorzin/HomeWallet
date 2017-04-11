using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWallet.Models.ReceiptViewModels
{
    public class ReceiptDetailsViewModel
    {
        public int ID { get; set; }
        public DateTime PurchaseDate {get;set;}
        public string Shop {get;set;}
        public double Total { get; set; }
        public ICollection<DetailsProductViewModel> Products {get;set;} = new List<DetailsProductViewModel>();

    }

    public class DetailsProductViewModel
    {
        public string Product { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
    }
}
