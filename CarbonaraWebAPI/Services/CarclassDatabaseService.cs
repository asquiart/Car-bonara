using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Services
{
    public class CarclassDatabaseService
    {
        private AppDbContext Context;
        public CarclassDatabaseService(AppDbContext context)
        {
            Context = context;
        }

        public void AddNewCarclass(CarclassDTO dto)
        {
            Carclass carclass = new Carclass(
                dto.Name,
                dto.PriceFaktor
                );
            Context.Carclass.Add(carclass);
            Context.SaveChanges();
        }
        public IEnumerable<CarclassDTO> GetAllCarclasses()
        {
            List<CarclassDTO> list = new List<CarclassDTO>();
            foreach (Carclass carclass in Context.Carclass.ToArray())
            {
                list.Add(new CarclassDTO(carclass));
            }
            return list;
        }
        public Carclass? GetCarclass(int id)
        {
            return Context.Carclass.SingleOrDefault(c => c.Id == id);
        }
        public void ChangeCarclass(Carclass old, CarclassDTO newCarclass)
        {
          
            Context.Carclass.Update(old);

            old.PriceFaktor = newCarclass.PriceFaktor;
            old.Name = newCarclass.Name;

            Context.SaveChanges();
        }

    }
}
