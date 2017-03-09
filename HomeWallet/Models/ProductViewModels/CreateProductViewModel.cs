using System.Collections.Generic;

namespace HomeWallet.Models.ProductViewModels
{
    public class CreateProductViewModel
    {
        public int ProductID {get;set;}
        public bool isNew {get;set;}
        public string Name {get;set;}
        public double Amount {get;set;}
        public double Price {get;set;}
        public ICollection<int> Categories = new List<int>();
    }
}
