using CarbonaraWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.EntityFrameworkCore;
using CarbonaraWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using CarbonaraWebAPI.Util;
using CarbonaraWebAPI.Model.DTO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static CarbonaraWebAPI.Model.DAO.User;
//using CarbonaraWebAPI.Model.DTO;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// Ganzes Objekt uebergeben oder jede methode fuer jede einzelnes Attribut von User
namespace CarbonaraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ExternalPartnerService ExternalPartnerService;
        private AppDbContext _context;
        private readonly AuthService _authService;

        public UserController(ExternalPartnerService externalPartnerServive, AppDbContext context, AuthService authService)
        {
            ExternalPartnerService = externalPartnerServive;
            _context = context;
            _authService = authService;
        }


        [Authorize]
        [HttpGet("current/")]
        public ActionResult<UserDTO> GetCurrentUser()
        { 
            User? user = _context.User.Where(u => u.Id == User.GetUserID()).Include(u => u.Address).Include(u => u.Person).Include(u => u.Plan).SingleOrDefault();
            if (user == null)
                return BadRequest();

            UserDTO userDTO = new UserDTO(user);
            return Ok(userDTO);

        }

      
      
        //[Authorize]
        //[HttpPut("lock/")]
        //public ActionResult LockUser()
        //{
        //    User? lockedUser;

        //    lockedUser = _context.User.Where(u => u.Id == User.GetUserID()).Include(u => u.Person).Include(u => u.Address).SingleOrDefault();

        //    if (lockedUser == null)
        //        return BadRequest();

        //    lockedUser.UserState_ = UserState.Locked;

        //    _context.SaveChanges();
        //    return Ok();
        //}

        [Authorize]
        [HttpPut("update")]
        public ActionResult UpdateUser([FromBody]UserDTO dto)
        {
            User? oldUser;

            oldUser = _context.User.Where(u => u.Id == User.GetUserID()).Include(u => u.Person).Include(u => u.Address).SingleOrDefault();

            if (oldUser == null)
                return BadRequest();

            //_context.User.Update(oldUser);
            Plan? plan = _context.Plan.Where(p => p.Id == dto.Plan.Id).SingleOrDefault();
            if (plan == null)
                return BadRequest();

            oldUser.Plan = plan;
            oldUser.Person.Title = dto.Person.Title;
            oldUser.PaymentMethod_ = dto.Payment;
            oldUser.DriverlicenseNumber = dto.DriverlicenseNumber;

            oldUser.Person.Email = dto.Person.Email;
            oldUser.Person.FormOfAddress = dto.Person.FormOfAddress;
            oldUser.Person.Title = dto.Person.Title;
            oldUser.Person.Lastname = dto.Person.Lastname;
            oldUser.Person.Firstname = dto.Person.Firstname;

            oldUser.Address.ZIP = dto.Address.ZIP;
            oldUser.Address.Street = dto.Address.Street;
            oldUser.Address.Country = dto.Address.Country;
            oldUser.Address.City = dto.Address.City;
            oldUser.Address.Number = dto.Address.Number;

            _context.SaveChanges();
            return Ok();



            /*
            if (User.GetUserID() == dto.Id)
            {
                oldUser = _context.User.Where(u => u.Id == dto.Id).SingleOrDefault();

                if (oldUser == null)
                    return BadRequest();

                //_context.User.Update(oldUser);
                oldUser.FromDTO(dto);
                _context.SaveChanges();
                return Ok();
            }

            return Forbid();*/
        }

        [AllowAnonymous]
        [HttpPut("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody] RegistrationComposite composite)
        {
            User newUser = new User();
            // hier keine Id ueberpruefung, da diese eh nicht veraender werden kann
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            newUser.FromDTO(composite.User);
            newUser.Person.LastLogin = DateTime.UtcNow;
            Plan? plan = _context.Plan.SingleOrDefault(p => p.Id == composite.User.Plan.Id);
            if (plan == null)
                return BadRequest();
            newUser.Plan = plan;
            newUser.UserState_ = UserState.Unauthorized;
            newUser.CardId = ExternalPartnerService.GenerateCardId();

            _context.User.Add(newUser);
            _context.SaveChanges();

            var token = await _authService.Register(composite.User.Person.Email, composite.Password);
            if (token != null)
                return Ok(token);

            return BadRequest();
        }

        [Authorize]
        [HttpGet("getnewcard")]
        public ActionResult<int> GetNewCard()
        {
            User? userWithNewCardId;

            userWithNewCardId = _context.User.Where(u => u.Id == User.GetUserID()).SingleOrDefault();
            //userWithNewCardId = _context.User.Where(u => u.CardId == cardId).SingleOrDefault();

            if (userWithNewCardId == null)
                return BadRequest();

            userWithNewCardId.CardId = ExternalPartnerService.GenerateCardId();

            _context.SaveChanges();

            return Ok(userWithNewCardId.CardId);
        }
    }

    public class RegistrationComposite
    {
        public RegistrationComposite(UserDTO user, string password)
        {
            User = user;
            Password = password;
        }

        [Required]
        [JsonPropertyName("user")]
        public UserDTO User { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

}
