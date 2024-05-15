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
    public class WishlistController(WishlistServices wishlistServices, UserServices userServices, PlaceServices placeServices) : ControllerBase
    {

        private readonly UserServices _userServices = userServices;
        private readonly WishlistServices _wishlistServices = wishlistServices;
        private readonly PlaceServices _placeServices = placeServices;
        [HttpGet]
        public async Task<ActionResult<WishlistOutputDto>> Get()
        {
            var mapper = MapperConfig.Initialize();
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            var wishlist = await _wishlistServices.GetAsync(userId);
            var wishlistOutput = mapper.Map<List<WishlistOutputDto>>(wishlist);
            return Ok(wishlistOutput);
        }


        
        [HttpGet("{placeId:length(36)}", Name = "GetWishlist")]
        public async Task<ActionResult<WishlistOutputDto>> Get(string placeId)
        {
            var mapper = MapperConfig.Initialize();
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            var wishlist = await _wishlistServices.CheckExist(userId, placeId);
            if (wishlist == null)
            {
                return NotFound();
            }
            var wishlistOutput = mapper.Map<WishlistOutputDto>(wishlist);
            return Ok(wishlistOutput);
        }
        [HttpPost]
        public async Task<ActionResult<Wishlist>> Post(WishlistInputDto wishlistIn)
        {
            var mapper = MapperConfig.Initialize();
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            var wishlist = mapper.Map<Wishlist>(wishlistIn);
            wishlist.User = await _userServices.GetUserAndPassword(userId);
            wishlist.Place = await _placeServices.GetAsync(wishlistIn.PlaceId);
            await _wishlistServices.CreateAsync(wishlist);
            return Ok(wishlist.Id);
        }
       
        [HttpDelete("{placeId:length(36)}")]
        public async Task<IActionResult> Delete(string placeId)
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            await _wishlistServices.RemoveAsync(userId, placeId);
            return NoContent();
        }
    }

}
