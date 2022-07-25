using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class CartypeDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Fueltype { get; set; }
        public CarclassDTO Carclass { get; set; }

       
        public CartypeDTO(Cartype cartype)
        {
            Id = cartype.Id;
            Name = cartype.Name;
            Manufacturer = cartype.Manufacturer;
            Fueltype = cartype.Fueltype;
            Carclass = new CarclassDTO( cartype.Carclass);
        }

        public CartypeDTO()
        {

        }

    }
}

