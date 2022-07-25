using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    [Index(nameof(Email), IsUnique = true)]
    public class Person
    {
        public Person()
        {
        }

        public Person(string firstname, string lastname, string email, string title, string formOfAddress, DateTime lastLogin)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Title = title;
            FormOfAddress = formOfAddress;
            LastLogin = lastLogin;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Title { get; set; }
        [Required]
        public string FormOfAddress { get; set; }
        public DateTime LastLogin { get; set; }

        public Person FromDTO(Model.DTO.PersonDTO dto)
        {
            Id = dto.Id;
            Firstname = dto.Firstname;
            Lastname = dto.Lastname;
            Email = dto.Email;
            Title = dto.Title;
            FormOfAddress = dto.FormOfAddress;
            LastLogin = DateTimeOffset.FromUnixTimeMilliseconds(dto.LastLogin).UtcDateTime;

            return this;
        }


    }
}
