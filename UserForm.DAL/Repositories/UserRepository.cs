using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserForm.DAL.Models;

namespace UserForm.DAL.Repositories
{
    public class UserRepository
    {
        private readonly AssignmentSupportDBContext _context;

        public UserRepository(AssignmentSupportDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync() =>
            await _context.Users.Include(u => u.Role).Include(u => u.Campus).ToListAsync();

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.Include(u => u.Role).Include(u => u.Campus)
                .FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
