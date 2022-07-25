using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Cartype
    {
        public Cartype()
        {
        }

        public Cartype( string name , string manufacturer, string fueltype, Carclass carclass)
        {
          
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.Fueltype = fueltype;
            this.Carclass = carclass;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Fueltype { get; set; }
        [Required]
        public Carclass Carclass { get; set; }
    }
}
