using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAppAPI.Models;
using TravelAppAPI.Models.Config;
using TravelAppAPI.Models.Dto;
using TravelAppAPI.Services;

namespace TravelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController(BookingServices bookingServices, UserServices userServices, PlaceServices placeServices) : ControllerBase
    {
        private readonly BookingServices _bookingServices = bookingServices;
        private readonly UserServices _userServices = userServices;
        private readonly PlaceServices _placeServices = placeServices;

        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<List<BookingOutputDto>>> GetAll()
        {
            var mapper = MapperConfig.Initialize();
            var bookings = await _bookingServices.GetAsync();
            var bookingOutput = mapper.Map<List<BookingOutputDto>>(bookings);
            return bookingOutput;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingOutputDto>>> Get()
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            var mapper = MapperConfig.Initialize();
            var bookings = await _bookingServices.GetByUserIdAsync(userId);
            var bookingOutput = mapper.Map<List<BookingOutputDto>>(bookings);
            return bookingOutput;
        }

        [HttpGet("{id:length(36)}", Name = "GetBooking")]
        public async Task<ActionResult<BookingOutputDto>> Get(string id)
        {
            var mapper = MapperConfig.Initialize();
            var booking = await _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingOutput = mapper.Map<BookingOutputDto>(booking);

            return bookingOutput;
        }


        [HttpPost]
        public async Task<ActionResult<Booking>> Create(BookingInputDto bookingIn)
        {
            var mapper = MapperConfig.Initialize();
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            var booking = mapper.Map<Booking>(bookingIn);
            booking.User = await _userServices.GetAsync(userId);
            booking.Place = await _placeServices.GetAsync(bookingIn.PlaceId);
            Booking createBooking = await _bookingServices.CreateAsync(booking);
            return Ok(createBooking);
        }
        [Route("Checkin/{id:length(36)}")]
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
        [Route("Checkout/{id:length(36)}")]
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
        [Route("Cancel/{id:length(36)}")]
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

        [HttpPut("{id:length(36)}")]
        public async Task<IActionResult> Update(string id, Booking bookingIn)
        {
            var booking = await _bookingServices.GetAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            await _bookingServices.UpdateAsync(id, bookingIn);
            return NoContent();
        }
        [HttpDelete("{id:length(36)}")]
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
