using EnterWell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterWell.Controllers
{
    public class StoreController : Controller
    {
        ShoppingStoreEntities StoreDB = new ShoppingStoreEntities();

        // GET: Store
        public ActionResult Index()
        {
            var Categories = StoreDB.Categories.ToList();
            return View(Categories);
        }
        public ActionResult Browse(string Category)
        {
            var CategoryModel = StoreDB.Categories.Include("Items")
                .Single(c => c.Name == Category);

            return View(CategoryModel);
        }

        public ActionResult Details(int id)
        {
            var Item = StoreDB.Items.Find(id);
            return View(Item);
        }

    }
}