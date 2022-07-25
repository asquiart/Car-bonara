using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Carclass
    {
        public Carclass() 
        {
        }

        public Carclass( string name , float priceFaktor)
        {
  
            this.Name = name;
            this.PriceFaktor = priceFaktor;
        }
      
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
        [Required]
        public float PriceFaktor { get; set; }
    }
}
