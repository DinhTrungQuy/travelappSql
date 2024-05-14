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
    public class UserController(UserServices userServices, FileServices fileServices, AuthServices authServices) : ControllerBase
    {
        private readonly UserServices _userServices = userServices;
        private readonly FileServices _fileServices = fileServices;
        private readonly AuthServices _authServices = authServices;
        [HttpGet]
        public async Task<ActionResult<User>> Get()
        {
            var request = HttpContext.Request;
            string userId = _userServices.DecodeJwtToken(request);
            return Ok(await _userServices.GetAsync(userId));

        }
        [HttpGet("{userId:length(24)}")]
        public async Task<ActionResult<User>> Get(string userId)
        {
            return Ok(await _userServices.GetAsync(userId));

        }
        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, UserDto userIn)
        {
            var map = MapperConfig.Initialize();
            User user = await _authServices.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userModel = map.Map<User>(userIn);
            userModel.UserId = id;

            var filePath = await _fileServices.SaveUserFile(userIn.Image!, userModel.UserId);
            if (filePath == "Invalid file")
            {
                return BadRequest("Invalid file");

            }
            else
            {
                userModel.ImageUrl = "https://quydt.speak.vn/images/users/" + userModel.UserId + Path.GetExtension(filePath);
            }
            var password = await _userServices.GetUserPassword(userModel.Username);
            userModel.Password = password;
            await _userServices.UpdateAsync(id, userModel);
            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = _authServices.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userServices.RemoveAsync(id);
            return NoContent();
        }
    }
}
