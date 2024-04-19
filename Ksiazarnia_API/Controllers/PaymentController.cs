using System.Net;
using Ksiazarnia_API.Data;
using Ksiazarnia_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Ksiazarnia_API.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApiResponse _response;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public PaymentController( IConfiguration configuration, ApplicationDbContext context)
        {
            _response = new();
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {
            ShoppingCart? shoppingCart = await  _context.ShoppingCarts
                .Include(u => u.CartItems)
                .ThenInclude(u => u.Book)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (shoppingCart == null || shoppingCart.CartItems.Count == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            #region Create Payment Intent

            StripeConfiguration.ApiKey = _configuration.GetValue<string>("StripeSettings:Secret");
            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.Book.Price);
            var options = new PaymentIntentCreateOptions
            {
                Amount = (int)(shoppingCart.CartTotal * 100),
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                }
            };

            var service = new PaymentIntentService();
            PaymentIntent response = await service.CreateAsync(options);
            shoppingCart.PaymentIntent = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;

            #endregion

            _response.Result = shoppingCart;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
