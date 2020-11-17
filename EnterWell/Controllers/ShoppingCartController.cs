using EnterWell.Models;
using EnterWell.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterWell.Controllers
{
    public class ShoppingCartController : Controller
    {
        ShoppingStoreEntities StoreDB = new ShoppingStoreEntities();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            var Cart = ShoppingCart.GetCart(this.HttpContext);

            var ViewModel = new ShoppingCartViewModel
            {
                CartItems = Cart.GetCartItems(),
                CartTotal = Cart.GetTotal(),
                //CartTotalWithPDV = Cart.GetTotalWithPDV()
            };
            return View(ViewModel);
        }

        public ActionResult AddToCart(int id)
        {
            var AddedItem = StoreDB.Items.Single(item => item.ItemId == id);

            var Cart = ShoppingCart.GetCart(this.HttpContext);
            Cart.AddToCart(AddedItem);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var Cart = ShoppingCart.GetCart(this.HttpContext);

            string ItemName = StoreDB.Carts.Single(item => item.RecordId == id).item.Title;

            int ItemCount = Cart.RemoveFromCart(id);

            var Results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(ItemName) +
                " has been removed from your shopping cart.",
                CartTotal = Cart.GetTotal(),
                CartCount = Cart.GetCount(),
                ItemCount = ItemCount,
                DeleteId = id
            };
            return Json(Results);
        }
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var Cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = Cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}