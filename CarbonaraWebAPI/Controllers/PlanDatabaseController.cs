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
    public class PlanDatabaseController : ControllerBase
    {
        private PlanDatabaseService Service;
        private AppDbContext Context;
        public PlanDatabaseController(PlanDatabaseService service, AppDbContext context)
        {
            Service = service;
            Context = context;
        }

        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public ActionResult<PlanDTO> Get(int id)
        {
            Plan? obj = Service.GetPlan(id);
            if (obj == null)
                return BadRequest();
            return Ok(new PlanDTO(obj));
        }

        [AllowAnonymous]
        [HttpGet("getall")]
        public ActionResult<IEnumerable<PlanDTO>> GetAll()
        {
            return Ok(Service.GetAllPlans());
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("add")]
        public ActionResult Add([FromBody] PlanDTO dto)
        {
            Service.AddNewPlan(dto);
            return Ok();
        }


        [Authorize(Roles = "Employee")]
        [HttpPut("change/{id}")]
        public ActionResult Change(int id, [FromBody] PlanDTO newDTO)
        {
        
            Plan? old = Context.Plan.SingleOrDefault(c => c.Id == id);
            if (old == null)
                return BadRequest();
            Service.ChangePlan(old, newDTO);
            return Ok();

        }
    }
}
