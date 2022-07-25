using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CarbonaraWebAPI.Model.DAO
{
    [ExcludeFromCodeCoverage]
    public class Maintanance
    {
        public Maintanance()
        {
        }

        public Maintanance( DateTime time, bool finished, string description, Car car)
        {
           
            Time = time;
            Finished = finished;
            Description = description;
            Car = car;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public bool Finished { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Car Car { get; set; }
    }
}
