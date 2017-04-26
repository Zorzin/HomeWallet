using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWallet.Models.ShopViewModels
{
    public class ShopDetailsViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<string> Products { get; set; }
    }
}
