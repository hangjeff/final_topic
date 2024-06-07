using DapperProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using WebApplication1.Models;
using System.Data.SqlClient;
using MobileDAL.cs;
using Microsoft.CodeAnalysis;
using NuGet.Protocol.Core.Types;
using DapperNews.NAH;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using project_ver1.Models;


namespace project_ver1.Models
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        MobiledetailDAL _mdal = new MobiledetailDAL();
        DataTable dt;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly HomeDbContext _context;

        //新增AppDbContext 
        public HomeController(HomeDbContext context)
        {
            //_logger = logger; 
            _context = context;

            //新增

        }


        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult EachProductDetails(string productId)
        //{
        //    if (!int.TryParse(productId, out int id))
        //    {
        //        // Handle invalid product ID
        //        return RedirectToAction("Error");
        //    }

        //    string mycmd = $"SELECT m.ProductID, m.FruitsName, m.price, m.url, md.Features, md.made, md.delivery " +
        //                   $"FROM Fruits m INNER JOIN FruitsDetails md ON m.ProductID = md.FruitsId " +
        //                   $"WHERE m.ProductID = {id}";

        //    DataTable dt = _mdal.SelactAll(mycmd);

        //    List<ProductDetails> list = new List<ProductDetails>();

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        ProductDetails mob = new ProductDetails
        //        {
        //            FruitsName = dt.Rows[i]["FruitsName"].ToString(),
        //            Price = Convert.ToDouble(dt.Rows[i]["Price"]),
        //            Url = dt.Rows[i]["Url"].ToString(),
        //            Features = dt.Rows[i]["Features"].ToString(),
        //            delivery = dt.Rows[i]["delivery"].ToString(),
        //            ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"])
        //        };

        //        list.Add(mob);
        //    }

        //    return View(list);
        //}

        public IActionResult EachProductDetails(Mobiles m)
        {
            m.ProductID = Convert.ToInt32(m.ProductID);
            string mycmd = "select m.ProductID,m.FruitsName,m.price,m.url,md.Features,md.made,md.delivery from Fruits m inner join FruitsDetails md on m.ProductID=md.FruitsId where m.ProductID=" + m.ProductID +
                           "UNION ALL " +
                           "select x.ProductID,x.FruitsName,x.price,x.url,td.Features,td.made,td.delivery from Tea x inner join TeaDetails td on x.ProductID=td.FruitsId where x.ProductID=" + m.ProductID + "";
            dt = new DataTable();

            dt = _mdal.SelactAll(mycmd);

            List<ProductDetails> list = new List<ProductDetails>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductDetails mob = new ProductDetails();

                mob.FruitsName = dt.Rows[i]["FruitsName"].ToString();
                mob.Price = Convert.ToDouble(dt.Rows[i]["Price"]);
                mob.Url = dt.Rows[i]["Url"].ToString();
                mob.Features = dt.Rows[i]["Features"].ToString();
                mob.delivery = dt.Rows[i]["delivery"].ToString();
                mob.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);

                list.Add(mob);
            }

            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SearchResults(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // If the query is empty, return an error view
                return RedirectToAction("Error");
            }

            // Construct the search query based on the product name
            string searchQuery = "select m.ProductID,m.FruitsName,m.price,m.url,md.Features,md.made,md.delivery from Fruits m inner join FruitsDetails md on m.ProductID=md.FruitsId where m.FruitsName LIKE @productName " +
                           "UNION ALL " +
                           "select x.ProductID,x.FruitsName,x.price,x.url,td.Features,td.made,td.delivery from Tea x inner join TeaDetails td on x.ProductID=td.FruitsId where x.FruitsName LIKE @productName";

            // Execute the search query using Dapper and retrieve the results
            var searchResultsList = _mdal.Query<ProductDetails>(searchQuery, new { productName = "%" + query + "%" });

            return View(searchResultsList);
        }

        public IActionResult Search(string query)
        {
            // Forward the query parameter to SearchResult action
            return RedirectToAction("SearchResults", new { query });
        }


        public IActionResult commodity(string category = "Fruits")
        {
            // Construct SQL query based on the selected category
            string mycmd = "SELECT * FROM " + category;
            DataTable dt = _mdal.SelactAll(mycmd);

            List<Mobiles> list = new List<Mobiles>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Mobiles mob = new Mobiles();
                mob.slno = Convert.ToInt32(dt.Rows[i]["slNo"]);
                mob.FruitsName = dt.Rows[i]["FruitsName"].ToString();
                mob.Price = Convert.ToDouble(dt.Rows[i]["Price"]);
                mob.Url = dt.Rows[i]["Url"].ToString();
                mob.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                list.Add(mob);
            }

            return View(list);
        }

        public IActionResult News(string category = "News")
        {
            string mycmd = "SELECT * FROM " + category;
            DataTable dt = _mdal.SelactAll(mycmd);

            List<Newbie> list = new List<Newbie>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Newbie mob = new Newbie();
                mob.slno = Convert.ToInt32(dt.Rows[i]["slNo"]);
                mob.Header = dt.Rows[i]["Header"].ToString();
                mob.News = dt.Rows[i]["News"].ToString();
                mob.Url = dt.Rows[i]["Url"].ToString();
                mob.Date = dt.Rows[i]["Date"].ToString();
                mob.Description = dt.Rows[i]["Description"].ToString();
                list.Add(mob);
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult PlaceOrder(List<Mobiles> cartItems)
        {
            // Check if the cartItems collection is null or empty
            if (cartItems == null || !cartItems.Any())
            {
                // Handle the case where the shopping cart is empty
                return RedirectToAction("EmptyCart");
            }

            // Iterate over each item in the cart
            foreach (var item in cartItems)
            {
                // Example query to insert order details
                string query = "INSERT INTO OrderDetail (slno, Quantity, Price, Ordertime) VALUES (@slno, @Quantity, @Price, @Ordertime)";

                // Parameters for the query
                var parameters = new
                {
                    item.slno,
                    item.Ordertime,
                    item.Quantity,
                    item.Price
                };

                // Execute the query using your DAL
                _mdal.DMLOpperation(query, parameters);
            }

            // Clear the shopping cart after placing the order
            HttpContext.Session.Remove("cart");

            // Set a message to indicate successful order placement
            TempData["OrderPlacedMessage"] = "Your order has been placed successfully!";

            // Redirect to the order confirmation page
            return RedirectToAction("OrderConfirmation", "Home");
        }




        public IActionResult OrderConfirmation()
        {
            return View();
        }






        //=========================================================================

        public ActionResult Park_information()
        {
            SetUserViewBag();
            return View();
        }


        public ActionResult member()
        {
            return View();
        }
        //[HttpPost]
        public IActionResult About()
        {
            SetUserViewBag();
            return View();
        }

        [HttpPost]
        public IActionResult About(Contact appdb)
        {
            DateTime now = DateTime.Now;
            Contact contact = new Contact
            {
                ID = appdb.ID,
                QuestionCategory = appdb.QuestionCategory,
                Name = appdb.Name,
                Sex = appdb.Sex,
                Phone = appdb.Phone,
                QuestionContent = appdb.QuestionContent,
                SetTime = now
            };
            _context.Contact.Add(contact);
            _context.SaveChanges();
            
            return Redirect("/Homo/Index");



        }

        public IActionResult Account()
        {

            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                return RedirectToAction("Members_Information", "Homo");
            }
            else if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                return RedirectToAction("Backstage", "Homo");
            }
            else
            {

                return RedirectToAction("member", "Homo");
            }


        }
        public IActionResult Backstage()
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                var backcontact = _context.Contact
                                     .Where(item => !item.Finished)
                                     .ToList();

                return View(backcontact);
            }
            else
            {
                return RedirectToAction("member", "Homo");
            }
        }

        [HttpPost]
        public IActionResult ContactEdit(int contactId)
        {

            var contactToUpdate = _context.Contact.FirstOrDefault(c => c.ID == contactId);

            if (contactToUpdate != null)
            {

                contactToUpdate.Finished = true;


                _context.SaveChanges();
            }


            return RedirectToAction("Backstage");
        }
        public IActionResult Members_join()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Members_join(Customer myuser)
        {
            ViewBag.DuplicateMessage = "該電子信箱已被註冊";

          
            var existingCustomer = _context.Customer.FirstOrDefault(c => c.Email == myuser.Email);
            if (existingCustomer != null)
            {
                //ModelState.AddModelError("Email", "該電子信箱已經被註冊。");
                return View(myuser);
            }

            // 儲存用戶註冊資訊到資料庫
            _context.Customer.Add(myuser);
            _context.SaveChanges();
            HttpContext.Session.SetString("UserName", myuser.Name);
            HttpContext.Session.SetInt32("UserId", myuser.ID);

            TempData["SuccessMessage"] = "註冊成功";
  
            //return View("Members_Information", "Home");

           
            return View(myuser);
        }



        public IActionResult Members_Information()
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var user = _context.Customer.FirstOrDefault(u => u.ID == userId);
                return View(user);
            }
            else
            {
                return RedirectToAction("member", "Homo");
            }

        }

        [HttpPost]
        public IActionResult member(LoginViewModel model)
        {
            var user = _context.Customer.FirstOrDefault(u => u.Email == model.Email && u.Pwd == model.Password);
            var employee = _context.Employee.FirstOrDefault(u => u.Email == model.Email && u.Pwd == model.Password);

            if (employee != null)
            {
                HttpContext.Session.SetInt32("EmployeeId", employee.ID);
                HttpContext.Session.SetString("EmployeeName", employee.Name);
                return RedirectToAction("Backstage", "Homo");
            }
            else
            {
                if (user != null)
                {
                   
                    //HttpContext.Session.SetString("key", "value");
                    HttpContext.Session.SetInt32("UserId", user.ID);
                    HttpContext.Session.SetString("UserName", user.Name);

                    return RedirectToAction("Members_Information", "Homo");
                }
                else
                {
                    ViewBag.ErrorMessage = "帳號或密碼不正確";
                    return View();
                }
            }
        }

        public IActionResult Logout()
        {
           
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("EmployeeId");
            HttpContext.Session.Remove("EmployeeName");
            return RedirectToAction("Index", "Homo");
        }


        private void SetUserViewBag()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userName = HttpContext.Session.GetString("UserName");
                var userId = HttpContext.Session.GetInt32("UserId");

                ViewBag.UserName = userName;
                ViewBag.UserId = userId;
            }

            if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                var EmployeeName = HttpContext.Session.GetString("EmployeeName");
                var EmployeeId = HttpContext.Session.GetInt32("EmployeeId");

                ViewBag.UserName = EmployeeName;
                ViewBag.UserId = EmployeeId;
            }
        }




        //===================================================================================





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
