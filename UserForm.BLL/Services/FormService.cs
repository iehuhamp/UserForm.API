using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserForm.BLL.DTOs;
using UserForm.DAL.Models;
using UserForm.DAL.Repositories;

namespace UserForm.BLL.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _forms;
        private readonly AssignmentSupportDBContext _db;

        public FormService(IFormRepository forms, AssignmentSupportDBContext db)
        {
            _forms = forms;
            _db = db;
        }

        #region SUBMIT FORM
        public async Task<Guid> SubmitAsync(SubmitFormRequest req, CancellationToken ct = default)
        {
            var statusPending = await _db.FormStatuses
                .Where(x => x.StatusCode == "PENDING_APPROVAL")
                .Select(x => x.FormStatusId)
                .SingleAsync(ct);

            var form = new FormRegisterService
            {
                FormId = Guid.NewGuid(),
                UserId = req.UserId,
                CourseCode = req.CourseCode,
                CampusId = req.CampusId,
                ExamDate = req.ExamDate,
                ServiceId = req.ServiceId,
                FormStatusId = statusPending,
                Notes = req.Notes,
                OriginalFormId = req.OriginalFormId,
                SubmittedAt = DateTime.UtcNow
            };

            await _forms.AddAsync(form, ct);

            _db.FormStatusHistories.Add(new FormStatusHistory
            {
                FormId = form.FormId,
                ToStatusId = statusPending,
                ChangedByUserId = req.UserId,
                ChangedAt = DateTime.UtcNow,
                ChangeNote = "Nộp đơn"
            });

            // Notification: FORM_RECEIVED
            var notifType = await _db.NotificationTypes
                .Where(x => x.TypeCode == "FORM_RECEIVED")
                .Select(x => x.NotificationTypeId)
                .SingleAsync(ct);

            _db.Notifications.Add(new Notification
            {
                NotificationId = Guid.NewGuid(),
                NotificationTypeId = notifType,
                ToUserId = req.UserId,
                FormId = form.FormId,
                Subject = "Đã nhận đơn",
                Body = "Đơn đăng ký của bạn đã được ghi nhận.",
                SentAt = DateTime.UtcNow,
                DeliveryChannel = "IN_APP"
            });

            await _db.SaveChangesAsync(ct);
            return form.FormId;
        }
        #endregion

        #region REVIEW FORM
        public async Task ReviewAsync(Guid formId, ReviewFormRequest req, CancellationToken ct = default)
        {
            var form = await _db.FormRegisterServices.FirstAsync(f => f.FormId == formId, ct);
            var statusApproved = await _db.FormStatuses
                .Where(x => x.StatusCode == "APPROVED")
                .Select(x => x.FormStatusId)
                .SingleAsync(ct);
            var statusRejected = await _db.FormStatuses
                .Where(x => x.StatusCode == "REJECTED")
                .Select(x => x.FormStatusId)
                .SingleAsync(ct);

            var fromStatus = form.FormStatusId;

            if (req.Approve)
            {
                form.FormStatusId = statusApproved;
                form.ApprovedAt = DateTime.UtcNow;
                form.ApprovedByUserId = req.AdminUserId;
                form.AdminNotes = req.AdminNote;

                _db.FormStatusHistories.Add(new FormStatusHistory
                {
                    FormId = form.FormId,
                    FromStatusId = fromStatus,
                    ToStatusId = statusApproved,
                    ChangedByUserId = req.AdminUserId,
                    ChangedAt = DateTime.UtcNow,
                    ChangeNote = "Duyệt đơn"
                });

                var notifType = await _db.NotificationTypes
                    .Where(x => x.TypeCode == "FORM_APPROVED")
                    .Select(x => x.NotificationTypeId)
                    .SingleAsync(ct);

                _db.Notifications.Add(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationTypeId = notifType,
                    ToUserId = form.UserId,
                    FormId = form.FormId,
                    Subject = "Đơn được duyệt",
                    Body = "Đơn của bạn đã được duyệt. Hỗ trợ sẽ được tiến hành.",
                    SentAt = DateTime.UtcNow,
                    DeliveryChannel = "IN_APP"
                });
            }
            else
            {
                form.FormStatusId = statusRejected;
                form.RejectedAt = DateTime.UtcNow;
                form.RejectedByUserId = req.AdminUserId;
                form.AdminNotes = req.AdminNote;

                _db.FormStatusHistories.Add(new FormStatusHistory
                {
                    FormId = form.FormId,
                    FromStatusId = fromStatus,
                    ToStatusId = statusRejected,
                    ChangedByUserId = req.AdminUserId,
                    ChangedAt = DateTime.UtcNow,
                    ChangeNote = req.AdminNote ?? "Từ chối đơn"
                });

                var notifType = await _db.NotificationTypes
                    .Where(x => x.TypeCode == "FORM_REJECTED")
                    .Select(x => x.NotificationTypeId)
                    .SingleAsync(ct);

                _db.Notifications.Add(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationTypeId = notifType,
                    ToUserId = form.UserId,
                    FormId = form.FormId,
                    Subject = "Đơn bị từ chối",
                    Body = "Đơn của bạn đã bị từ chối. Vui lòng kiểm tra ghi chú.",
                    SentAt = DateTime.UtcNow,
                    DeliveryChannel = "IN_APP"
                });
            }

            await _db.SaveChangesAsync(ct);
        }
        #endregion

        #region MARK OUTCOME
        public async Task MarkOutcomeAsync(Guid formId, MarkOutcomeRequest req, CancellationToken ct = default)
        {
            // Thêm session hỗ trợ
            _db.SupportSessions.Add(new SupportSession
            {
                SupportSessionId = Guid.NewGuid(),
                FormId = formId,
                AdminUserId = req.AdminUserId,
                StartedAt = DateTime.UtcNow,
                EndedAt = DateTime.UtcNow,
                OutcomeCode = req.Outcome,
                OutcomeNote = req.OutcomeNote
            });

            var form = await _db.FormRegisterServices.FirstAsync(f => f.FormId == formId, ct);

            var statusInProgress = await _db.FormStatuses
                .Where(s => s.StatusCode == "IN_PROGRESS")
                .Select(s => s.FormStatusId)
                .SingleAsync(ct);
            var statusFailed = await _db.FormStatuses
                .Where(s => s.StatusCode == "FAILED")
                .Select(s => s.FormStatusId)
                .SingleAsync(ct);
            var statusCompleted = await _db.FormStatuses
                .Where(s => s.StatusCode == "COMPLETED")
                .Select(s => s.FormStatusId)
                .SingleAsync(ct);
            var statusAwaitingPayment = await _db.FormStatuses
                .Where(s => s.StatusCode == "AWAITING_PAYMENT")
                .Select(s => s.FormStatusId)
                .SingleAsync(ct);

            if (req.Outcome.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase))
            {
                // Mark completed
                form.FormStatusId = statusCompleted;
                _db.FormStatusHistories.Add(new FormStatusHistory
                {
                    FormId = form.FormId,
                    FromStatusId = statusInProgress,
                    ToStatusId = statusCompleted,
                    ChangedByUserId = req.AdminUserId,
                    ChangedAt = DateTime.UtcNow,
                    ChangeNote = "Hỗ trợ hoàn tất"
                });

                // Sau đó chuyển sang chờ thanh toán
                form.FormStatusId = statusAwaitingPayment;
                _db.FormStatusHistories.Add(new FormStatusHistory
                {
                    FormId = form.FormId,
                    FromStatusId = statusCompleted,
                    ToStatusId = statusAwaitingPayment,
                    ChangedByUserId = req.AdminUserId,
                    ChangedAt = DateTime.UtcNow,
                    ChangeNote = "Chờ thanh toán"
                });

                // Tạo hóa đơn
                var price = await _db.Services
                    .Where(s => s.ServiceId == form.ServiceId)
                    .Select(s => s.ServicePrice)
                    .SingleAsync(ct);

                var unpaidStatus = await _db.PaymentStatuses
                    .Where(p => p.StatusCode == "UNPAID")
                    .Select(p => p.PaymentStatusId)
                    .SingleAsync(ct);

                var invoice = new Invoice
                {
                    InvoiceId = Guid.NewGuid(),
                    FormId = form.FormId,
                    UserId = form.UserId,
                    PaymentStatusId = unpaidStatus,
                    Subtotal = price,
                    DiscountAmount = 0,
                    Currency = "VND",
                    CreatedAt = DateTime.UtcNow
                };

                _db.Invoices.Add(invoice);
                _db.InvoiceItems.Add(new InvoiceItem
                {
                    InvoiceId = invoice.InvoiceId,
                    ServiceId = form.ServiceId,
                    Description = "Phí dịch vụ hỗ trợ",
                    Qty = 1,
                    UnitPrice = price
                });

                // Notification
                var notifType = await _db.NotificationTypes
                    .Where(x => x.TypeCode == "PAYMENT_REQUEST")
                    .Select(x => x.NotificationTypeId)
                    .SingleAsync(ct);

                _db.Notifications.Add(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationTypeId = notifType,
                    ToUserId = form.UserId,
                    FormId = form.FormId,
                    Subject = "Yêu cầu thanh toán",
                    Body = $"Dịch vụ đã hoàn tất. Vui lòng thanh toán {price:N0} VND.",
                    SentAt = DateTime.UtcNow,
                    DeliveryChannel = "IN_APP"
                });
            }
            else
            {
                // Nếu thất bại, tạo form mới cho user điền lại
                form.FormStatusId = statusFailed;
                _db.FormStatusHistories.Add(new FormStatusHistory
                {
                    FormId = form.FormId,
                    FromStatusId = statusInProgress,
                    ToStatusId = statusFailed,
                    ChangedByUserId = req.AdminUserId,
                    ChangedAt = DateTime.UtcNow,
                    ChangeNote = req.OutcomeNote ?? "Hỗ trợ thất bại"
                });

                var statusPending = await _db.FormStatuses
                    .Where(s => s.StatusCode == "PENDING_APPROVAL")
                    .Select(s => s.FormStatusId)
                    .SingleAsync(ct);

                var newForm = new FormRegisterService
                {
                    FormId = Guid.NewGuid(),
                    UserId = form.UserId,
                    CourseCode = form.CourseCode,
                    CampusId = form.CampusId,
                    ServiceId = form.ServiceId,
                    ExamDate = null,
                    FormStatusId = statusPending,
                    Notes = "Form tái nộp sau thất bại",
                    OriginalFormId = form.FormId,
                    SubmittedAt = DateTime.UtcNow
                };

                await _db.FormRegisterServices.AddAsync(newForm, ct);

                var notifType = await _db.NotificationTypes
                    .Where(x => x.TypeCode == "SUPPORT_RESULT")
                    .Select(x => x.NotificationTypeId)
                    .SingleAsync(ct);

                _db.Notifications.Add(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    NotificationTypeId = notifType,
                    ToUserId = form.UserId,
                    FormId = form.FormId,
                    Subject = "Kết quả hỗ trợ",
                    Body = "Hỗ trợ thất bại. Hệ thống đã tạo form mới để bạn điền lại.",
                    SentAt = DateTime.UtcNow,
                    DeliveryChannel = "IN_APP"
                });
            }

            await _db.SaveChangesAsync(ct);
        }
        #endregion

        #region GET ALL / DETAIL / HISTORY
        public async Task<List<FormRegisterService>> GetAllFormsAsync()
        {
            return await _forms.GetAllFormsAsync();
        }

        public async Task<FormRegisterService?> GetDetailAsync(Guid formId, CancellationToken ct = default)
        {
            return await _forms.GetDetailAsync(formId, ct);
        }

        public async Task<IReadOnlyList<FormSummaryDto>> GetUserHistoryAsync(Guid userId, CancellationToken ct = default)
        {
            return await _db.FormRegisterServices
                .Include(f => f.Service)
                .Include(f => f.FormStatus)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.SubmittedAt)
                .Select(f => new FormSummaryDto(
                    f.FormId,
                    f.Service.ServiceCode,
                    f.FormStatus.StatusCode,
                    f.SubmittedAt,
                    f.ApprovedAt,
                    f.OriginalFormId))
                .ToListAsync(ct);
        }
        #endregion

        #region DELETE FORM
        public async Task<bool> DeleteFormAsync(Guid formId)
        {
            var form = await _db.FormRegisterServices.FindAsync(formId);
            if (form == null)
                return false;

            _db.FormRegisterServices.Remove(form);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<FormRegisterService?> GetFormByIdAsync(Guid formId)
        {
            return await _db.FormRegisterServices.FindAsync(formId);
        }
        #endregion
    }
}
