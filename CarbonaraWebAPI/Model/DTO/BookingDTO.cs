using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class BookingDTOOut
    {
        public int Id { get; set; }
        public long EndTime { get; set; }
        public long StartTime { get; set; }
        public long ReturnedTime { get; set; }
        public string Remarks { get; set; }
        public bool Cancelled { get; set; } = false;
        public bool Returned { get; set; } = false;
        public UserDTO User { get; set; }
        public CarclassDTO Carclass { get; set; }
        public StationDTO? StartStation { get; set; }
        public StationDTO EndStation { get; set; }
        public float StartKilometers { get; set; }
        public float EndKilometers { get; set; }
        public string CarLicenseNumber { get; set; }
        public float BillAmount { get; set; }
        public BookingDTOOut(Booking booking, bool flat)
        {        
            Id = booking.Id;
            EndTime = new DateTimeOffset(booking.Endtime, TimeSpan.Zero).ToUnixTimeMilliseconds();
            StartTime = new DateTimeOffset(booking.Starttime, TimeSpan.Zero).ToUnixTimeMilliseconds();
            ReturnedTime = booking.Returned ? new DateTimeOffset(booking.ReturnedTime, TimeSpan.Zero).ToUnixTimeMilliseconds() : -1;
            Remarks = booking.Remarks;
            Cancelled = booking.Cancelled;
            Returned = booking.Returned;
            User = new UserDTO(booking.User, flat);
            Carclass =  new CarclassDTO( booking.Carclass);
            StartStation = booking.Startstation != null ? new StationDTO( booking.Startstation, flat) : null;
            EndStation = new StationDTO( booking.Endstation, flat);
            StartKilometers = booking.StartKilometers;
            EndKilometers = booking.EndKilometers;
            CarLicenseNumber = booking.Car != null ? booking.Car.LicensePlateNumber : "-";
            BillAmount = booking.Bill != null ? booking.Bill.Amount : -1;
        }

        public BookingDTOOut(Booking booking) : this(booking, false)
        { }

        public BookingDTOOut()
        {

        }


    }

    public class BookingDTOIn
    {
        public int Id { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public CarclassDTO Carclass { get; set; }
        public StationDTO Startstation { get; set; }
        public StationDTO Endstation { get; set; }

      




    }
}
