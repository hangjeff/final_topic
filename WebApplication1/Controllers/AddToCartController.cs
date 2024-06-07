using DapperProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using MobileDAL.cs;

namespace WebApplication1.Controllers
{
    public class AddToCartController : Controller
    {
        private readonly MobiledetailDAL _mdal;

        public AddToCartController(MobiledetailDAL mdal)
        {
            _mdal = mdal;
        }

        public ActionResult Add(int id, int quantity, string name, string url, string features, string made, string delivery, double price)
        {
            Mobiles mo = new Mobiles
            {
                slno = id,
                Quantity = quantity,
                FruitsName = name,
                Url = url,
                Features = features,
                Made = made,
                Delivery = delivery,
                Price = price
            };

            if (HttpContext.Session.GetObject<List<Mobiles>>("cart") == null)
            {
                List<Mobiles> li = new List<Mobiles>();
                li.Add(mo);
                HttpContext.Session.SetObject("cart", li);
                ViewBag.cart = li.Count;
                HttpContext.Session.SetInt32("count", 1);
            }
            else
            {
                List<Mobiles> li = HttpContext.Session.GetObject<List<Mobiles>>("cart");
                li.Add(mo);
                HttpContext.Session.SetObject("cart", li);
                ViewBag.cart = li.Count;
                int count = HttpContext.Session.GetInt32("count") ?? 0;
                HttpContext.Session.SetInt32("count", count + 1);
            }
            return RedirectToAction("commodity", "Home");
        }


        public ActionResult Myorder()
        {
            var cartItems = HttpContext.Session.GetObject<List<Mobiles>>("cart");
            return View(cartItems);
        }

        public ActionResult Remove(Mobiles mob)
        {
            List<Mobiles> cart = HttpContext.Session.GetObject<List<Mobiles>>("cart");

            if (cart != null)
            {
                int indexToRemove = cart.FindIndex(x => x.slno == mob.slno);

                if (indexToRemove != -1)
                {
                    cart.RemoveAt(indexToRemove);
                    HttpContext.Session.SetObject("cart", cart);

                    int count = HttpContext.Session.GetInt32("count") ?? 0;
                    if (count > 0)
                    {
                        HttpContext.Session.SetInt32("count", count - 1);
                    }
                }
            }

            return RedirectToAction("Myorder", "AddToCart");
        }
    }
}


