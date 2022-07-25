using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class CarDTO
    {
     
        public int Id { get; set; }
      
        public int KilometersDriven { get; set; } = 0;
      
        public string LicensePlateNumber { get; set; }

        public float TankLevel { get; set; } = 0;
       
        public DAO.Car.CarStatus Status { get; set; }
       
        public DAO.Car.CarLockStatus LockStatus { get; set; }
        
        public CartypeDTO Type { get; set; }

      

        public CarDTO(Car car)
        {
            Id = car.Id;
            KilometersDriven = car.KilometersDriven;
            TankLevel = car.TankLevel;
            Status = car.Status;
            LockStatus = car.LockStatus;
            Type = new CartypeDTO( car.Type);
            LicensePlateNumber = car.LicensePlateNumber;
        }
        public CarDTO()
        {

        }
    }
 }

