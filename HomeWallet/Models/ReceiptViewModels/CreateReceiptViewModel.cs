using System;
using System.Collections.Generic;
public class CreateReceiptViewModel
{
  public DateTime Date {get;set;}
  public int ShopID {get;set;}
  public ICollection<AddProductViewModel> Products {get;set;} = new List<AddProductViewModel>();
 
}
