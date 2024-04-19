using System.Net;
using Ksiazarnia_API.Data;
using Ksiazarnia_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ksiazarnia_API.Controllers
{
    [Route("api/ShoppingCart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApiResponse _response;
        private readonly ApplicationDbContext _context;

        public ShoppingCartController( ApplicationDbContext context)
        {
            _response = new();
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
        {
            try
            {
                ShoppingCart? shoppingCart;
                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new();
                }
                else
                {
                    shoppingCart = _context.ShoppingCarts
                        .Include(u => u.CartItems)
                        .ThenInclude(u => u.Book)
                        .FirstOrDefault();
                }


                if (shoppingCart == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.Book.Price);

                _response.Result = shoppingCart;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess =  false;
                _response.ErrorMessages.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddUpdateItem(string userId, int bookId, int updateQuantity)
        {
            ShoppingCart? shoppingCart = _context.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
            Book? book = _context.Books.FirstOrDefault(u => u.Id ==  bookId);

            if (book == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            if (shoppingCart == null & updateQuantity > 0)
            {
                ShoppingCart newCart = new() { UserId = userId};
                _context.ShoppingCarts.Add(newCart);
                await  _context.SaveChangesAsync();

                CartItem newCartItem = new()
                {
                    BookId = bookId,
                    Quantity = updateQuantity,
                    ShoppingCartId = newCart.Id,
                    Book = null
                };

                _context.CartItems.Add(newCartItem);
            }
            else
            {
                CartItem cartItemCart = shoppingCart.CartItems.FirstOrDefault(u => u.BookId == bookId);
                if (cartItemCart == null)
                {
                    CartItem newCartItem = new()
                    {
                        BookId = bookId,
                        Quantity = updateQuantity,
                        ShoppingCartId = shoppingCart.Id,
                        Book = null
                    };
                    _context.CartItems.Add(newCartItem);
                }
                else
                {
                    int newQuantity = cartItemCart.Quantity + updateQuantity;
                    if (newQuantity <= 0)
                    {
                        _context.CartItems.Remove(cartItemCart);
                        if (shoppingCart.CartItems.Count() == 1)
                        {
                            _context.ShoppingCarts.Remove(shoppingCart);
                        }
                    }
                    else
                    {
                        cartItemCart.Quantity = newQuantity;
                    }
                }
            }
            await _context.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
