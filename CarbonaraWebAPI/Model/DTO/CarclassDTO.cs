using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class CarclassDTO
    {
       
        public CarclassDTO(Carclass carclass)
        {
            Id = carclass.Id;
            Name = carclass.Name;
            PriceFaktor = carclass.PriceFaktor;
        }

      

        public int Id { get; set; } 
        public string Name { get; set; }
        public float PriceFaktor { get; set; }

        public CarclassDTO()
        {

        }
    }


}
