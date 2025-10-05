using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserForm.DAL.Models;

namespace UserForm.BLL.Services
{
    public interface IUserService
    {
        // CRUD
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(User user);

        // Auth
        Task<User?> GetByEmailAsync(string email);
        Task<User> RegisterAsync(User user, string password);
        Task<User?> LoginAsync(string email, string password);
    }
}