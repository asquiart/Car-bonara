using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class CarService
    {
        private AppDbContext Context;
        public CarService(AppDbContext context)
        {
            Context = context;
        }

        public bool IsLocked(Car car )
        {

            return car.LockStatus == Car.CarLockStatus.locked;
        }

        public void LockCar(Car car)
        {

            car.LockStatus = Car.CarLockStatus.locked;

            Context.SaveChanges();
        }

        public void UnlockCar( Car car)
        {
         

            car.LockStatus = Car.CarLockStatus.unlocked;

            Context.SaveChanges();
        }

        public float GetMillage(Car car)
        {
           

            //Mock Millage
            int millage = car.KilometersDriven + 1 ;
            //

            car.KilometersDriven = millage;        
            Context.SaveChanges();
            return millage;
        }

        public Booking? GetCurrentBooking(Car car)
        {
            return Context.Booking.Where(c => c.Car.Id == car.Id && !c.Returned).Include(c => c.Endstation).Include(c => c.Startstation).Include(c => c.Carclass).Include(c => c.User).ThenInclude(u => u.Person).SingleOrDefault();
            
        }
        public Station? GetCurrentStation(Car car)
        {
          

            if (Context.Booking.SingleOrDefault(c => c.Car.Id == car.Id && !c.Returned) != null)
                return null;
            List<Booking> bookingsOfCar = Context.Booking.Where(c => c.Car == car).Include(c => c.Endstation).ThenInclude(c => c.Address).ToList();
            if (bookingsOfCar.Count == 0)
                return null;
            return bookingsOfCar.OrderBy(c => -c.Endtime.Ticks).FirstOrDefault()?.Endstation;
           
               
        }
    }
}
