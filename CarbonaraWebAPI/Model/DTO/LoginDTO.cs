namespace CarbonaraWebAPI.Model.DTO
{
    public class LoginDTO
    {
        public LoginDTO()
        {

        }
        public LoginDTO(string token, bool isUser, PersonDTO person)
        {
            Token = token;
            IsUser = isUser;
            Person = person;
        }

        public string Token { get; set; }

        public bool IsUser { get; set; }

        public PersonDTO Person { get; set; }

        // token employee|user vorname, nachname, 
    }
}
