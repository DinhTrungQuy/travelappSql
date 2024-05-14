using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAppAPI.Models;
using TravelAppAPI.Sevices;

namespace TravelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController(BookingServices bookingServices, UserServices userServices) : ControllerBase
    {
        private readonly BookingServices _bookingServices = bookingServices;
        private readonly UserServices _userServices = userServices;
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> Get()
        {
            return await _bookingServices.GetAsync();
        }
        [HttpGet("{id:length(24)}", Name = "GetBooking")]
        public async Task<ActionResult<Booking>> Get(string id)
        {
            var booking = await _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return booking;
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> Create(Booking booking)
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            booking.UserId = userId;
            Booking createBooking = await _bookingServices.CreateAsync(booking);
            return Ok(createBooking);
        }
        [Route("Checkin/{id:length(24)}")]
        [HttpPost]
        public async Task<ActionResult<Booking>> Checkin(string id)
        {
            Booking booking = await _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            booking.Status = 1;
            booking.CheckInTime = DateTime.Now;
            await _bookingServices.UpdateAsync(id, booking);
            return NoContent();
        }
        [Route("Checkout/{id:length(24)}")]
        [HttpPost]
        public async Task<ActionResult<Booking>> Checkout(string id)
        {
            Booking booking = await _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            booking.Status = 2;
            booking.CheckOutTime = DateTime.Now;
            await _bookingServices.UpdateAsync(id, booking);
            return NoContent();
        }
        [Route("Cancel/{id:length(24)}")]
        [HttpPost]
        public async Task<ActionResult<Booking>> Cancel(string id)
        {
            Booking booking = await _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            booking.Status = 4;
            booking.CheckInTime = DateTime.Now;
            await _bookingServices.UpdateAsync(id, booking);
            return NoContent();
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Booking bookingIn)
        {
            var booking = _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            await _bookingServices.UpdateAsync(id, bookingIn);
            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var booking = await _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            await _bookingServices.RemoveAsync(booking.Id);
            return NoContent();
        }
    }
}
