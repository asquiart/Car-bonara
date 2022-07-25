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
    public class CarDatabaseController : ControllerBase
    {
        private CarDatabaseService Service;
        private AppDbContext Context;
        public CarDatabaseController(CarDatabaseService service, AppDbContext context)
        {
            Service = service;
            Context = context;
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("get/{id}")]
        public ActionResult<CarDTO> Get(int id)
        {
            Car? obj = Service.GetCar(id);
            if (obj == null)
                return BadRequest();
            return Ok(new CarDTO(obj));
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("getAll")]
        public ActionResult<IEnumerable<CarDTO>> GetAll()
        {
            return Ok(Service.GetAllCars());
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("add")]
        public ActionResult Add([FromBody] AddCarDTO dto)
        {
            Station? station = Context.Station.SingleOrDefault(c => c.Id == dto.Station.Id);
            if (station == null)
                return BadRequest();
            if (Service.AddNewCar(dto.Car, station,  DateTimeOffset.FromUnixTimeMilliseconds(dto.Time).UtcDateTime))
                return Ok();
            return BadRequest();
        }


        [Authorize(Roles = "Employee")]
        [HttpPut("change/{id}")]
        public ActionResult Change(int id, [FromBody] CarDTO newDTO)
        {
            Car? old = Context.Car.SingleOrDefault(c => c.Id == id);
            newDTO.Id = id;
            if (old == null)
                return BadRequest();
            Service.ChangeCar(old, newDTO);
            return Ok();

        }
    }

  

}