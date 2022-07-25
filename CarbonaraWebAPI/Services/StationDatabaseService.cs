using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class StationDatabaseService
    {
        private AppDbContext Context;
        public StationDatabaseService(AppDbContext context)
        {
            Context = context;
        }
        public void AddNewStation(StationDTO dto)
        {
            Address adress = new Address(
                dto.Address.Number,
                dto.Address.ZIP,
                dto.Address.Country,
                dto.Address.City,
                dto.Address.Street);
            Context.Address.Add(adress);

            Station station = new Station(
                dto.Name,
                adress,
                dto.Capacity
                );

            Context.Station.Add(station);
            Context.SaveChanges();
        }
        public IEnumerable<StationDTO> GetAllStations()
        {
            List<StationDTO> list = new List<StationDTO>();
            foreach (Station station in Context.Station.Where(c => true).Include(c => c.Address).ToArray())
            {
                list.Add(new StationDTO(station));
            }
            return list;


        }
        public void ChangeStation(Station old, StationDTO newStation)
        {
          
            Context.Station.Update(old);
            Context.Address.Update(old.Address);

            old.Capacity = newStation.Capacity;
            old.Name = newStation.Name;

            old.Address.Street = newStation.Address.Street;
            old.Address.City = newStation.Address.City;
            old.Address.ZIP = newStation.Address.ZIP;
            old.Address.Number = newStation.Address.Number;
            old.Address.Country = newStation.Address.Country;

            Context.SaveChanges();

        }
        public Station? GetStation(int id)
        {
            return Context.Station.Where(c => c.Id == id).Include(c => c.Address).SingleOrDefault();
        }



    }
}
