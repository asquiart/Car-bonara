using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class CarDatabaseService
    {
        private AppDbContext Context;
        private BookingService BookingService;
        public CarDatabaseService(AppDbContext context, BookingService bookingService)
        {
            Context = context;
            BookingService = bookingService;
        }

        public bool AddNewCar(CarDTO dto, Station firstStation, DateTime availableFrom)
        {
            Cartype? cartype = Context.Cartype.Where(c => c.Id == dto.Type.Id).Include(c => c.Carclass).SingleOrDefault();
            if (cartype == null)
                return false;

            if (!BookingService.CanAddInitialBooking(firstStation, cartype.Carclass, availableFrom))
                return false;


            Car car = new Car(
                dto.KilometersDriven,
                dto.TankLevel,
                dto.Status,
                dto.LockStatus,
                cartype,
                dto.LicensePlateNumber);
            Context.Car.Add(car);

        

            Booking firstBooking = new Booking(
                availableFrom,
                availableFrom,
                DateTime.Now,
                "Initial Booking",
                false,
                true,
                null,
                car.Type.Carclass,
                null,
                firstStation,
                car,
                null);
            Context.Booking.Add(firstBooking);

            Context.SaveChanges();
            return true;
        }
        public IEnumerable<CarDTO> GetAllCars()
        {
            List<CarDTO> list = new List<CarDTO>();
            foreach (Car car in Context.Car.Where(c => true).Include(c => c.Type).ThenInclude(c => c.Carclass).ToArray())
            {
                list.Add(new CarDTO(car));
            }
            return list;


        }
        public void ChangeCar(Car old, CarDTO newCar)
        {
          
            
            Context.Car.Update(old);

            old.KilometersDriven = newCar.KilometersDriven;
            old.TankLevel = newCar.TankLevel;
            old.Status = newCar.Status;
            old.LockStatus = newCar.LockStatus;
            old.LicensePlateNumber = newCar.LicensePlateNumber;
            Cartype? type = Context.Cartype.Where(c => c.Id == newCar.Type.Id).SingleOrDefault();
            if (type != null)
                old.Type = type;
            Context.SaveChanges();

        }
        public Car? GetCar(int id)
        {
            return Context.Car.Where(c => c.Id == id).Include(c => c.Type).ThenInclude(c => c.Carclass).SingleOrDefault();
        }

    }
}
