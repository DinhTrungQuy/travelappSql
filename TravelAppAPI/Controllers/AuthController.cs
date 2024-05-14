using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelAppAPI.Models;
using TravelAppAPI.Models.Config;
using TravelAppAPI.Models.Dto;
using TravelAppAPI.Services;

namespace TravelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthServices authServices, CacheServices cacheServices) : ControllerBase
    {
        private readonly AuthServices _authServices = authServices;
        private readonly CacheServices _cacheServices = cacheServices;


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Get()
        {
            return Ok(await _authServices.GetAsync());
        }



        [HttpGet("{id:length(24)}", Name = "GetUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _authServices.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Login(LoginDto user, IOptions<JwtSettings> settings)
        {
            var userLogin = await _authServices.CheckExist(user.Username, user.Password);
            var ExpriredTime = DateTime.Now.AddHours(1);
            //var ExpriredTime = DateTime.Now.AddSeconds(10);
            if (!String.IsNullOrEmpty(userLogin.UserId))
            {
                var issuer = settings.Value.Issuer;
                var audience = settings.Value.Audience;
                var key = Encoding.ASCII.GetBytes(settings.Value.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                                new Claim("Id", userLogin.UserId),
                                new Claim("Username", userLogin.Username),
                                new Claim("role", userLogin.Role )
                            }),
                    Expires = ExpriredTime,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);

                CookieOptions option = new()
                {
                    Expires = ExpriredTime,
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                };
                Response.Cookies.Append("Token", stringToken, option);

                _cacheServices.SetData(stringToken, "true", ExpriredTime);
                return Ok(stringToken);
            }
            return Unauthorized();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("adminlogin")]
        public async Task<ActionResult<User>> AdminLogin(LoginDto user, IOptions<JwtSettings> settings)
        {
            var userLogin = await _authServices.CheckExist(user.Username, user.Password);
            var ExpriredTime = DateTime.Now.AddHours(1);
            if (!String.IsNullOrEmpty(userLogin.UserId) && userLogin.Role == "Admin")
            {
                var issuer = settings.Value.Issuer;
                var audience = settings.Value.Audience;
                var key = Encoding.ASCII.GetBytes(settings.Value.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                                new Claim("Id", userLogin.UserId),
                                new Claim("Username", userLogin.Username),
                                new Claim("role", userLogin.Role )
                            }),
                    Expires = ExpriredTime,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);

                CookieOptions option = new()
                {
                    Expires = ExpriredTime,
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                };
                Response.Cookies.Append("Token", stringToken, option);
                _cacheServices.SetData(stringToken, "true", ExpriredTime);
                return Ok(stringToken);
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("logout")]
        public ActionResult Logout()
        {
            var token = Request.Headers.Authorization.ToString();
            token = token.Replace("Bearer ", "");
            _cacheServices.RemoveData(token);
            CookieOptions option = new()
            {
                Expires = DateTime.Now,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("Token", "", option);
            return Ok();
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Register(RegisterDto user)
        {
            var mapper = MapperConfig.Initialize();
            if (await _authServices.CheckExistUser(user.Username))
            {
                return BadRequest("Username is already exist");
            }
            var userModel = mapper.Map<User>(user);
            userModel.Password = _authServices.CreateMD5(userModel.Password);
            await _authServices.CreateAsync(userModel);
            return Ok(userModel);
        }


    }
}