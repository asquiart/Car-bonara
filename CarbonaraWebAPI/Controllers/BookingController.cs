using System.Text.Json;
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
    public class BookingController : ControllerBase
    {
        private BookingService BookingService;
        private CarService CarService;
        private AppDbContext Context;
        public BookingController(BookingService service, CarService carService, AppDbContext context)
        {
            CarService = carService;
            BookingService = service;
            Context = context;
        }

        [AllowAnonymous]
        [HttpPut("available")]
        public ActionResult<float> IsBookingAvailable([FromBody] BookingDTOIn dto)
        {

            DateTime StartTime = DateTimeOffset.FromUnixTimeMilliseconds(dto.StartTime).UtcDateTime;
            DateTime EndTime = DateTimeOffset.FromUnixTimeMilliseconds(dto.EndTime).UtcDateTime;

          
            if (BookingService.IsBookingAvailable(dto))
            {
                Carclass carclass = Context.Carclass.Where(c => c.Id == dto.Carclass.Id).Single();               
                double amount = float.MaxValue;
                if (User.Claims.Count() == 0)
                    foreach (Plan plan in Context.Plan.ToArray())
                    {
                        amount = Math.Min(amount, BookingService.GetBillAmount(plan, carclass,StartTime , EndTime , EndTime, 0,   out _));
                    }
                else 
                {
                    User user = Context.User.Where(c => c.Id == User.GetUserID()).Include(c => c.Plan).Single();
                    amount =  BookingService.GetBillAmount(user.Plan, carclass, StartTime, EndTime, EndTime ,0 , out _);
                }
                return Ok((float)amount);
            }
            else
                return Ok(-1f);
        }

        [AllowAnonymous]
        [HttpPut("availableCarclasses")]
        public ActionResult<IEnumerable<CarclassDTO>> GetAvailableCarClasses([FromBody] BookingDTOIn dto)
        {
            return Ok(BookingService.GetAvailableCarClasses(dto));
        }

        [Authorize(Roles = "Authorized")]
        [HttpPost("book")]
        public ActionResult MakeBooking([FromBody] BookingDTOIn dto)
        {
            User user = Context.User.Single(c => c.Id == User.GetUserID());

            if (BookingService.MakeBooking(dto, user))
                return Ok();
            else
                return BadRequest();
        }

        [Authorize(Roles = "Authorized")]
        [HttpPut("isChangePossible/{id}")]
        public ActionResult<bool> IsChangeBookingAllowed(int id, [FromBody] BookingDTOIn newBooking)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == id).Include(c => c.Carclass).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.User).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (User.GetUserID() != booking.User?.Id)
                return Forbid();

            return Ok(BookingService.IsChangeBookingAllowed(booking, newBooking));

        }

        [Authorize(Roles = "Authorized")]
        [HttpPut("change/{id}")]
        public ActionResult ChangeBooking(int id, [FromBody] BookingDTOIn newBooking)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == id).Include(c => c.Carclass).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.User).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (User.GetUserID() != booking.User.Id)
                return Forbid();

            if (BookingService.ChangeBooking(booking, newBooking))
                return Ok();
            else
                return BadRequest();
        }

        [Authorize(Roles = "Authorized")]
        [HttpPut("cancel/{id}")]
        public ActionResult CancelBooking(int id)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == id).Include(c => c.Carclass).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.User).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (User.GetUserID() != booking.User.Id)
                return Forbid();

          
            if (BookingService.CancelBooking(booking))
                return Ok();
            else 
                return BadRequest();
        }

        [Authorize]
        [HttpGet("history")]
        public ActionResult<IEnumerable<BookingDTOOut>> GetBookingHistory()
        {
            User user = Context.User.Single(c => c.Id == User.GetUserID());
            return Ok(BookingService.GetBookingHistory(user));
        }




        //[Authorize(Roles = "Authorized")]
        //[HttpPut("start/{id}")]
        //public ActionResult StartBooking(int id, [FromBody] CarDTO carDTO)
        //{
        //    Booking? booking = Context.Booking.Where(c => c.Id == id).Include(c => c.Carclass).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.User).SingleOrDefault();
        //    if (booking == null)
        //        return BadRequest();
        //    Car? car = Context.Car.Where(c => c.Id == carDTO.Id).Include(c => c.Type).ThenInclude(c => c.Carclass).SingleOrDefault(); 
        //    if (car == null)
        //        return BadRequest();

        //    if (User.GetUserID() != booking.User.Id)
        //        return Forbid();

        //    if (BookingService.StartBooking(booking, car))
        //        return Ok();
        //    else
        //        return BadRequest();
        //}

        [Authorize(Roles = "Authorized")]
        [HttpPut("start/{id}")]
        public ActionResult<CarDTO> StartBooking(int id)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == id).Include(c => c.Carclass).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.User).ThenInclude(c => c.Plan).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (User.GetUserID() != booking.User?.Id)
                return Forbid();

            Car? car = null;
            foreach (Car c in Context.Car.Where(c => true).Include(c => c.Type).ThenInclude(c => c.Carclass).ToArray())
            {
                if (CarService.GetCurrentStation(c) == booking.Startstation)
                    if (CarService.GetCurrentBooking(c) == null)
                        if (c.Type.Carclass == booking.Carclass)
                        {
                            car = c;
                            break;
                        }

            }
            if (car == null)
                return BadRequest();

            if (BookingService.StartBooking(booking, car))
                return Ok(new CarDTO (car));
            else
                return BadRequest();
        }


        [Authorize(Roles = "Authorized")]
        [HttpPut("finish/{id}")]
        public ActionResult FinishBooking(int id)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == id).Include(c => c.Carclass).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.Car).Include(c => c.User).ThenInclude(c => c.Plan).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (User.GetUserID() != booking.User?.Id)
                return Forbid();

            if (BookingService.FinishBooking(booking))
                return Ok();
            else
                return BadRequest();
        }

        //[Authorize(Roles = "Authorized")]
        //[HttpGet("get/{id}")]
        //public ActionResult<BookingDTOOut> GetBooking(int id)
        //{


        //    Booking? booking = BookingService.GetBooking(id);
        //    if (booking == null)
        //        return BadRequest();
        //    if (User.GetUserID() != booking.User?.Id)
        //        return Forbid();


        //    return Ok(new BookingDTOOut(booking));
        //}



        [Authorize]
        [HttpGet("getBill/{bookingId}")]
        public ActionResult<BillDTO?> GetBill(int bookingId)
        {
            Booking? booking = BookingService.GetBooking(bookingId);
            if (booking == null)
                return BadRequest();
            Bill? bill = Context.Bill.Where(b => booking.Bill != null && b.Id == booking.Bill.Id).Include(b => b.Positions).SingleOrDefault();
            if (bill == null)
                return Ok();

            if (User.GetUserID() != bill.User.Id)
                return Forbid();
            return Ok(new BillDTO(bill));
        }


        [Authorize]   
        [HttpGet("bills")]
        public ActionResult<IEnumerable<BillDTO>> GetBills()
        {
            User user = Context.User.Single(c => c.Id == User.GetUserID());
            return Ok(BookingService.GetBills(user));
        }

       

       


    }
}
