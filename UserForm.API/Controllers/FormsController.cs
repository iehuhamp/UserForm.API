using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserForm.BLL.DTOs;
using UserForm.BLL.Services;

namespace UserForm.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService;
        }


        [HttpPost]
        [Authorize(Roles = "4")] 
        public async Task<ActionResult<object>> Submit([FromBody] SubmitFormRequest req, CancellationToken ct)
        {
            var id = await _formService.SubmitAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { formId = id });
        }


        [HttpGet("{id:guid}")]
        [Authorize(Roles = "3,4")] 
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var form = await _formService.GetDetailAsync(id, ct);
            if (form == null)
                return NotFound(new { message = "Không tìm thấy form với ID này." });

            return Ok(form);
        }


        [HttpPost("{id:guid}/review")]
        [Authorize(Roles = "3")] 
        public async Task<IActionResult> Review(Guid id, [FromBody] ReviewFormRequest req, CancellationToken ct)
        {
            await _formService.ReviewAsync(id, req, ct);
            return Ok(new { message = "Đã duyệt / từ chối form thành công." });
        }


        [HttpPost("{id:guid}/outcome")]
        [Authorize(Roles = "3")] 
        public async Task<IActionResult> Outcome(Guid id, [FromBody] MarkOutcomeRequest req, CancellationToken ct)
        {
            await _formService.MarkOutcomeAsync(id, req, ct);
            return Ok(new { message = "Đã cập nhật trạng thái hỗ trợ / tạo hóa đơn / form mới." });
        }

        [HttpGet("history/{userId:guid}")]
        [Authorize(Roles = "3,4")]
        public async Task<IActionResult> History(Guid userId, CancellationToken ct)
        {
            var list = await _formService.GetUserHistoryAsync(userId, ct);
            if (list == null || !list.Any())
                return NoContent();

            return Ok(list);
        }


        [HttpGet]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> GetAllForms(CancellationToken ct)
        {
            var forms = await _formService.GetAllFormsAsync();
            if (forms == null || forms.Count == 0)
                return NoContent();

            return Ok(forms);
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _formService.DeleteFormAsync(id);
            if (!success)
                return NotFound(new { message = "Không tìm thấy form cần xóa." });

            return Ok(new { message = "Đã xóa form thành công." });
        }
    }
}
