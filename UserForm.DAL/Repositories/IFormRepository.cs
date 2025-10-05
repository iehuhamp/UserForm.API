using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserForm.DAL.Models;

namespace UserForm.DAL.Repositories;

public interface IFormRepository : IGenericRepository<FormRegisterService>
{
    Task<FormRegisterService?> GetDetailAsync(Guid formId, CancellationToken ct = default);
    Task<IReadOnlyList<FormRegisterService>> GetUserFormsAsync(Guid userId, CancellationToken ct = default);
    public  Task<List<FormRegisterService>> GetAllFormsAsync();
}