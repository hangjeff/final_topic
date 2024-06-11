using Microsoft.AspNetCore.Mvc;
using project_ver1.Models;
using System;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace project_ver1.Controllers
{
    public class RoomController : Controller
    {
        private readonly HomeDbContext _context;

        public RoomController(HomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve room data from the database
            var rooms = await _context.Rooms
                .Include(r => r.Category) // Include related category data if needed
                .ToListAsync();

            // Pass room data to the view
            return View(rooms);
        }

        [HttpPost]
        public ActionResult SearchRooms()
        {
            //// Query rooms based on room type and availability
            //var availableRooms = await _context.Rooms
            //    .Where(r => r.Category.Name == roomType &&
            //                r.HasReserved == false &&
            //                r.CanReserve == true)
            //    .ToListAsync();

            //// Filter available rooms based on check-in and check-out dates
            //foreach (var room in availableRooms.ToList())
            //{
            //    var reservationsForRoom = await _context.RoomOrder
            //        .Where(ro => ro.ID == room.ID &&
            //                      ro.CheckIn <= checkOutDate &&
            //                      ro.CheckOut >= checkInDate)
            //        .ToListAsync();

            //    if (reservationsForRoom.Any())
            //    {
            //        availableRooms.Remove(room);
            //    }
            //}

            //// Count the number of available rooms
            //int availableRoomCount = availableRooms.Count;

            //// Pass the number of available rooms to the view
            //ViewBag.AvailableRoomCount = availableRoomCount;


            var rb = from rs in _context.Rooms
                     join orderDetails in _context.RoomOrderDetails on rs.ID equals orderDetails.RoomID
                     join order in _context.RoomOrder on orderDetails.OrderID equals order.ID
                     where order.CheckIn > DateTime.Parse("2024-03-01 ")
                        && order.CheckOut < DateTime.Parse("2024-05-11 ")
                        //&& rs.CategoryID == 1
                        //&& !_context.RoomOrderDetails.Any(rod => rod.RoomID == rs.ID)
                        //&& !_context.RoomOrder.Any(ro =>
                        //     ro.CheckIn <= DateTime.Parse("2024-04-11 ") &&
                        //     ro.CheckOut >= DateTime.Parse("2024-04-01 "))

                     select new
                     {
                         CategoryID = rs.CategoryID,
                         CheckIn = order.CheckIn,
                         CheckOut = order.CheckOut,
                         ID = orderDetails.RoomID
                     };

            var RoombookingList = rb.ToList();



            return View(RoombookingList);
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


    }

}






