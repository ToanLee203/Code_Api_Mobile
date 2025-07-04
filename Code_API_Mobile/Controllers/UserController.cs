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
                return Conflict(new { message = "Email đã tồn tại!"});
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

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng!" });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName
            });
        }
        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            // Kiểm tra email có bị trùng với người dùng khác không
            var emailExists = _context.Users
                .Any(u => u.Email == request.NewEmail && u.Id != request.UserId);

            if (emailExists)
            {
                return Conflict(new { message = "Email đã được sử dụng bởi tài khoản khác!" });
            }

            // Kiểm tra mật khẩu cũ có đúng không
            if (user.Password != request.CurrentPassword)
            {
                return BadRequest(new { message = "Mật khẩu hiện tại không đúng!" });
            }

            // Cập nhật
            user.Email = request.NewEmail;
            user.Password = request.NewPassword;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Cập nhật thành công",
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

    public class UpdateUserRequest
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }


}
