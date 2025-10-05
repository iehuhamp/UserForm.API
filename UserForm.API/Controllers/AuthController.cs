using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserForm.BLL.DTOs;
using UserForm.BLL.Helpers;
using UserForm.BLL.Services;
using UserForm.DAL.Models;

namespace UserForm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenHelper _jwt;

        public AuthController(IUserService userService, JwtTokenHelper jwt)
        {
            _userService = userService;
            _jwt = jwt;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (req.Password != req.ConfirmPassword)
                return BadRequest("Mật khẩu xác nhận không khớp.");

            var existing = await _userService.GetByEmailAsync(req.Email);
            if (existing != null)
                return Conflict("Email đã tồn tại.");

            var user = new User
            {
                StudentId = req.StudentID,
                StudentName = req.StudentName,
                Email = req.Email,
                CampusId = req.CampusId,
                RoleId = 4, // Default role: User
                IsActive = true
            };

            await _userService.RegisterAsync(user, req.Password);
            return Ok("Đăng ký thành công!");
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _userService.LoginAsync(req.Email, req.Password);
            if (user == null)
                return Unauthorized("Email hoặc mật khẩu không đúng.");

            var token = _jwt.GenerateToken(user);

            return Ok(new LoginResponse
            {
                Token = token,
                Email = user.Email,
                StudentName = user.StudentName
            });
        }
    }
}
