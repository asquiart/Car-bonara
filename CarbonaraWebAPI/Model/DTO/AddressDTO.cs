using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class AddressDTO
    {  
        public int Id { get; set; }
        public string Number { get; set; }
        public string ZIP { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }



        public AddressDTO()
        {

        }
        /*public AddressDTO(int id, string number, string zip, string country, string city, string street)
        {
            Id = id;
            Number = number;
            ZIP = zip;
            Country = country;
            City = city;
            Street = street;
        }*/

        public AddressDTO(Address address)
        {
            Id = address.Id;
            Number = address.Number;
            ZIP = address.ZIP;
            Country = address.Country ?? "Deutschland";
            City = address.City;
            Street = address.Street;
        }
    }
    
}
