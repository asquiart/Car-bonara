using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CarbonaraWebAPI.Model.DAO
{
    [ExcludeFromCodeCoverage]
    public class Picture
    {
        public Picture()
        {
        }

        public Picture(User user, string format, byte[] daten)
        {
            User = user;
            Format = format;
            Daten = daten;
        }


        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

       
        public User User { get; set; }
        [Required]
        public string Format { get; set; }
        [Required]
        public byte[] Daten { get; set; }

    }
}
