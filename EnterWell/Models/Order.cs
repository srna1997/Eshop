using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterWell.Models
{
    [Bind(Exclude = "OrderId")]
    public class Order
    {
        [Display(Name ="Bill number")]
        public int OrderId { get; set; }

        [Display(Name = "Date")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Due date")]
        public System.DateTime OrderDueDate { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "For pay(+PDV 25%)")]
        public decimal TotalWithPDV { get; set; }

        [Display(Name = "Customer")]
        public string UserName { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}