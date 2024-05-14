using Microsoft.EntityFrameworkCore;
using TravelApp.EntityFrameworkCore;
using TravelAppAPI.Models;

namespace TravelAppAPI.Services
{
    public class AuthServices
    {
        private readonly TravelAppDbContext _context;

        public AuthServices(TravelAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<LoginInfo> CheckExist(string userName, string password)
        {
            var hashedPassword = CreateMD5(password);
            var user = await _context.Users
                .Where(u => u.Username == userName && u.Password == hashedPassword)
                .Select(u => new LoginInfo { UserId = u.UserId, Username = u.Username, Role = u.Role })
                .FirstOrDefaultAsync();

            return user ?? new LoginInfo();
        }

        public async Task<bool> CheckExistUser(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public string CreateMD5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // Make sure your .NET version supports this method
            }
        }

        public async Task<User> GetAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
