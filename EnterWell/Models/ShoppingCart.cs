using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterWell.Models
{
    public class ShoppingCart
    {
        ShoppingStoreEntities StoreDB = new ShoppingStoreEntities();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var Cart = new ShoppingCart();
            Cart.ShoppingCartId = Cart.GetCartId(context);

            return Cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Item Item)
        {
            var CartItem = StoreDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ItemId == Item.ItemId);
            if (CartItem == null)
            {
                CartItem = new Cart
                {
                    ItemId = Item.ItemId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                StoreDB.Carts.Add(CartItem);
            }
            else
            {
                CartItem.Count++;
            }
            StoreDB.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var CartItem = StoreDB.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (CartItem != null)
            {
                if (CartItem.Count > 1)
                {
                    CartItem.Count--;
                    itemCount = CartItem.Count;
                }
                else
                {
                    StoreDB.Carts.Remove(CartItem);
                }
                StoreDB.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var CartItems = StoreDB.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var CartItem in CartItems)
            {
                StoreDB.Carts.Remove(CartItem);
            }
            StoreDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return StoreDB.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from CartItems in StoreDB.Carts
                          where CartItems.CartId == ShoppingCartId
                          select (int?)CartItems.Count).Sum();
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (
                from CartItems in StoreDB.Carts
                where CartItems.CartId == ShoppingCartId
                select (int?)CartItems.Count *
                CartItems.item.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal OrderTotal = 0;

            var CartItems = GetCartItems();

            foreach (var item in CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ItemId = item.ItemId,
                    OrderId = order.OrderId,
                    UnitPrice = item.item.Price,
                    Quantity = item.Count,
                    Total = item.Count * item.item.Price
                };

                OrderTotal += (item.Count * item.item.Price);

                StoreDB.OrderDetails.Add(orderDetail);
            }
            order.Total = OrderTotal;

            StoreDB.SaveChanges();
            EmptyCart();

            return order.OrderId;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();

                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string Email)
        {
            var ShoopingCart = StoreDB.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in ShoopingCart)
            {
                item.CartId = Email;
            }
            StoreDB.SaveChanges();
        }
    }

    
}