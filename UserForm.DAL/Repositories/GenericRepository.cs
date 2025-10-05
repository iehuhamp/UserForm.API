using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserForm.DAL.Models;

namespace UserForm.DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AssignmentSupportDBContext _db;
    protected readonly DbSet<T> _set;

    public GenericRepository(AssignmentSupportDBContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
        => await _set.FindAsync(new object?[] { id }, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _set.AddAsync(entity, ct);

    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _set.AnyAsync(predicate, ct);

    public async Task<IReadOnlyList<T>> ListAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? include = null,
        CancellationToken ct = default)
    {
        IQueryable<T> q = _set.AsNoTracking();
        if (predicate != null) q = q.Where(predicate);
        if (!string.IsNullOrWhiteSpace(include)) q = q.Include(include);
        if (orderBy != null) q = orderBy(q);
        return await q.ToListAsync(ct);
    }
}
