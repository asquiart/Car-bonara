using CarbonaraWebAPI;
using CarbonaraWebAPI.Controllers;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPITest.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarbonaraWebAPITest
{
    [Collection("Integration Tests")]
    public class UserTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
        public UserTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RegisterUserAsync()
        {
            UserDTO newUser = new UserDTO(DataSeeder.user);
            newUser.State = User.UserState.Unauthorized;

            string urlRegister = "/api/user/register";
            string urlCurrent = "/api/user/current";
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            // register
            var body = new RegistrationComposite(newUser, "nein").ConvertToStringBody();
            var client = _factory.CreateClient();
            var response = await client.PutAsync(urlRegister, body);

            // set token
            Assert.Equal(expectedStatus, response.StatusCode);
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // get user
            response = await client.GetAsync(urlCurrent);
            Assert.Equal(expectedStatus, response.StatusCode);
            UserDTO responseUser = await response.ConvertToTypeFromStringAsync<UserDTO>();

            // check state
            Assert.Equal(User.UserState.Unauthorized, responseUser.State);
            newUser.State = User.UserState.Unauthorized;

            // check new IDs
            Assert.NotEqual(newUser.Id, responseUser.Id);
            newUser.Id = responseUser.Id;
            newUser.Person.Id = responseUser.Id;
            newUser.Address.Id = responseUser.Id;
            Assert.NotEqual(newUser.CardId, responseUser.CardId);
            newUser.CardId = responseUser.CardId;

            // Last Login fewer than 2 Minutes ago
            Assert.NotEqual(newUser.Person.LastLogin, responseUser.Person.LastLogin);
            Assert.True((int)DateTime.UtcNow.Subtract(DateTimeOffset.FromUnixTimeMilliseconds(responseUser.Person.LastLogin).UtcDateTime).TotalMinutes < 2);
            newUser.Person.LastLogin = responseUser.Person.LastLogin;


            //Ignore Id because It is newly generated
            responseUser.Address.Id = newUser.Address.Id;
            // check User
            
            Assert.Equal(newUser.ToJson(), responseUser.ToJson());
        }

        [Fact]
        public async Task UpdateUserAsync_Success()
        {
            User oldUser = DataSeeder.users[0];

          
         

            string pw = DataSeeder.people[3].Firstname + "123";

            string urlLogin = "/api/auth/login";
            string urlCurrent = "/api/user/current";
            string urlUpdate = "/api/user/update";

            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            var body = new LoginRequest(DataSeeder.people[3].Email, pw).ConvertToStringBody();

            // login
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedStatus, response.StatusCode);

            // set token
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // get current User to compare after updated user
            response = await client.GetAsync(urlCurrent);
            Assert.Equal(expectedStatus, response.StatusCode);
            UserDTO responseDTO = await response.ConvertToTypeFromStringAsync<UserDTO>();


            // build updated user
            PersonDTO newPersonDTO = new(oldUser.Id, "firstname", "lastname", "firstname.lastname@email.de", "title", "formOfAddress", new DateTimeOffset(new DateTime(2022, 07, 21)).ToUnixTimeMilliseconds());
            AddressDTO newAddressDTO = new(DataSeeder.RandAddress());
            UserDTO newUserDTO = new(oldUser.Id, newPersonDTO, new PlanDTO(DataSeeder.plans[1]), newAddressDTO, "xxx", User.UserState.Authorized, 0, User.PaymentMethod.Mastercard);

            // update
            body = newUserDTO.ConvertToStringBody(); 
            response = await client.PutAsync(urlUpdate, body);
            Assert.Equal(expectedStatus, response.StatusCode);


            // get updated User
            response = await client.GetAsync(urlCurrent);
            Assert.Equal(expectedStatus, response.StatusCode);

            // check user
            responseDTO = await response.ConvertToTypeFromStringAsync<UserDTO>();
            responseDTO.Person.LastLogin = newUserDTO.Person.LastLogin;
            //Ignore Adress Id because it is just generated
            newUserDTO.Address.Id = responseDTO.Address.Id;
            Assert.Equal(newUserDTO.ToJson(), responseDTO.ToJson());

            // reverse update
            UserDTO oldUserDTO = new UserDTO(oldUser);
            body = oldUserDTO.ConvertToStringBody();
            response = await client.PutAsync(urlUpdate, body);
            Assert.Equal(expectedStatus,response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserAsync_BadRequest()
        {
            //int personID = 4;

            User oldUser = DataSeeder.users[0];


          //, true muss das true?

            // Person oldPerson = DataSeeder.people[3];
            //oldPerson.Id = 4;

            //PersonDTO oldPersonDTO = new PersonDTO(oldPerson);

            //oldUserDTO.Person = oldPersonDTO;

            //oldUserDTO.Plan.Id = 1;

            string pw = DataSeeder.users[0].Person.Firstname + "123";

            string urlLogin = "/api/auth/login";
            string urlCurrent = "/api/user/current";
            string urlUpdate = "/api/user/update";

            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            HttpStatusCode expectedBadRequest = HttpStatusCode.BadRequest;



            var body = new LoginRequest(DataSeeder.users[0].Person.Email, pw).ConvertToStringBody();
            
        

            // login
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedStatus, response.StatusCode);

            // set token
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // get current User to compare after updated user

            response = await client.GetAsync(urlCurrent);
            Assert.Equal(expectedStatus, response.StatusCode);

            UserDTO responseDTO = (UserDTO)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(UserDTO));
            UserDTO oldUserDTO = new UserDTO(oldUser);
            responseDTO.Person.LastLogin = oldUserDTO.Person.LastLogin;

         
            //Assert.NotEqual(oldUserDTO.Person.LastLogin, responseDTO.Person.LastLogin);
            Assert.Equal(oldUserDTO.ToJson(), responseDTO.ToJson());


            Plan plan = DataSeeder.plans[1];
            //plan.Id = 2;

            PersonDTO newPersonDTO = new(0, "firstname", "lastname", "firstname.lastname@email.de", "title", "formOfAddress", new DateTimeOffset(new DateTime(2022, 07, 21)).ToUnixTimeMilliseconds());
            AddressDTO newAddressDTO = new(DataSeeder.RandAddress());
            UserDTO newUserDTO = new(0, newPersonDTO, new PlanDTO(plan), newAddressDTO, "xxx", User.UserState.Authorized, 0, User.PaymentMethod.Mastercard); ; ;

            newUserDTO.Id = 4;
            newUserDTO.Person.Id = 4;
            newUserDTO.Address.Id = 4;
            newUserDTO.Plan.Id = -2;


            body = newUserDTO.ConvertToStringBody();
            response = await client.PutAsync(urlUpdate, body);

            Assert.Equal(expectedBadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetNewCard()
        {
            string urlLogin = "/api/auth/login";
            string urlCurrent = "/api/user/current";
            string url = "api/user/getnewcard";
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            // login
            UserDTO user = new(DataSeeder.users[1]);
            var body = new LoginRequest(user.Person.Email, user.Person.Firstname + "123").ConvertToStringBody();
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);

            // set token
            Assert.Equal(expectedStatus, response.StatusCode);
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // get old cardid
            response = await client.GetAsync(urlCurrent);
            Assert.Equal(expectedStatus, response.StatusCode);
            int oldCardID = (await response.ConvertToTypeFromStringAsync<UserDTO>()).CardId;

            // get new cardid
            response = await client.GetAsync(url);
            Assert.Equal(expectedStatus, response.StatusCode);
            int newCardID = (await response.ConvertToTypeFromStringAsync<int>());
            Assert.NotEqual(oldCardID, newCardID);

            // check cardid on user
            response = await client.GetAsync(urlCurrent);
            Assert.Equal(expectedStatus, response.StatusCode);
            int userCardID = (await response.ConvertToTypeFromStringAsync<UserDTO>()).CardId;
            Assert.Equal(newCardID, userCardID);
        }

        [Fact]
        public async Task SetUserStatusOk()
        {
            string urlLogin = "/api/auth/login";
            

            string url = "api/usermanagement/setuserstatus/";
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            // login
            EmployeeDTO employee = new(DataSeeder.employees[2]);
            PersonDTO person = new(DataSeeder.people[2]);

            var body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);

            // set token
            Assert.Equal(expectedStatus, response.StatusCode);
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);



            // get userstatus
            User unauthorizedUser = DataSeeder.users[1];

            Assert.Equal(User.UserState.Unauthorized, unauthorizedUser.UserState_);


            // set userstatus to Authorized
            User newUsterstaus = DataSeeder.users[1];
            newUsterstaus.UserState_ = User.UserState.Authorized;

            UserDTO newUsterstausDTO = new UserDTO(newUsterstaus);

            body = newUsterstausDTO.ConvertToStringBody();

            response = await client.PatchAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);
            
            Assert.Equal(User.UserState.Authorized, unauthorizedUser.UserState_);
        }

    }
}