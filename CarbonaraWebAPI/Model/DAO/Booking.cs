using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Booking
    {

        public Booking()
        {

        }
        public Booking( DateTime starttime, DateTime endtime, DateTime bookingtime, string remarks, bool cancelled, bool returned, User user, Carclass carclass, Station startstation, Station endstation, Car car, Bill? bill)
        {

            Starttime = starttime;
            Endtime = endtime;
            Bookingtime = bookingtime;
            Remarks = remarks;
            Cancelled = cancelled;
            Returned = returned;
            User = user;
            Carclass = carclass;
            Startstation = startstation;
            Endstation = endstation;
            Car = car;
            Bill = bill;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Starttime { get; set; }
        [Required]
        public DateTime Endtime { get; set; }
        [Required]
        public DateTime Bookingtime { get; set; }
        [Required]
        public DateTime ReturnedTime { get; set; }
        public string Remarks { get; set; }
        [Required]
        public bool Cancelled { get; set; } = false;
        [Required]
        public bool Returned { get; set; } = false;
        public User? User { get; set; }
        [Required]
        public Carclass Carclass { get; set; }

        public Station? Startstation { get; set; }
        [Required]
        public Station Endstation { get; set; }

        public float StartKilometers { get; set; }
        public Bill? Bill { get; set; }
        public float EndKilometers { get; set; }
        public Car? Car { get; set; }

    }
}
