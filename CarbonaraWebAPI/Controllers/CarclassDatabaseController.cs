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
    public class CarclassDatabaseController : ControllerBase
    {
        private CarclassDatabaseService Service;
        private AppDbContext Context;
        public CarclassDatabaseController(CarclassDatabaseService service, AppDbContext context)
        {
            Service = service;
            Context = context;
        }

        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public ActionResult<CarclassDTO> Get(int id)
        {
            Carclass? obj = Service.GetCarclass(id);
            if (obj == null)
                return BadRequest();
            return Ok(new CarclassDTO(obj));
        }

        [AllowAnonymous]
        [HttpGet("getall")]
        public ActionResult<IEnumerable<CarclassDTO>> GetAll()
        {
            return Ok(Service.GetAllCarclasses());
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("add")]
        public ActionResult Add([FromBody] CarclassDTO dto)
        {
            Service.AddNewCarclass(dto);
            return Ok();
        }


        [Authorize(Roles = "Employee")]
        [HttpPut("change/{id}")]
        public ActionResult Change(int id, [FromBody] CarclassDTO newDTO)
        {
            Carclass? old = Context.Carclass.SingleOrDefault(c => c.Id == id);
            if (old == null)
                return BadRequest();
            Service.ChangeCarclass(old, newDTO);
            return Ok();

        }
    }
}
