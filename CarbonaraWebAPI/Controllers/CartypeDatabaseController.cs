using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarbonaraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartypeDatabaseController : ControllerBase
    {
        private CartypeDatabaseService Service;
        private AppDbContext Context;
        public CartypeDatabaseController(CartypeDatabaseService service, AppDbContext context)
        {
            Service = service;
            Context = context;
        }

        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public ActionResult<CartypeDTO> Get(int id)
        {
            Cartype? obj = Service.GetCartyoe(id);
            if (obj == null)
                return BadRequest();
            return Ok(new CartypeDTO(obj));
        }

        [AllowAnonymous]
        [HttpGet("getAll")]
        public ActionResult<IEnumerable<CartypeDTO>> GetAll()
        {
            return Ok(Service.GetAllCartypes());
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("add")]
        public ActionResult Add([FromBody] CartypeDTO dto)
        {
            Service.AddNewCartype(dto);
            return Ok();
        }


        [Authorize(Roles = "Employee")]
        [HttpPut("change/{id}")]
        public ActionResult Change(int id, [FromBody] CartypeDTO newDTO)
        {
            Cartype? old = Context.Cartype.SingleOrDefault(c => c.Id == id);
            if (old == null)
                return BadRequest();
            Service.ChangeCartype(old, newDTO);
            return Ok();

        }
    }
}
