using Microsoft.EntityFrameworkCore;
using UserForm.BLL.DTOs;
using UserForm.DAL.Models;

namespace UserForm.BLL.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AssignmentSupportDBContext _db;
        private readonly INotificationService _noti;

        public InvoiceService(AssignmentSupportDBContext db, INotificationService noti)
        {
            _db = db;
            _noti = noti;
        }

        public async Task<Invoice> GenerateInvoiceAsync(Guid formId)
        {
            var form = await _db.FormRegisterServices
                .Include(f => f.Service)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.FormId == formId);

            if (form == null)
                throw new Exception("Form not found.");

            // ✅ Lấy PaymentStatusId tương ứng
            var pendingStatusId = await _db.PaymentStatuses
                .Where(p => p.StatusCode == "PENDING")
                .Select(p => p.PaymentStatusId)
                .FirstOrDefaultAsync();

            var invoice = new Invoice
            {
                InvoiceId = Guid.NewGuid(),
                FormId = formId,
                UserId = form.UserId,
                PaymentStatusId = pendingStatusId,
                Subtotal = form.Service.ServicePrice,
                DiscountAmount = 0,
                TotalAmount = form.Service.ServicePrice,
                Currency = "VND",
                CreatedAt = DateTime.UtcNow
            };

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();

            await _noti.SendAsync(form.UserId, "Hóa đơn mới",
                $"Hóa đơn cho dịch vụ {form.Service.ServiceName} - Tổng tiền: {form.Service.ServicePrice:#,##0}đ",
                form.FormId);

            return invoice;
        }

        public async Task<IEnumerable<Invoice>> GetUserInvoicesAsync(Guid userId)
        {
            return await _db.Invoices
                .Include(i => i.Form)
                .ThenInclude(f => f.Service)
                .Include(i => i.PaymentStatus)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> PayInvoiceAsync(Guid invoiceId)
        {
            var inv = await _db.Invoices
                .Include(i => i.Form)
                .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

            if (inv == null) return false;

            var paidStatusId = await _db.PaymentStatuses
                .Where(p => p.StatusCode == "PAID")
                .Select(p => p.PaymentStatusId)
                .FirstAsync();

            inv.PaymentStatusId = paidStatusId;
            inv.PaidAt = DateTime.UtcNow;

            _db.Invoices.Update(inv);
            await _db.SaveChangesAsync();

            await _noti.SendAsync(inv.Form.UserId,
                "Thanh toán thành công",
                $"Bạn đã thanh toán thành công hóa đơn #{inv.InvoiceId}.",
                inv.Form.FormId);

            return true;
        }

        public async Task RecordPaymentAsync(Guid invoiceId, PayInvoiceRequest req, CancellationToken ct = default)
        {
            var inv = await _db.Invoices
                .Include(i => i.Form)
                .ThenInclude(f => f.User)
                .FirstAsync(i => i.InvoiceId == invoiceId, ct);

            var paidId = await _db.PaymentStatuses
                .Where(p => p.StatusCode == "PAID")
                .Select(p => p.PaymentStatusId)
                .SingleAsync(ct);

            inv.PaymentStatusId = paidId;
            inv.PaymentRef = req.PaymentRef;
            inv.PaidAt = DateTime.UtcNow;

            var form = inv.Form;

            var awaiting = await _db.FormStatuses
                .Where(s => s.StatusCode == "AWAITING_PAYMENT")
                .Select(s => s.FormStatusId)
                .SingleAsync(ct);

            var paidForm = await _db.FormStatuses
                .Where(s => s.StatusCode == "PAID")
                .Select(s => s.FormStatusId)
                .SingleAsync(ct);

            _db.FormStatusHistories.Add(new FormStatusHistory
            {
                FormId = form.FormId,
                FromStatusId = awaiting,
                ToStatusId = paidForm,
                ChangedByUserId = form.UserId,
                ChangedAt = DateTime.UtcNow,
                ChangeNote = "Đã thanh toán"
            });

            form.FormStatusId = paidForm;

            await _noti.SendAsync(form.UserId,
                "Xác nhận thanh toán",
                "Cảm ơn bạn! Thanh toán đã được xác nhận.",
                form.FormId);

            await _db.SaveChangesAsync(ct);
        }
    }
}
