using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserForm.BLL.DTOs;
using UserForm.BLL.Services;

namespace UserForm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "4")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(IInvoiceService invoiceService, ILogger<InvoicesController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
        }

        [HttpPost("{id:guid}/pay")]
        public async Task<IActionResult> Pay(Guid id, [FromBody] PayInvoiceRequest req, CancellationToken ct)
        {
            if (req == null)
                return BadRequest("Dữ liệu thanh toán không hợp lệ.");

            await _invoiceService.RecordPaymentAsync(id, req, ct);
            _logger.LogInformation("Invoice {InvoiceId} đã được thanh toán.", id);

            return Ok(new { Message = "Thanh toán thành công!", InvoiceId = id });
        }
    }
}
