using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(Person person, bool isAdmin)
        {
            Person = person;
            IsAdmin = isAdmin;
        }

     
        public Person Person { get; set; }
        public bool IsAdmin { get; set; } = false;
      
 
        [Key]
        [ForeignKey("Person")]
        public int Id { get; set; }

    }
}
