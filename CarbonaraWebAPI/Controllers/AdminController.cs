using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Services;
using CarbonaraWebAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarbonaraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private AdminService Service;
        private AppDbContext Context;
        public AdminController(AdminService service, AppDbContext context )
        {
            Service = service;
            Context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get/{id}")]
        public ActionResult< EmployeeDTO> GetEmployee(int id)
        {
            Employee? obj = Service.GetEmployee(id);
            if (obj == null)
                return BadRequest();
            return Ok(new EmployeeDTO(obj));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getAll")]
        public ActionResult<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            return Ok(Service.GetAllEmployees());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteEmployee(int id)
        {
          Employee? employee = Context.Employee.Where(c => c.Id == id).Include(c => c.Person).SingleOrDefault();
            if (employee == null)
                return BadRequest();
            Service.DeleteEmployee(employee);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public ActionResult AddEmployee([FromBody] EmployeeDTO dto)
        {
            string password = dto.person.Firstname + "123";
            Service.AddEmployee(dto , password);
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("change/{id}")]
        public ActionResult ChangeEmployee(int id, [FromBody] EmployeeDTO newDTO)
        {
            Employee? current = Context.Employee.Where(c => c.Id == User.GetUserID()).Include(c => c.Person).SingleOrDefault();
            Employee? old = Context.Employee.Where(c => c.Id == id).Include(c => c.Person).SingleOrDefault();
            if (old == null || current == null)
                return BadRequest();
            if (current.Id == old.Id)
                if (newDTO.IsAdmin == false)
                    newDTO.IsAdmin = true;
            Service.ChangeEmployee(old, newDTO);
            

            return Ok();

        }
    }
}
