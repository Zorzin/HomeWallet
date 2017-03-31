using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeWallet.Models.ReceiptViewModels
{
    public class CreateReceiptViewModel
    {
        public DateTime Date {get;set;}
        public int ShopID {get;set;}
        public ICollection<AddProductViewModel> Products {get;set;} = new List<AddProductViewModel>();
    }
    public class CreateCyclicalReceiptViewModel
    {
        public int ShopID {get;set;}
        public ICollection<AddProductViewModel> Products {get;set;} = new List<AddProductViewModel>();
        public int Cycle {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate{get;set;}
    }
    public class AddProductViewModel
    {
      public Product Product { get; set; }
      public double Amount { get; set; }
      public double Price { get; set; }
      public double Total { get; set; }
    }
}
