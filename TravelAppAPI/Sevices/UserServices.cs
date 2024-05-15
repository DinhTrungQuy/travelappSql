using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using TravelApp.EntityFrameworkCore;
using TravelAppAPI.Models;

namespace TravelAppAPI.Services
{
    public class UserServices
    {
        private readonly TravelAppDbContext _context;

        public UserServices(TravelAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetAsync(string id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Password = String.Empty;  // Clear password before returning user details
            }
            return user;
        }
        public async Task<User> GetUserAndPassword(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<string> GetUserPassword(string username)
        {
            var user = await _context.Users
                .Where(u => u.Username == username)
                .Select(u => u.Password)
                .FirstOrDefaultAsync();

            return user ?? string.Empty;
        }

        public string DecodeJwtToken(HttpRequest request)
        {
            string authHeader = request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
            {
                authHeader = authHeader.Replace("Bearer ", string.Empty);
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                var userId = tokenS?.Claims.First(claim => claim.Type == "Id")?.Value;
                return userId ?? string.Empty;
            }
            return string.Empty;
        }

        public async Task UpdateAsync(string id, User userIn)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Entry(user).CurrentValues.SetValues(userIn);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(User userIn)
        {
            var user = await _context.Users.FindAsync(userIn.UserId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
