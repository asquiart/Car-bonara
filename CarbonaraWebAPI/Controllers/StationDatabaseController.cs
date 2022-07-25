using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarbonaraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationDatabaseController : ControllerBase
    {
        private StationDatabaseService Service;
        private AppDbContext Context;
        public StationDatabaseController(StationDatabaseService service, AppDbContext context)
        {
            Service = service;
            Context = context;
        }

        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public ActionResult<StationDTO> Get(int id)
        {
            Station? obj = Service.GetStation(id);
            if (obj == null)
                return BadRequest();
            return Ok(new StationDTO(obj));
        }

        [AllowAnonymous]
        [HttpGet("getAll")]
        public ActionResult<IEnumerable<StationDTO>> GetAll()
        {
            return Ok(Service.GetAllStations());
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("add")]
        public ActionResult Add([FromBody] StationDTO dto)
        {
            Service.AddNewStation(dto);
            return Ok();
        }


        [Authorize(Roles = "Employee")]
        [HttpPut("change/{id}")]
        public ActionResult Change(int id, [FromBody] StationDTO newDTO)
        {
            Station? old = Context.Station.Where(c => c.Id == id).Include(c => c.Address).SingleOrDefault();
            if (old == null)
                return BadRequest();
            Service.ChangeStation(old, newDTO);
            return Ok();

        }
    }
}
