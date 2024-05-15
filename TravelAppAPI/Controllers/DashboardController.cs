using Microsoft.AspNetCore.Mvc;
using TravelAppAPI.Models;
using TravelAppAPI.Services;
using System.Threading.Tasks;

namespace TravelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardServices _dashboardServices;

        public DashboardController(DashboardServices dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }

        [HttpGet]
        public async Task<ActionResult<Dashboard>> Get()
        {
            var profit = _dashboardServices.GetProfit();
            var place = await _dashboardServices.GetTotalPlaces();
            var user = await _dashboardServices.GetTotalUsers();
            var booking = await _dashboardServices.GetTotalBookings();
            var dashboardId = await _dashboardServices.GetDashboard();
            var updatedDashboard = await _dashboardServices.UpdateDashboard(new Dashboard
            {
                Id = dashboardId,
                Profit = profit,
                TotalPlaces = place,
                TotalUsers = user,
                TotalBookings = booking
            });
            return Ok(updatedDashboard);
        }
    }
}
