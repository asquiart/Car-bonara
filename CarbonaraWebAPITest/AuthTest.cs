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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static CarbonaraWebAPITest.Util.DataSeeder;

namespace CarbonaraWebAPITest
{
    [Collection("Integration Tests")]
    public class AuthTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
        public AuthTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        


        [Theory]
        [InlineData("api/booking/available", DataSeeder.HttpMethod.PUT, Role.ANONM)]
        [InlineData("api/booking/availablecarclasses", DataSeeder.HttpMethod.PUT, Role.ANONM)]
        [InlineData("api/booking/book", DataSeeder.HttpMethod.POST, Role.AUTHU)]
        [InlineData("api/booking/isChangePossible/0", DataSeeder.HttpMethod.PUT, Role.AUTHU)]
        [InlineData("api/booking/change/0", DataSeeder.HttpMethod.PUT, Role.AUTHU)]
        [InlineData("api/booking/cancel/0", DataSeeder.HttpMethod.PUT, Role.AUTHU)]
        [InlineData("api/booking/history", DataSeeder.HttpMethod.GET, Role.SGDIN)]
        [InlineData("api/booking/start/0", DataSeeder.HttpMethod.PUT, Role.AUTHU)]
        [InlineData("api/booking/finish/0", DataSeeder.HttpMethod.PUT, Role.AUTHU)]
        [InlineData("api/booking/getBill/0", DataSeeder.HttpMethod.GET, Role.SGDIN)]
        [InlineData("api/booking/bills", DataSeeder.HttpMethod.GET, Role.SGDIN)]
        [InlineData("api/auth/login", DataSeeder.HttpMethod.POST, Role.ANONM)]
        [InlineData("api/auth/change", DataSeeder.HttpMethod.PUT, Role.SGDIN)]
        [InlineData("api/auth/impersonate/0", DataSeeder.HttpMethod.GET, Role.EMPLE)]
        [InlineData("api/admin/get/0", DataSeeder.HttpMethod.GET, Role.ADMIN)]
        [InlineData("api/admin/getall", DataSeeder.HttpMethod.GET, Role.ADMIN)]
        [InlineData("api/admin/add", DataSeeder.HttpMethod.POST, Role.ADMIN)]
        [InlineData("api/admin/change/0", DataSeeder.HttpMethod.PUT, Role.ADMIN)]
        [InlineData("api/admin/delete/0", DataSeeder.HttpMethod.DELETE, Role.ADMIN)]
        [InlineData("api/carclassdatabase/get/0", DataSeeder.HttpMethod.GET, Role.ANONM)]
        [InlineData("api/carclassdatabase/getall", DataSeeder.HttpMethod.GET, Role.ANONM)]
        [InlineData("api/carclassdatabase/add", DataSeeder.HttpMethod.POST, Role.EMPLE)]
        [InlineData("api/carclassdatabase/change/0", DataSeeder.HttpMethod.PUT, Role.EMPLE)]
        [InlineData("api/cardatabase/get/0", DataSeeder.HttpMethod.GET, Role.EMPLE)]
        [InlineData("api/cardatabase/getall", DataSeeder.HttpMethod.GET, Role.EMPLE)]
        [InlineData("api/cardatabase/add", DataSeeder.HttpMethod.POST, Role.EMPLE)]
        [InlineData("api/cardatabase/change/0", DataSeeder.HttpMethod.PUT, Role.EMPLE)]
        [InlineData("api/cartypedatabase/get/0", DataSeeder.HttpMethod.GET, Role.ANONM)]
        [InlineData("api/cartypedatabase/getall", DataSeeder.HttpMethod.GET, Role.ANONM)]
        [InlineData("api/cartypedatabase/add", DataSeeder.HttpMethod.POST, Role.EMPLE)]
        [InlineData("api/cartypedatabase/change/0", DataSeeder.HttpMethod.PUT, Role.EMPLE)]
        [InlineData("api/plandatabase/get/0", DataSeeder.HttpMethod.GET, Role.ANONM)]
        [InlineData("api/plandatabase/getall", DataSeeder.HttpMethod.GET, Role.ANONM)]
        [InlineData("api/plandatabase/add", DataSeeder.HttpMethod.POST, Role.EMPLE)]
        [InlineData("api/plandatabase/change/0", DataSeeder.HttpMethod.PUT, Role.EMPLE)]
        [InlineData("api/ping", DataSeeder.HttpMethod.GET, Role.ANONM)]
        [InlineData("api/car/car/0", DataSeeder.HttpMethod.GET, Role.AUTHU)]
        [InlineData("api/car/lock/0", DataSeeder.HttpMethod.PUT, Role.AUTHU)]
        [InlineData("api/car/unlock/0", DataSeeder.HttpMethod.PUT, Role.AUTHU)]
        [InlineData("api/car/km/0/", DataSeeder.HttpMethod.GET, Role.EMPLE)]
        [InlineData("api/car/booking/0", DataSeeder.HttpMethod.GET, Role.EMPLE)]
        [InlineData("api/user/update", DataSeeder.HttpMethod.PUT, Role.SGDIN)]
        [InlineData("api/user/register", DataSeeder.HttpMethod.PUT, Role.ANONM)]
        [InlineData("api/user/getnewcard", DataSeeder.HttpMethod.GET, Role.SGDIN)]
        [InlineData("api/user/current", DataSeeder.HttpMethod.GET, Role.SGDIN)]
        [InlineData("api/usermanagement/getuserlist", DataSeeder.HttpMethod.GET, Role.EMPLE)]
        [InlineData("api/usermanagement/setuserstatus", DataSeeder.HttpMethod.PATCH, Role.EMPLE)]
        public async Task TestAccessAsync(string URL, DataSeeder.HttpMethod method, Role role)
        {
            foreach (Role r in Enum.GetValues(typeof(Role)))
            {
                var status = await GetStatusCode(URL, method, r);
                if (r < role)
                    if (r == Role.ANONM)
                        Assert.Equal(HttpStatusCode.Unauthorized, status);
                    else
                        Assert.Equal(HttpStatusCode.Forbidden, status);
                else
                {
                    Assert.NotEqual(HttpStatusCode.Unauthorized, status);
                    //Assert.NotEqual(HttpStatusCode.Forbidden, status);
                }
            }
        }

