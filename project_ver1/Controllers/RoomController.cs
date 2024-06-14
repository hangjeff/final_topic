using Microsoft.AspNetCore.Mvc;
using project_ver1.Models;
using System;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyWebsite.Extensions;

namespace project_ver1.Controllers
{
    public class RoomController : Controller
    {
        private readonly HomeDbContext _context;

        //OrderData orderData declaration
        private OrderData orderData;

        public RoomController(HomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            SetUserViewBag();
            // Retrieve room data from the database
            var rooms = await _context.Rooms
                .Include(r => r.Category) // Include related category data if needed
                .ToListAsync();

            // Pass room data to the view
            return View(rooms);
        }

        [HttpPost]
        public ActionResult SearchRooms(DateTime checkInDate, DateTime checkOutDate, int roomType)
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                // 獲取所有房間
                var allRooms = _context.Rooms
                                   .Where(r => r.CategoryID == roomType)
                                   .ToList();

                // 獲取已預訂的房間ID
                var bookedRoomIds = (from rs in _context.Rooms
                                     join orderDetails in _context.RoomOrderDetails on rs.ID equals orderDetails.RoomID
                                     join order in _context.RoomOrder on orderDetails.OrderID equals order.ID
                                     where order.CheckIn < checkOutDate && order.CheckOut > checkInDate
                                     select orderDetails.RoomID).ToList();

                // 獲取可用房間
                var availableRooms = allRooms.Where(r => !bookedRoomIds.Contains(r.ID)).ToList();

                // 將可用房間轉換成 RoomViewModel
                var availableRoomViewModels = availableRooms.Select(r => new Rooms
                {
                    ID = r.ID,
                    Price = r.Price,
                    Image = r.Image

                }).ToList();
                ViewBag.CheckInDate = checkInDate;
                ViewBag.CheckOutDate = checkOutDate;
                return View(availableRoomViewModels);
            }
            else
            {
                return Redirect("/Home/member");
            }
        }

        public IActionResult BookRoom(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                ViewBag.CheckInDate = checkInDate;
                ViewBag.CheckOutDate = checkOutDate;
                var rooms = _context.Rooms.Include(r => r.Category).FirstOrDefault(r => r.ID == roomId);

                return View(rooms);
            }
            else
            {
                return Redirect("/Home/member");
            }

        }
        public IActionResult ConfirmBooking()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                SetUserViewBag();
                return View();
            }
            else
            {
                return Redirect("/Home/member");
            }
        }

        public IActionResult ConfirmBooking(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (HttpContext.Session.GetObject<OrderData>("room") == null)
                {
                    // create a new orderData
                    orderData = new OrderData();
                    orderData.CustomerID = (int)HttpContext.Session.GetInt32("UserId");
                    orderData.OrderTime = DateTime.Now;
                    orderData.CheckIn = checkInDate;
                    orderData.CheckOut = checkOutDate;
                    orderData.OrderFinished = false;
                    orderData.EmployeeID = null;
                    orderData.SumPrice = 0;
                }
                else
                {
                    orderData = HttpContext.Session.GetObject<OrderData>("room");
                }

                DetailData detailData = new DetailData
                {
                    RoomID = roomId,
                    Price = 1800
                };


                var roomList = new List<int>();
                ViewBag.CheckInDate = checkInDate;
                ViewBag.CheckOutDate = checkOutDate;
                return View();
            }
            else
            {

                return Redirect("/Home/member");
            }
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
        public class OrderData : Room_Order
        {

            public List<DetailData> Details { get; set; }
            public OrderData()
            {
                Details = new List<DetailData>();
            }
        }
        public class DetailData : Room_Order_Details
        {

        }


    }

}









