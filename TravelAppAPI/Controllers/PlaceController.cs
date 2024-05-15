using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAppAPI.Model;
using TravelAppAPI.Models.Config;
using TravelAppAPI.Models.Dto;
using TravelAppAPI.Services;

namespace TravelAppAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PlaceController(PlaceServices placeServices, FileServices fileServices, BookingServices bookingServices, WishlistServices wishlistServices) : ControllerBase
    {
        private readonly PlaceServices _placeServices = placeServices;
        private readonly FileServices _fileServices = fileServices;
        private readonly BookingServices _bookingServices = bookingServices;
        private readonly WishlistServices _wishlistServices = wishlistServices;
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Place>> Get()
        {

            return Ok(await _placeServices.GetAsync());
        }
        [HttpGet("{id:length(36)}", Name = "GetPlace")]
        [AllowAnonymous]
        public async Task<ActionResult<Place>> Get(string id)
        {
            var place = await _placeServices.GetAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            return Ok(place);
        }
        [HttpPost]
        public async Task<ActionResult<Place>> Post([FromForm] PlaceDto place)
        {
            var mapper = MapperConfig.Initialize();
            var placeModel = mapper.Map<Place>(place);
            await _placeServices.CreateAsync(placeModel);
            var filePath = await _fileServices.SavePlaceFile(place.Image!, placeModel.PlaceId);
            placeModel.ImageUrl = "https://quydt.speak.vn/images/places/" + placeModel.PlaceId + Path.GetExtension(filePath);
            placeModel.Rating = "0";
            await _placeServices.UpdateAsync(placeModel.PlaceId, placeModel);
            return Ok(placeModel);
        }
        [HttpPut("{id:length(36)}")]
        public async Task<IActionResult> Update(string id, [FromForm] PlaceDto placeIn)
        {
            var mapper = MapperConfig.Initialize();
            var place = await _placeServices.GetAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            var placeModel = mapper.Map<Place>(placeIn);
            placeModel.PlaceId = id;
            placeModel.Rating = place.Rating;
            var filePath = await _fileServices.SavePlaceFile(placeIn.Image!, placeModel.PlaceId);
            if (filePath == "Invalid file")
            {
                return BadRequest("Invalid file");

            }
            else
            {
                placeModel.ImageUrl = "https://quydt.speak.vn/images/places/" + placeModel.PlaceId + Path.GetExtension(filePath);
            }

            await _placeServices.UpdateAsync(id, placeModel);
            return NoContent();
        }
        [HttpDelete("{id:length(36)}")]
        public async Task<IActionResult> Delete(string id)
        {

            Place place = await _placeServices.GetAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            _fileServices.DeletePlaceFile(place.ImageUrl);
            await _placeServices.RemoveAsync(id);
            await _bookingServices.RemoveByPlaceIdAsync(id);
            await _wishlistServices.RemoveByPlaceIdAsync(id);
            return NoContent();
        }
    }
}
