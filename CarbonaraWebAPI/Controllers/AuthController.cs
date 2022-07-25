using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Services;
using CarbonaraWebAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CarbonaraWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _dbContext;

        public AuthController(AuthService authService, AppDbContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        /*[HttpGet]
        public async Task<PersonDTO> GetCurrentPersonAsync()
        {
            return new PersonDTO(await _dbContext.Person.SingleAsync(p => p.Id == User.GetUserID()));
        }*/

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var logindata = await _authService.Login(request.Email, request.Password);
            
            return logindata is null ? Unauthorized() : Ok(logindata);
        }

        /*[AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var logindata = await _authService.Register(request.Email, request.Password);

            return logindata is null ? Unauthorized() : Ok(logindata);
        }*/
        [Authorize]
        [HttpPut("change")]
        public async Task<ActionResult> ChangePassword([FromBody] string[] passwords)
        {
            if (passwords.Length != 2)
                return BadRequest();
            if (string.IsNullOrEmpty(passwords[0]) || string.IsNullOrEmpty(passwords[1]))
                return BadRequest();
            if (!_authService.ValidatePasswordHash(passwords[0], await _authService._userService.FindByIDAsync(User.GetUserID())))
                return Unauthorized();
            var authData = AuthService.CreatePasswordHash(passwords[1]);
            await _authService._userService.ChangeAuthDataAsync(authData, User.GetUserID());
            return Ok();
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("impersonate/{id}")]
        public async Task<ActionResult> ImpersonateAsync(int id)
        {
            var claims = await _authService.BuildClaimsAsync(id);
            if (claims is null)
                return BadRequest();
            var token = _authService.CreateJWTtoken(claims);
            if (token is null)
                return BadRequest();
            var iUser = await _authService._userService.GetUserById(id);
            if (iUser is null)
                return Forbid();
            var logindata = new LoginDTO(token, true, new PersonDTO(iUser.Person));
            return logindata is null ? BadRequest() : Ok(logindata);
        }

        /*[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GenTokenAsync()
        {
            return Ok(_authService.CreateJWTtoken(await _authService.BuildClaimsAsync(1)));
        }*/
    }

    public class LoginRequest
    {
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
