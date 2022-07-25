using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class CartypeDatabaseService
    {
        private AppDbContext Context;
        public CartypeDatabaseService(AppDbContext context)
        {
            Context = context;
        }
        public void AddNewCartype(CartypeDTO dto)
        {
            Cartype cartype = new Cartype(
                dto.Name,
                dto.Manufacturer,
                dto.Fueltype,
                Context.Carclass.Single(c => c.Id == dto.Carclass.Id)
                );
            Context.Cartype.Add(cartype);
            Context.SaveChanges();
        }
        public IEnumerable<CartypeDTO> GetAllCartypes()
        {
            List<CartypeDTO> list = new List<CartypeDTO>();
            foreach (Cartype cartype in Context.Cartype.Include(c => c.Carclass).ToArray())
            {
                list.Add(new CartypeDTO(cartype));
            }
            return list;
        }
        public Cartype? GetCartyoe(int id)
        {
            return Context.Cartype.Where(c => c.Id == id).Include(c => c.Carclass).SingleOrDefault();
        }
        public void ChangeCartype(Cartype old, CartypeDTO newCartype)
        {
           

            Context.Cartype.Update(old);

            old.Carclass = Context.Carclass.Single(c => c.Id == newCartype.Carclass.Id);
            old.Manufacturer = newCartype.Manufacturer;
            old.Fueltype = newCartype.Fueltype;
            old.Name = newCartype.Name;

            Context.SaveChanges();
        }

    }
}
