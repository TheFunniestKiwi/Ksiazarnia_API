using System.IdentityModel.Tokens.Jwt;
using Ksiazarnia_API.Data;
using Ksiazarnia_API.Models;
using Ksiazarnia_API.Models.DTO;
using Ksiazarnia_API.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ksiazarnia_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _jwtKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApiResponse _response;

        public AuthController(ApplicationDbContext context, IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _response = new ApiResponse();
            _jwtKey = config.GetValue<string>("ApiSettings:Secret")!;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto == null || !ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            AppUser? userFromDb = _context.Users
                .FirstOrDefault(u => u.NormalizedEmail == registerRequestDto.Email.ToUpper());

            if (userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User with this email already exists");
                return BadRequest(_response);
            }

            AppUser user = new()
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                Name = registerRequestDto.Name,
                LastName = registerRequestDto.LastName
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }

                    if (registerRequestDto.Role.ToLower() == SD.Role_Admin)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }

                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch (Exception) { }

            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Error while registering");
            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null || !ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            AppUser? userFromDb = _context.Users
                .FirstOrDefault(u => u.NormalizedUserName == loginRequestDto.Username.ToUpper());
            if (userFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No user with that username");
                return NotFound(_response);
            }

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDto.Password);

            if (!isValid)
            {
                _response.Result = new LoginResponseDto();
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is not valid");
                return BadRequest(_response);
            }

            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_jwtKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new("fullName", userFromDb.Name + " " + userFromDb.LastName),
                    new("id", userFromDb.Id),
                    new(ClaimTypes.Email, userFromDb.Email!),
                    new(ClaimTypes.Role, roles.FirstOrDefault()!)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponse = new()
            {
                Email = userFromDb.Email!,
                Token = tokenHandler.WriteToken(token)
            };

            if (string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is not valid");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);

        }
    }
}
