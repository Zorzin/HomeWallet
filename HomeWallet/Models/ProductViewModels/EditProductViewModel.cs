using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWallet.Models.ProductViewModels
{
    public class EditProductViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public ICollection<int> Categories { get; set; }
    }
}
