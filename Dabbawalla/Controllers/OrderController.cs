using Dabbawalla.Models;
using Dabbawalla.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading;

namespace Dabbawalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly EmailListenerService _emailListenerService;
        public OrderController(MyDbContext context, EmailListenerService emailListenerService)
        {
            _context = context;
            _emailListenerService = emailListenerService;
        }

        [HttpGet("OrderPlacingAndAddressDetails")]
        public IActionResult OrderPlacingAndAddressDetails([FromQuery] int foodId, [FromQuery] string? userLogedIn)
        {
            var foodDetails = _context.FoodItems.Where(x => x.Id == foodId).FirstOrDefault();

            var addressDetails = _context.Addresses.Where(x => x.UserId.ToString() == userLogedIn.ToString()).ToList();

            var userDetails = _context.Users.Where(x => x.Id.ToString() ==  userLogedIn).FirstOrDefault();

            var details = new
            {
                UserId = userDetails.Id,
                FoodId = foodDetails.Id,
                Name = userDetails.Name,
                Phone = userDetails.PhoneNumber,
                Addresses = addressDetails.ToList(),
                Price = foodDetails.Price,
                MealType = _context.FoodTypes.Where(x => x.FoodId == foodDetails.FoodId).Select(x => x.Name).FirstOrDefault()
            };

            return Ok(details);
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder(string userId, int adressId, int Quantity, int foodId, decimal amount, CancellationToken? cancellationToken)
        {
            var vendorFoodDetails = _context.FoodItems.FirstOrDefault(x => x.Id == foodId);
            var coustomerDetails = _context.Users.FirstOrDefault(x => x.Id.ToString() == userId);
            var vendorDetails = _context.Users.FirstOrDefault(x => x.Id.ToString() == vendorFoodDetails.UserId.ToString());
            var address = _context.Addresses.FirstOrDefault(x => x.Id == adressId);

            string fullAddress = $"{address.DoorNumber}, {address.Street}, {address.Area}, {address.City}, {address.State}, {address.PostalCode}";

            var orderDetails = new OrderDetail()
            {
                UserId = userId,
                FoodId = foodId,
                Quantity = Quantity,
                Price = amount,
                AddressId = adressId
            };
            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();

            var paymentDetails = new PaymentDetail()
            {
                OrderId = orderDetails.Id,
                PaymentMode = "COD",
                Price = amount,
                TransactionStatus = "In Progress",
                CreatedDate = DateTime.Now
            };
            _context.PaymentDetails.Add(paymentDetails);
            _context.SaveChanges();

            await _emailListenerService.SendVendorMail(
                vendorDetails.EmailAddress,
                vendorDetails.Name,
                vendorFoodDetails.Name,
                Quantity,
                fullAddress,
                CancellationToken.None
            );

            await _emailListenerService.SendCoustomerMail(
                coustomerDetails.EmailAddress,
                coustomerDetails.Name,
                vendorFoodDetails.Name,
                Quantity,
                amount,
                CancellationToken.None
            );

            return Ok("Order Placed");
        }

    }
}
