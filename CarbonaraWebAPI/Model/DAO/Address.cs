using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Address
    {
        public Address()
        {
        }

        public Address( string number, string zip, string country, string city, string street)
        {
           
            Number = number;
            ZIP = zip;
            Country = country;
            City = city;
            Street = street;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }
        [Required]
        public string ZIP { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }

        public Address FromDTO(Model.DTO.AddressDTO dto)
        {
            Id = dto.Id;
            Number = dto.Number;
            ZIP = dto.ZIP;
            Country = dto.Country;
            City = dto.City;
            Street = dto.Street;

            return this;
        }

    }

}
