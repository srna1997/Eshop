using EnterWell.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterWell.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ShoppingStoreEntities StoreDB = new ShoppingStoreEntities();

        public CheckoutController(IPDVService pdv)
        {
            this.Service = pdv;
        }

        public IPDVService Service { get; set; }

        public ActionResult AddressPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddressPayment(FormCollection values)
        {
            
            var order = new Order();
            TryUpdateModel(order);

            try
            {

                var cart = ShoppingCart.GetCart(this.HttpContext);
 
                order.UserName = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                order.OrderDueDate = DateTime.Now.AddDays(3);
                order.Total = cart.GetTotal();
                order.TotalWithPDV = cart.GetTotal() * this.Service.PDV();

                StoreDB.Orders.Add(order);
                StoreDB.SaveChanges();

                    
                cart.CreateOrder(order);

                return RedirectToAction("Complete",
                    new { id = order.OrderId });
              
            }
            catch
            {

                return View(order);
            }
        }

        public ActionResult Complete(int id)
        {

            bool isValid = StoreDB.Orders.Any(
                o => o.OrderId == id &&
                o.UserName == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

    }
}