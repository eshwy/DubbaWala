using Dabbawalla.Dto;
using Dabbawalla.Models;
using Dabbawalla.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dabbawalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly EmailListenerService _emailListenerService;
        public MenuController(MyDbContext context, EmailListenerService emailListenerService)
        {
            _context = context;
            _emailListenerService = emailListenerService;
        }

        //[Authorize]
        [HttpGet("GetAllMenu")]
        public async Task<IActionResult> GetAllMenu(string? pincode, int? foodType)
        {
            var foodItems = _context.FoodItems.AsNoTracking().ToList();
            var addresses = _context.Addresses.AsNoTracking().ToList();

            var foodDetails = from food in _context.FoodItems.AsNoTracking()
                              join address in _context.Addresses.AsNoTracking() on food.UserId.ToString() equals address.UserId.ToString()
                              where (foodType == null || food.FoodId == foodType) &&
                                    (pincode == null || address.PostalCode.Contains(pincode))
                              select new
                              {
                                  Id = food.Id,
                                  FoodName = food.Name,
                                  FoodDesc = food.Description,
                                  RestrauntName = _context.Users.Where(x => x.Id.ToString() == food.UserId.ToString()).FirstOrDefault().RestrauntName,
                                  Price = food.Price,
                                  Location = address.City,
                                  Area = address.Area,
                                  Pincode = address.PostalCode,
                                  FoodId = food.FoodId,
                                  Rating = food.Rating
                              };





            // ✉️ Trigger auto-reply email
            //await _emailListenerService.SendAutoReplyAsync(
            //    toEmail: "eshwanthkoku@gmail.com", // Replace with actual customer email
            //    subject: "Order Confirmation",
            //    userId: "user-123",
            //    password: "temporaryPwd123",
            //    ticketId: "ticket-456",
            //    cancellationToken: HttpContext.RequestAborted
            //);

            return Ok(foodDetails.ToList());
        }
    }
}
