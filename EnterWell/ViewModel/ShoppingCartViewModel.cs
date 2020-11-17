using EnterWell.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnterWell.ViewModel
{
    public class ShoppingCartViewModel
    {
        [Key]
        public int id { get; set; }

        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public decimal CartTotalWithPDV { get; set; }
    }
}