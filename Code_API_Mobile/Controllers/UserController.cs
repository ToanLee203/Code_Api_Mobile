using Code_API_Mobile.ModelDTO;
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

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email đã tồn tại!" });
            }

            // Tạo người dùng mới
            var newUser = new User
            {
                Email = request.Email,
                Password = request.Password, // Lưu ý: nên mã hóa mật khẩu nếu dùng thật
                FullName = request.FullName
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new
            {
                newUser.Id,
                newUser.Email,
                newUser.FullName
            });
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
