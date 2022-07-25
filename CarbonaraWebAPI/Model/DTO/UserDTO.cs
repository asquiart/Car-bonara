using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{

    /**
     * Unauthorisiert Führerschein nicht vorgezeigt -> wie gast darf nicht buchen
     * Authorisiert darf buchen
     * Locked -> wie gast darf nicht buchen
     */


    public class UserDTO
    {
        public int Id { get; set; }
        public PersonDTO Person { get; set; }
        public PlanDTO Plan { get; set; }
        public AddressDTO Address { get; set; }
        public string DriverlicenseNumber { get; set; }
        public User.UserState State { get; set; }
        public int CardId { get; set; }
        public User.PaymentMethod Payment { get; set; }



        public UserDTO(int id, PersonDTO person, PlanDTO plan, AddressDTO address, string driverlicenseNumber, User.UserState userState, int cardId, User.PaymentMethod paymentMethod)
        {
            Id = id;
            Person = person;
            Plan = plan;
            Address = address;
            DriverlicenseNumber = driverlicenseNumber;
            State = userState;
            CardId = cardId;
            Payment = paymentMethod;
        }

        public UserDTO(User user, bool flat)
        {
            Id = user.Id;
            Person = new PersonDTO(user.Person);
            Plan = flat ? null : new PlanDTO(user.Plan);
            Address = flat ? null : new AddressDTO(user.Address);
            DriverlicenseNumber = user.DriverlicenseNumber;
            State = user.UserState_;
            CardId = user.CardId;
            Payment = user.PaymentMethod_;
        }
        public UserDTO(User user) : this(user, false)
        { }

        public UserDTO()
        {

        }

    }
}
