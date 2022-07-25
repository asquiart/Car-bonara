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
    public class CarController : ControllerBase
    {

        private CarService Service;
        private AppDbContext Context;
        public CarController(CarService service, AppDbContext context)
        {
            Service = service;
            Context = context;
        }

        [Authorize(Roles = "Authorized")]
        [HttpGet("car/{bookingId}")]
        public ActionResult<CarDTO> GetCarOfBooking(int bookingId)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == bookingId).Include(c => c.Car).ThenInclude(c => c.Type).ThenInclude(t => t.Carclass).Include(c => c.User).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (booking.User.Id != User.GetUserID())
                return Forbid();

            Car? car = booking?.Car;
            if (car == null)
                return BadRequest();
            return Ok(new CarDTO(car));
        }

            [Authorize(Roles = "Authorized")]
        [HttpPut("lock/{bookingId}")]
        public ActionResult<Car> LockCar(int bookingId)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == bookingId).Include(c => c.Car).Include(c => c.User).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (booking.User.Id != User.GetUserID())
                return Forbid();

            Car? car = booking?.Car;
            if (car == null)
                return BadRequest();

            if (Service.GetCurrentBooking(car)?.Id != booking.Id)
                return BadRequest();


            Service.LockCar(car);
            return Ok(car);
        }

        [Authorize(Roles = "Authorized")]
        [HttpPut("unlock/{bookingId}")]
        public ActionResult<Car> UnlockCar(int bookingId)
        {
            Booking? booking = Context.Booking.Where(c => c.Id == bookingId).Include(c => c.Car).Include(c => c.User).SingleOrDefault();
            if (booking == null)
                return BadRequest();

            if (booking.User.Id != User.GetUserID())
                return Forbid();

            Car? car = booking?.Car;
            if (car == null)
                return BadRequest();

         
            if (Service.GetCurrentBooking(car)?.Id != booking.Id)
                return BadRequest();
     


            Service.UnlockCar(car);
            return Ok(car);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("km/{id}")]
        public ActionResult<float> GetMillage(int id)
        {
            Car? car = Context.Car.Where(c => c.Id == id).SingleOrDefault();
            if (car == null)
                return BadRequest();

            return Ok(Service.GetMillage(car));
        }


        [Authorize(Roles = "Employee")]
        [HttpGet("booking/{id}")]
        public ActionResult<BookingDTOOut> GetCurrentBooking(int id)
        {
            Car? car = Context.Car.Where(c => c.Id == id).Include(c => c.Type).ThenInclude(c => c.Carclass).SingleOrDefault();
            if (car == null)
                return BadRequest();

            Booking? booking = Service.GetCurrentBooking(car);
            if (booking == null)
                return Ok(null);
            return Ok(new BookingDTOOut(booking, true));
        }
       
    }
}
