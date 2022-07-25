using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class AdminService
    {
        private AuthService AuthService;
        private AppDbContext Context;
        public AdminService(AppDbContext context, AuthService authService)
        {
            AuthService = authService;
            Context = context;
        }
        public Employee? GetEmployee(int id)
        {
            return Context.Employee.Where(c => c.Id == id).Include(c => c.Person).SingleOrDefault();
        }


        public IEnumerable<EmployeeDTO> GetAllEmployees()
        {
            List<EmployeeDTO> list = new List<EmployeeDTO>();
            foreach (Employee employee in Context.Employee.Where(c => true).Include(c => c.Person).ToArray())
            {
                  list.Add(new EmployeeDTO(employee));
            }
            return list;
        }


        public void DeleteEmployee(Employee employee)
        {
           
            Context.Employee.Remove(employee);
            Context.Person.Remove(employee.Person);
            Context.SaveChanges();
        }

        public void AddEmployee(EmployeeDTO dto , string password)
        {
            Person person = new Person(
                dto.person.Firstname,
                dto.person.Lastname,
                dto.person.Email,
                dto.person.Title,
                dto.person.FormOfAddress,
                DateTime.UtcNow);

            Employee employee = new Employee(person, dto.IsAdmin);

            Context.Employee.Add(employee);
            Context.Person.Add(person);
            Context.SaveChanges();
            AuthService.Register(person.Email, password).Wait();
        }

        public void ChangeEmployee(Employee old , EmployeeDTO newDTO)
        {
           
          
            old.IsAdmin = newDTO.IsAdmin;
            old.Person.Firstname = newDTO.person.Firstname;
            old.Person.Lastname = newDTO.person.Lastname;
            old.Person.Email = newDTO.person.Email;
            old.Person.FormOfAddress = newDTO.person.FormOfAddress;
            old.Person.Title = newDTO.person.Title;

            Context.SaveChanges();
            
        }
    }
}
