using System;
using System.Collections.Generic;

namespace HomeWallet.Models.ReceiptViewModels
{
    public class CreateReceiptViewModel
    {
        public DateTime Date {get;set;}
        public int ShopID {get;set;}
        public ICollection<AddProductViewModel> Products {get;set;} = new List<AddProductViewModel>();
    }
    public class AddProductViewModel
    {
      public Product Product { get; set; }
      public double Amount { get; set; }
      public double Price { get; set; }
    }
}
