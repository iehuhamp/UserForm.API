using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserForm.DAL.Models;

namespace UserForm.DAL.Repositories;

public class FormRepository : GenericRepository<FormRegisterService>, IFormRepository
{
    public FormRepository(AssignmentSupportDBContext db) : base(db) { }

    public async Task<FormRegisterService?> GetDetailAsync(Guid formId, CancellationToken ct = default)
        => await _db.FormRegisterServices
                    .Include(f => f.User)
                    .Include(f => f.Campus)
                    .Include(f => f.Service)
                    .Include(f => f.FormStatus)
                    .FirstOrDefaultAsync(f => f.FormId == formId, ct);

    public async Task<IReadOnlyList<FormRegisterService>> GetUserFormsAsync(Guid userId, CancellationToken ct = default)
        => await _db.FormRegisterServices
                    .Include(f => f.FormStatus)
                    .Where(f => f.UserId == userId)
                    .OrderByDescending(f => f.SubmittedAt)
                    .ToListAsync(ct);

    public async Task<List<FormRegisterService>> GetAllFormsAsync()
    {
        return await _db.FormRegisterServices
            //.Include(f => f.User)
            //.Include(f => f.Service)
            //.Include(f => f.Campus)
            //.Include(f => f.FormStatus)
            .OrderByDescending(f => f.SubmittedAt)
            .ToListAsync();
    }
}
