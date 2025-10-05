using UserForm.DAL.Models;
using UserForm.DAL.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace UserForm.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo)
        {
            _repo = repo;
        }

        // 🔒 Hash password
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        // 📘 CRUD METHODS
        public async Task<IEnumerable<User>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<User?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user != null)
                await _repo.DeleteAsync(user);
        }

        // 👥 AUTH METHODS
        public async Task<User?> GetByEmailAsync(string email) => await _repo.GetByEmailAsync(email);

        public async Task<User> RegisterAsync(User user, string password)
        {
            user.PasswordHash = HashPassword(password);
            user.IsActive = true;

            if (user.CreatedAt == default)
                user.CreatedAt = DateTime.UtcNow;

            user.UpdatedAt = DateTime.UtcNow;

            await _repo.AddAsync(user);
            return user;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);
            if (user == null) return null;

            string hash = HashPassword(password);
            if (user.PasswordHash != hash)
                return null;

            return user;
        }
    }
}
