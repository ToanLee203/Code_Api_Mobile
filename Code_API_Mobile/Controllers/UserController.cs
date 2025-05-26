using Code_API_Mobile.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Code_API_Mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MobileDbContext _context;

        public UserController(MobileDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
                return Unauthorized(new { message = "Sai email hoặc mật khẩu!" });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
