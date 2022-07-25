using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Plan
    {
        public Plan()
        {
        }

        public Plan( string name, float priceWholeDay, float priceHourDay, float priceHourNight, float priceKm, float registrationFee , float priceHourOverdue)
        {
          
            Name = name;
            PriceWholeDay = priceWholeDay;
            PriceHourDay = priceHourDay;
            PriceHourNight = priceHourNight;
            PriceHourOverdue = priceHourOverdue;
            PriceKm = priceKm;
            RegistrationFee = registrationFee;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float PriceWholeDay { get; set; }

        [Required]
        public float PriceHourDay { get; set; }
        [Required]
        public float PriceHourNight { get; set; }
        [Required]
        public float PriceHourOverdue { get; set; }
        [Required]
        public float PriceKm { get; set; }
        [Required]
        public float RegistrationFee { get; set; }

        public Plan FromDTO(Model.DTO.PlanDTO dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            PriceWholeDay = dto.PriceWholeDay;
            PriceHourDay = dto.PriceHourDay;
            PriceHourNight = dto.PriceHourNight;
            PriceKm = dto.PriceKm;
            RegistrationFee = dto.RegistrationFee;

            return this;
        }
    }
}
