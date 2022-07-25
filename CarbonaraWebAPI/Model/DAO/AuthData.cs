using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarbonaraWebAPI.Model.DAO
{
    public class AuthData
    {
        [Key]
        [ForeignKey("Person")]
        public int Id { get; set; }
        public Person Person { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }

        public AuthData AddPerson(Person person) {
            this.Person = person;
            return this;
        }
    }
}
