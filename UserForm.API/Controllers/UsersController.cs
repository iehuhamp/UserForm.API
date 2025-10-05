using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserForm.BLL.Services;

namespace UserForm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }


        [HttpGet]
        [Authorize(Roles = "3")] 
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            if (users == null || !users.Any())
                return NoContent();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound("Không tìm thấy user.");
            return Ok(user);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound("Không tìm thấy user.");

            await _service.DeleteAsync(id);
            return Ok($"Đã xóa user {user.Email ?? user.StudentName}.");
        }
    }
}
