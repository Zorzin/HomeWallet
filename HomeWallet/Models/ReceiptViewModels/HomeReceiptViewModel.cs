using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWallet.Models.ReceiptViewModels
{
    public class HomeReceiptViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public ICollection<ShowReceiptViewModel> Receipts { get; set; } = new List<ShowReceiptViewModel>();
    }

    public class ShowReceiptViewModel
    {
        public int ID { get; set; }
        public string Shop { get; set; }
        public double Value { get; set; }

    }
}
