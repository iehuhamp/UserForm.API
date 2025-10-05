using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserForm.BLL.Services;
using UserForm.BLL.DTOs;
using UserForm.DAL.Models;

public interface IInvoiceService
{
    Task<Invoice> GenerateInvoiceAsync(Guid formId);
    Task<IEnumerable<Invoice>> GetUserInvoicesAsync(Guid userId);
    Task<bool> PayInvoiceAsync(Guid invoiceId);
    Task RecordPaymentAsync(Guid invoiceId, PayInvoiceRequest req, CancellationToken ct = default);
}