        public async Task<HttpStatusCode> GetStatusCode(string URL, DataSeeder.HttpMethod method, Role role)
        {
            HttpClient client = _factory.CreateClient();

            if (role != Role.ANONM)
            {
                string loginUrl = "/api/auth/login";
                var loginBody = new LoginRequest(RolePerson[role].Email, RolePerson[role].Firstname + "123").ConvertToStringBody();
                var loginResponse = await client.PostAsync(loginUrl,loginBody);
                LoginDTO login = await loginResponse.ConvertToTypeFromStringAsync<LoginDTO>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);
            }
            var body = "".ConvertToStringBody();
            HttpResponseMessage? response = method switch
            {
                DataSeeder.HttpMethod.GET => await client.GetAsync(URL),
                DataSeeder.HttpMethod.POST => await client.PostAsync(URL, body),
                DataSeeder.HttpMethod.PUT => await client.PutAsync(URL, body),
                DataSeeder.HttpMethod.DELETE => await client.DeleteAsync(URL),
                DataSeeder.HttpMethod.PATCH => await client.PatchAsync(URL, body),
                _ => null,
            };
            return response.StatusCode;
        }


        [Theory]
        [InlineData(0)]
        public async Task Login_Success(int i)
        {
            string url = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            PersonDTO person = new(DataSeeder.people[i]);

            var body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();

            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);

            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();

            // is user
            Assert.Equal(DataSeeder.users.Any(u => u.Person == DataSeeder.people[i]), loginDTO.IsUser);

            // Last Login fewer than 2 Minutes ago
            Assert.NotEqual(person.LastLogin, loginDTO.Person.LastLogin);
            Assert.True((int)DateTime.UtcNow.Subtract(DateTimeOffset.FromUnixTimeMilliseconds(loginDTO.Person.LastLogin).UtcDateTime).TotalMinutes < 2);
            person.LastLogin = loginDTO.Person.LastLogin;

            // rest is equal
            Assert.Equal(person.ToJson(), loginDTO.Person.ToJson());
        }

        [Fact]
        public async Task Login_WrongMail()
        {
            string url = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.Unauthorized;

            var body = new LoginRequest("email@mail", "password").ConvertToStringBody();

            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task Login_WrongPW()
        {
            string url = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.Unauthorized;

            var body = new LoginRequest(DataSeeder.people[0].Email, DataSeeder.people[0].Firstname).ConvertToStringBody();

            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task Login_NonMail()
        {
            string url = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.BadRequest;

            var body = new LoginRequest("email", "password").ConvertToStringBody();

            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task Login_Empty()
        {
            string url = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.BadRequest;

            var body = new LoginRequest("", "").ConvertToStringBody();

            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);
        }

        // assumes Loin works!
        [Fact]
        public async Task ChangePassword()
        {
            int personID = 1;

            string url = "/api/auth/change";
            string urlLogin = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            string newPW = "123";
            string oldPW = DataSeeder.people[personID].Firstname + "123";
            string[] passwords = { oldPW, newPW };

            var body = new LoginRequest(DataSeeder.people[personID].Email, oldPW).ConvertToStringBody();

            // login
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedStatus, response.StatusCode);

            // set token
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // change PW
            response = await client.PutAsync(url, passwords.ConvertToStringBody());
            Assert.Equal(expectedStatus, response.StatusCode);

            // login with new PW
            body = new LoginRequest(DataSeeder.people[personID].Email, newPW).ConvertToStringBody();
            response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedStatus, response.StatusCode);

            // set token
            loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // change PW back
            Array.Reverse(passwords);
            response = await client.PutAsync(url, passwords.ConvertToStringBody());
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_Empty()
        {
            int personID = 1;

            string url = "/api/auth/change";
            string urlLogin = "/api/auth/login";
            HttpStatusCode expectedLoginStatus = HttpStatusCode.OK;
            HttpStatusCode expectedStatus = HttpStatusCode.BadRequest;

            // empty password
            string newPW = "";
            string oldPW = DataSeeder.people[personID].Firstname + "123";
            string[] passwords = { newPW, oldPW };

            var body = new LoginRequest(DataSeeder.people[personID].Email, oldPW).ConvertToStringBody();

            // login
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedLoginStatus, response.StatusCode);

            // set token
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // change PW
            response = await client.PutAsync(url, passwords.ConvertToStringBody());
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_WrongPW()
        {
            int personID = 1;

            string url = "/api/auth/change";
            string urlLogin = "/api/auth/login";
            HttpStatusCode expectedLoginStatus = HttpStatusCode.OK;
            HttpStatusCode expectedStatus = HttpStatusCode.Unauthorized;

            // empty password
            string newPW = "123";
            string oldPW = DataSeeder.people[personID].Firstname + "123";
            string[] passwords = { newPW, newPW };

            var body = new LoginRequest(DataSeeder.people[personID].Email, oldPW).ConvertToStringBody();

            // login
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedLoginStatus, response.StatusCode);

            // set token
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            // change PW
            response = await client.PutAsync(url, passwords.ConvertToStringBody());
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task Impersonate()
        {

            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            var client = _factory.CreateClient();
            string loginUrl = "/api/auth/login";
            var loginBody = new LoginRequest(RolePerson[Role.AUTHU].Email, RolePerson[Role.AUTHU].Firstname + "123").ConvertToStringBody();
            var loginResponse = await client.PostAsync(loginUrl, loginBody);
            Assert.Equal(expectedStatus, loginResponse.StatusCode);
            PersonDTO person = (await loginResponse.ConvertToTypeFromStringAsync<LoginDTO>()).Person;

            var dbTest = new DatabaseTest(_factory);

            string url = $"api/auth/impersonate/{person.Id}";
            LoginDTO loginDTO = await dbTest.DatabaseGet<LoginDTO>(url, Role.EMPLE);

            Assert.Equal(person.ToJson(), loginDTO.Person.ToJson());
            Assert.True(loginDTO.IsUser);
        }

        [Fact]
        public async Task Impersonate_Forbid()
        {
            int personID = 4;
            int newUserID = 1;
            string url = "/api/auth/impersonate/";
            string urlLogin = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.Forbidden;
            HttpStatusCode expectedLoginStatus = HttpStatusCode.OK;
            PersonDTO person = new(DataSeeder.people[newUserID - 1]);
            person.Id = newUserID;

            string PW = DataSeeder.people[personID].Firstname + "123";

            var body = new LoginRequest(DataSeeder.people[personID].Email, PW).ConvertToStringBody();

            // login
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedLoginStatus, response.StatusCode);

            // set token
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            response = await client.GetAsync(url + newUserID.ToString());
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task Impersonate_Employee()
        {
            int personID = 1;
            int newUserID = 2;
            string url = "/api/auth/impersonate/";
            string urlLogin = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.Forbidden;
            HttpStatusCode expectedLoginStatus = HttpStatusCode.OK;
            PersonDTO person = new(DataSeeder.people[newUserID - 1]);
            person.Id = newUserID;

            string PW = DataSeeder.people[personID].Firstname + "123";

            var body = new LoginRequest(DataSeeder.people[personID].Email, PW).ConvertToStringBody();

            // login
            var client = _factory.CreateClient();
            var response = await client.PostAsync(urlLogin, body);
            Assert.Equal(expectedLoginStatus, response.StatusCode);

            // set token
            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            response = await client.GetAsync(url + newUserID.ToString());
            Assert.Equal(expectedStatus, response.StatusCode);
        }
    }
}
