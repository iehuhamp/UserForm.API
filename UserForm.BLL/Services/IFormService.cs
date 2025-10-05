using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserForm.BLL.DTOs;
using UserForm.DAL.Models;

namespace UserForm.BLL.Services;

public interface IFormService
{

    Task<Guid> SubmitAsync(SubmitFormRequest req, CancellationToken ct = default);
    Task ReviewAsync(Guid formId, ReviewFormRequest req, CancellationToken ct = default);
    Task MarkOutcomeAsync(Guid formId, MarkOutcomeRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<FormSummaryDto>> GetUserHistoryAsync(Guid userId, CancellationToken ct = default);


    Task<List<FormRegisterService>> GetAllFormsAsync();
    Task<FormRegisterService?> GetDetailAsync(Guid formId, CancellationToken ct = default);
    Task<bool> DeleteFormAsync(Guid formId);
}
