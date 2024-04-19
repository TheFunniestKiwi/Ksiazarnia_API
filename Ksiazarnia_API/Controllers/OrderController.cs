using System.Net;
using Ksiazarnia_API.Data;
using Ksiazarnia_API.Models;
using Ksiazarnia_API.Models.DTO;
using Ksiazarnia_API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ksiazarnia_API.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ApiResponse _response;

        public OrderController(ApplicationDbContext context )
        {
            _context = context;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId)
        {
            try
            {
                var orderHeaders = _context.OrderHeaders
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Book).OrderByDescending(u => u.Id);

                _response.Result = !string.IsNullOrEmpty(userId)
                    ? orderHeaders.Where(u => u.UserId == userId)
                    : orderHeaders;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }

            return _response;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrderById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var orderHeader = await _context.OrderHeaders
                    .Include(u => u.OrderDetails)
                    .ThenInclude(u => u.Book).FirstOrDefaultAsync(u => u.Id == id);

                if (orderHeader == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = orderHeader;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }

            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto orderHeaderDto)
        {
            try
            {
                OrderHeader order = new()
                {
                    UserId = orderHeaderDto.UserId,
                    OrderTotal = orderHeaderDto.OrderTotal,
                    OrderDate = DateTime.Now,
                    PaymentIntentId = orderHeaderDto.PaymentIntentId,
                    TotalItems = orderHeaderDto.TotalItems,
                    Status = SD.status_pending
                };

                if (ModelState.IsValid)
                {
                    _context.OrderHeaders.Add(order);
                    await _context.SaveChangesAsync();
                    foreach (var orderDetailDto in orderHeaderDto.OrderDetailsDto)
                    {
                        OrderDetails orderDetails = new()
                        {
                            OrderHeaderId = order.Id,
                            BookTitle = orderDetailDto.BookTitle,
                            BookId = orderDetailDto.BookId,
                            Price = orderDetailDto.Price,
                            Quantity = orderDetailDto.Quantity
                        };
                        _context.OrderDetails.Add(orderDetails);
                    }
                    await _context.SaveChangesAsync();
                    _response.Result = order;
                    order.OrderDetails = null;
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
                }
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }

            return _response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateOrder(int id,
            [FromBody] OrderHeaderUpdateDto? orderHeaderUpdateDto)
        {
            try
            {
                if (orderHeaderUpdateDto == null || id != orderHeaderUpdateDto.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                OrderHeader? orderFromDb = await _context.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id);

                if (orderFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                if (!string.IsNullOrEmpty(orderHeaderUpdateDto.Status))
                {
                    orderFromDb.Status = orderHeaderUpdateDto.Status;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDto.PaymentIntentId))
                {
                    orderFromDb.PaymentIntentId = orderHeaderUpdateDto.PaymentIntentId;
                }

                await _context.SaveChangesAsync();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }
    }
}
