using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Station
    {
        public Station()
        {
        }

        public Station( string name, Address address, int capacity)
        {
          
            Name = name;
            Address = address;
            Capacity = capacity;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public int Capacity { get; set; }
    }
}
