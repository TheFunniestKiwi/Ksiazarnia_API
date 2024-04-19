using Ksiazarnia_API.Data;
using Ksiazarnia_API.Models;
using Ksiazarnia_API.Models.DTO;
using Ksiazarnia_API.Services;
using Ksiazarnia_API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Ksiazarnia_API.Controllers
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ApiResponse _response;
        private readonly IBlobService _blobService;

        public BookController(ApplicationDbContext context, IBlobService blobService)
        {
            _context = context;
            _response = new ApiResponse();
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            _response.Result =  _context.Books.ToList();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            Book? book = _context.Books.FirstOrDefault(u => u.Id == id);
            if(book == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = book;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddBook([FromForm] BookCreateDto bookCreateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (bookCreateDto.File == null || bookCreateDto.File.Length == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(bookCreateDto.File.FileName)}";

                    /*To do: Change to automapper*/
                    Book book = new()
                    {
                        Title = bookCreateDto.Title,
                        Description = bookCreateDto.Description,
                        Author = bookCreateDto.Author,
                        Price = bookCreateDto.Price,
                        Publisher = bookCreateDto.Publisher,
                        Genre = bookCreateDto.Genre,
                        Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, bookCreateDto.File)
                    };

                    _context.Books.Add(book);
                    _context.SaveChanges();

                    _response.Result = book;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetBook", new { id = book.Id }, _response);

                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpPut("{id:int}", Name = "EditBook")]
        public async Task<ActionResult<ApiResponse>> EditBook(int id, [FromForm] BookUpdateDto bookUpdateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (bookUpdateDto == null || id != bookUpdateDto.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }

                    Book? book = await _context.Books.FindAsync(id);

                    if(book == null)
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        return NotFound(_response);
                    }



                    /*To do: Change to automapper*/


                    book.Title = bookUpdateDto.Title;
                    book.Description = bookUpdateDto.Description;
                    book.Author = bookUpdateDto.Author;
                    book.Price = bookUpdateDto.Price;
                    book.Publisher = bookUpdateDto.Publisher;
                    book.Genre = bookUpdateDto.Genre;

                    if (bookUpdateDto.File != null && bookUpdateDto.File.Length > 0)
                    {
                        await _blobService.DeleteBlob(book.Image.Split('/').Last(), SD.SD_Storage_Container);
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(bookUpdateDto.File.FileName)}";
                        book.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, bookUpdateDto.File);
                    }
                        

                    _context.Books.Update(book);
                    _context.SaveChanges();

                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);

                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteBook")]
        public async Task<ActionResult<ApiResponse>> DeleteBook(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                Book? book = await _context.Books.FindAsync(id);

                if (book == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                await _blobService.DeleteBlob(book.Image.Split('/').Last(), SD.SD_Storage_Container);
                _context.Books.Remove(book);
                _context.SaveChanges();

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }
    }
}
