using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class StationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressDTO Address { get; set; }
        public int Capacity { get; set; }

        public StationDTO(int id, string name, AddressDTO adressDTO, int capacity)
        {
            Id = id;
            Name = name;
            Address = adressDTO;
            Capacity = capacity;
        }

        public StationDTO(Station station, bool flat)
        { 
            Id =  station.Id;
            Name = station.Name;
            Address = flat ? null : new AddressDTO(station.Address);
            Capacity = station.Capacity;
        }

        public StationDTO(Station station) : this(station, false)
        { }

        public StationDTO()
        {

        }
    }

   
}

