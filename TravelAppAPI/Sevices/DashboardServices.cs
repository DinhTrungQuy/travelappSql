using Microsoft.EntityFrameworkCore;
using TravelApp.EntityFrameworkCore;
using TravelAppAPI.Models;

namespace TravelAppAPI.Services
{
    public class DashboardServices
    {
        private readonly TravelAppDbContext _context;

        public DashboardServices(TravelAppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetDashboard()
        {
            var dashboard = await _context.Dashboards
                                          .FirstOrDefaultAsync(d => d.Date == DateTime.Today);
            if (dashboard == null)
            {
                dashboard = new Dashboard();
                await _context.Dashboards.AddAsync(dashboard);
                await _context.SaveChangesAsync();
            }
            return dashboard.Id;
        }

        public async Task CreateAsync(Dashboard dashboard)
        {
            await _context.Dashboards.AddAsync(dashboard);
            await _context.SaveChangesAsync();
        }

        public async Task<Dashboard> UpdateDashboard(Dashboard dashboard)
        {
            var existingDashboard = await _context.Dashboards.FindAsync(dashboard.Id);
            if (existingDashboard != null)
            {
                existingDashboard.Profit = dashboard.Profit;
                existingDashboard.TotalUsers = dashboard.TotalUsers;
                existingDashboard.TotalPlaces = dashboard.TotalPlaces;
                existingDashboard.TotalBookings = dashboard.TotalBookings;
                _context.Dashboards.Update(existingDashboard);
                await _context.SaveChangesAsync();
            }
            return dashboard;
        }

        public async Task<int> GetTotalUsers()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalPlaces()
        {
            return await _context.Places.CountAsync();
        }

        public async Task<int> GetTotalBookings()
        {
            return await _context.Bookings.CountAsync();
        }

        public int GetProfit()
        {
            var profit = _context.Bookings
                                 .Where(b => b.Status == 2 || b.Status == 3)
                                 .Sum(b => b.TotalPrice);
            return profit;
        }
    }
}
