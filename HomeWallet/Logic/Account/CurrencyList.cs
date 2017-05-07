using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWallet.Models.CurrencyViewModels;

namespace HomeWallet.Logic.Account
{
    public static class CurrencyList
    {
        public static ICollection<CurrencyViewModel> GetList()
        {
            var list = new List<CurrencyViewModel>
            {
                new CurrencyViewModel()
                {
                    Name = "Polish złoty",
                    Value = "zł"
                },
                new CurrencyViewModel()
                {
                    Name = "America (United States) Dollars – USD",
                    Value = "$"
                },
                new CurrencyViewModel()
                {
                    Name = "Euro",
                    Value = "€"
                }
            };
            return list;
        }
    }
}
