using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Services
{
    public class PlanDatabaseService
    {
        private AppDbContext Context;
        public PlanDatabaseService(AppDbContext context)
        {
            Context = context;
        }

        public void AddNewPlan(PlanDTO dto)
        {
            Plan plan = new Plan(
                dto.Name,
                dto.PriceWholeDay,
                dto.PriceHourDay,
                dto.PriceHourNight,
                dto.PriceKm,
                dto.RegistrationFee,
                dto.PriceHourOverdue
                );
            Context.Plan.Add(plan);
            Context.SaveChanges();
        }
        public IEnumerable<PlanDTO> GetAllPlans()
        {
            List<PlanDTO> list = new List<PlanDTO>();
            foreach (Plan plan in Context.Plan.ToArray())
            {
                list.Add(new PlanDTO(plan));
            }
            return list;
        }
        public Plan? GetPlan(int id)
        {
            return Context.Plan.SingleOrDefault(c => c.Id == id);
        }
        public void ChangePlan(Plan old, PlanDTO newPlan)
        {
          
            Context.Plan.Update(old);

            old.Name = newPlan.Name;
            old.PriceWholeDay = newPlan.PriceWholeDay;
            old.PriceHourDay = newPlan.PriceHourDay;
            old.PriceHourNight = newPlan.PriceHourNight;
            old.PriceKm = newPlan.PriceKm;
            old.RegistrationFee = newPlan.RegistrationFee;
            old.PriceHourOverdue = newPlan.PriceHourOverdue;

            Context.SaveChanges();
            
        }
    }
}
