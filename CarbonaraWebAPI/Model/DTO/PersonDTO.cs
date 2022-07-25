using CarbonaraWebAPI.Model.DAO;

using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }    
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Title { get; set; }
        public string FormOfAddress { get; set; }
        public long LastLogin { get; set; }

        public PersonDTO(int id, string firstname, string lastname, string email, string title, string formOfAddress, long lastLogin)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Title = title;
            FormOfAddress = formOfAddress;
            LastLogin = lastLogin;
        }

        public PersonDTO(Person person)
        {
            Id = person.Id;
            Firstname = person.Firstname;
            Lastname = person.Lastname;
            Email = person.Email;
            Title = person.Title;
            FormOfAddress = person.FormOfAddress;
            LastLogin = new DateTimeOffset(person.LastLogin, TimeSpan.Zero).ToUnixTimeMilliseconds();
        }
        public PersonDTO()
        {

        }
    }
}

