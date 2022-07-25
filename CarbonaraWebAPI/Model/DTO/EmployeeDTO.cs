using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class EmployeeDTO
    {
         public  PersonDTO person { get; set; }     
         public bool IsAdmin { get; set; }

        /*public EmployeeDTO(PersonDTO personDTO, bool isAdmin)
        {
            person = personDTO;
            IsAdmin = isAdmin;
        }*/

        public EmployeeDTO(Employee employee)
        {
            person = new PersonDTO(employee.Person);
            IsAdmin = employee.IsAdmin;
        }

        public EmployeeDTO()
        {
                
        }
    }
}
