using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAppAPI.Models;
using TravelAppAPI.Services;

namespace TravelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController(WishlistServices wishlistServices, UserServices userServices) : ControllerBase
    {

        private readonly UserServices _userServices = userServices;
        private readonly WishlistServices _wishlistServices = wishlistServices;
        [HttpGet]
        public async Task<ActionResult<Wishlist>> Get()
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            return Ok(await _wishlistServices.GetAsync(userId));
        }
        [HttpGet("{placeId:length(36)}", Name = "GetWishlist")]
        public async Task<ActionResult<Wishlist>> Get(string placeId)
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            var wishlist = await _wishlistServices.CheckExist(userId, placeId);
            if (wishlist == null)
            {
                return NotFound();
            }
            return Ok(wishlist);
        }
        [HttpPost]
        public async Task<ActionResult<Wishlist>> Post(Wishlist wishlist)
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            wishlist.User.UserId = userId;
            await _wishlistServices.CreateAsync(wishlist);
            return CreatedAtRoute("GetWishlist", new { id = wishlist.Id.ToString() }, wishlist);
        }
        [HttpPut("{id:length(36)}")]
        public async Task<IActionResult> Update(string id, Wishlist wishlistIn)
        {
            var wishlist = _wishlistServices.GetAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }
            await _wishlistServices.UpdateAsync(id, wishlistIn);
            return NoContent();
        }
        [HttpDelete("{placeId:length(36)}")]
        public async Task<IActionResult> Delete(string placeId)
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            //var wishlist = 
            //if (wishlist == null)
            //{
            //    return NotFound();
            //}
            await _wishlistServices.RemoveAsync(userId, placeId);
            return NoContent();
        }
    }

}
