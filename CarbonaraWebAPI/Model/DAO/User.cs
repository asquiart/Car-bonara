using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarbonaraWebAPI.Model.DAO
{
    public class User
    {

        public enum PaymentMethod { Paypal, Sepa, Visa , Mastercard , Maestro, Giropay };
        public enum UserState { Unauthorized, Authorized, Locked };

        public User(Person person, Plan plan, Address address,  string driverlicenseNumber, UserState userState, int cardId, PaymentMethod paymentMethod)
        {
            Person = person;
            Plan = plan;
            Address = address;
            DriverlicenseNumber = driverlicenseNumber;
            UserState_ = userState;
            PaymentMethod_ = paymentMethod;
            CardId = cardId;
        }

        public User()
        {
        }
        [Key]
        [ForeignKey("Person")]
        public int Id { get; set; }

        public Person Person { get; set; }        
        [Required]
        public Plan Plan { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public string DriverlicenseNumber { get; set; }
        [Required]
        public UserState UserState_ { get; set; }
        [Required]
        public int CardId { get; set; }
        [Required]
        public PaymentMethod PaymentMethod_ { get; set; }

        // aus der sicht des DAO
        /*public Model.DTO.UserDTO ToDTO()
        {
            Model.DTO.UserDTO dtoUser = new Model.DTO.UserDTO()
            {
                Id = Id,
                Person = Person,
                Plan = Plan,
                Address = Address,
                DriverlicenseNumber = DriverlicenseNumber,
                UserState_ = (DTO.UserState)UserState_,
                CardId = CardId,
                PaymentMethod_ = (DTO.PaymentMethod)PaymentMethod_
            };
            return dtoUser;
        }*/

        public User FromDTO(Model.DTO.UserDTO dto)
        {
            Address dtoAddress = new Address();
            Person dtoPerson = new Person();
            Plan dtoPlan = new Plan();


            dtoAddress.FromDTO(dto.Address);
            dtoPerson.FromDTO(dto.Person);
            dtoPlan.FromDTO(dto.Plan);

            Id = dto.Id;
            Person = dtoPerson;
            Plan = dtoPlan;
            Address = dtoAddress;
            DriverlicenseNumber = dto.DriverlicenseNumber;
            UserState_ = dto.State;
            CardId = dto.CardId;
            PaymentMethod_ = dto.Payment;

            
            return this;

        }

        /*public static User FromDTO(Model.DTO.User dto)
        {
            User daoUser = new User()
            {
                Id = dto.Id,
                Person = dto.Person,
                Plan = dto.Plan,
                Address = dto.Address,
                DriverlicenseNumber = dto.DriverlicenseNumber,
                UserState_ = (UserState)dto.UserState_,
                CardId = dto.CardId,
                PaymentMethod_ = (PaymentMethod)dto.PaymentMethod_

            };
            return daoUser;
            
        }*/

    }

    

}
