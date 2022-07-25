using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CarbonaraWebAPI.Model.DAO
{
    [ExcludeFromCodeCoverage]
    public class Location
    {
        public Location()
        {
        }

        public Location( double lon, double lat, DateTime time, Car car)
        {
           
            Lon = lon;
            Lat = lat;
            Time = time;
            Car = car;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public double Lon { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public Car Car { get; set; }
    }
}
