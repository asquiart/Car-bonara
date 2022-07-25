using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Car
    {

        public enum CarStatus { onStation, moving, parked , unavailable };
        public enum CarLockStatus { unlocked, locked };

        public Car( int kilometersDriven, float tankLevel, CarStatus status, CarLockStatus lockStatus, Cartype type , string licensePlateNumber)
        {
            LicensePlateNumber = licensePlateNumber;
            KilometersDriven = kilometersDriven;
            TankLevel = tankLevel;
            Status = status;
            LockStatus = lockStatus;
            Type = type;
        }

        public Car()
        {
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int KilometersDriven { get; set; } = 0;
        [Required]
        public float TankLevel { get; set; } = 0;
        [Required]
        public string LicensePlateNumber { get; set; }
        [Required]
        public CarStatus Status { get; set; }
        [Required]
        public CarLockStatus LockStatus { get; set; }
        [Required]
        public Cartype Type { get; set; }
    }
}
