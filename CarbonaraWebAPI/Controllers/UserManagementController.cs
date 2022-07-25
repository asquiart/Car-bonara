using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CarbonaraWebAPI.Model.DAO.User;
using UserState = CarbonaraWebAPI.Model.DAO.User.UserState;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarbonaraWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
      
        private AppDbContext Context;
        public UserManagementController(AppDbContext context)
        {
            Context = context;
        }


        // kann auch in einen Service ausgelagert werden und anschliessend koennen alle methoden in den usercontroller mit der funktionensfreigabe
        [Authorize(Roles = "Employee")]
        [HttpGet("getuserlist")]
        public ActionResult<IEnumerable<UserDTO>> GetUserList()
        {
            User user;
            UserDTO userDTO;

            int highestId = Context.User.Max(u => u.Id);    //int? intIdt = Context.Users.Max(u => (int?)u.Id); generiert anscheinend eine exception, wenn kein User eingetragen ist

            List<UserDTO> userList = new List<UserDTO>();

            for (int userId = 1; userId <= highestId; userId++)
            {
                user = Context.User.Where(u => u.Id == userId).Include(u => u.Address).Include(u => u.Person).Include(u => u.Plan).SingleOrDefault();

                if (user != null)
                { 
                    userDTO = new UserDTO(user);
                    userList.Add(userDTO);
                }
            }
            return Ok(userList);
            //gibt immer Ok 200 zurück
            //if (userList != null)
               
            //else
              //  return BadRequest(userList);
        }


        [Authorize(Roles = "Employee")]
        [HttpPatch("setuserstatus/")]
        public ActionResult SetUserStauts([FromBody] Model.DTO.UserDTO dto)
        {
            //User.GetUserID() == dto.Id
            User oldUserStatus;

            oldUserStatus = Context.User.Where(u => u.Id == dto.Id).SingleOrDefault();

            if (oldUserStatus == null)
                return BadRequest();

            oldUserStatus.UserState_ = dto.State;

            Context.SaveChanges();
            return Ok();
        }

        /*[Authorize(Roles = "Employee, Admin")]
        [HttpPut("updateuser/")]
        public ActionResult UpdateUser([FromBody] Model.DTO.UserDTO dto)
        {
            User oldUser;

            oldUser = Context.User.Where(u => u.Id == dto.Id).SingleOrDefault();

            if (oldUser == null)
                return BadRequest();

            Context.User.Update(oldUser);
            oldUser.FromDTO(dto);
            Context.SaveChanges();
            return Ok();
        }*/

        /*[HttpPut("updateuser/")]
        public void SetChangedUserData([FromBody] Model.DTO.UserDTO dto)
        {
            Model.DAO.User oldUser;
         
            oldUser = Context.User.Where(u => u.Id == dto.Id).Single();

            oldUser.FromDTO(dto);
            
            Context.SaveChanges();
        }*/
    }
}
