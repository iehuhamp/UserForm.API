using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForm.BLL.DTOs;

public record SubmitFormRequest(
    Guid UserId,
    string CourseCode,
    Guid CampusId,
    DateTime? ExamDate,
    Guid ServiceId,
    string? Notes,
    Guid? OriginalFormId);

public record ReviewFormRequest(bool Approve, Guid AdminUserId, string? AdminNote);
public record MarkOutcomeRequest(string Outcome, Guid AdminUserId, string? OutcomeNote);
public record FormSummaryDto(Guid FormId, string ServiceCode, string StatusCode,
                             DateTime SubmittedAt, DateTime? ApprovedAt, Guid? OriginalFormId);

public record PayInvoiceRequest(string PaymentRef);
