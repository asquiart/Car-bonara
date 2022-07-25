using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class PlanDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float PriceWholeDay { get; set; }
        public float PriceHourDay { get; set; }
        public float PriceHourNight { get; set; }
        public float PriceHourOverdue { get; set; }
        public float PriceKm { get; set; }
        public float RegistrationFee { get; set; }


        public PlanDTO(int id, string name, float priceWholeDay, float priceHourDay, float priceHourNight, float priceKm, float registrationFee , float priceHourOverdue)
        {
            Id = id;
            Name = name;
            PriceWholeDay = priceWholeDay;
            PriceHourDay = priceHourDay;
            PriceHourNight = priceHourNight;
            PriceHourOverdue = priceHourOverdue;
            PriceKm = priceKm;
            RegistrationFee = registrationFee;
        }


        public PlanDTO(Plan plan)
        {
            Id = plan.Id;
            Name = plan.Name;
            PriceWholeDay = plan.PriceWholeDay;
            PriceHourDay = plan.PriceHourDay;
            PriceHourNight = plan.PriceHourNight;
            PriceHourOverdue = plan.PriceHourOverdue;
            PriceKm = plan.PriceKm;
            RegistrationFee = plan.RegistrationFee;
        }

        public PlanDTO()
        {

        }
      
    }

}
