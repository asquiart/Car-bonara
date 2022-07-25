using CarbonaraWebAPI;
using CarbonaraWebAPI.Controllers;
using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Services;
using CarbonaraWebAPITest.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;
using DataSeeder = CarbonaraWebAPITest.Util.DataSeeder;

namespace CarbonaraWebAPITest
{
    [Collection("Integration Tests")]
    public class FinishBookingAndGetBookingtest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
        public FinishBookingAndGetBookingtest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task FinishBookingAndGetBookingHistoryAllStatus()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {
            string url = "/api/auth/login";
            string urlstart = "api/booking/start/2";
            string urlfinish = "api/booking/finish/2";
            string urlfinishBadRequest = "api/booking/finish/99";

            string urlhistory = "api/booking/history";

            //string urlget = "api/booking/get/5";

            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            HttpStatusCode expectedStatusBadRequest = HttpStatusCode.BadRequest;
            HttpStatusCode expectedStatusBadForbidden = HttpStatusCode.Forbidden;

            var client = _factory.CreateClient();


            //login user without example bookings
            PersonDTO person = new(DataSeeder.people[3]);

            var body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();

            
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);

            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            User user = DataSeeder.users[0];



            // is user
            Assert.Equal(DataSeeder.users.Any(u => u.Person == user.Person), loginDTO.IsUser);


            


            // booking finish forbidden
            response = await client.PutAsync(urlfinish, null);
            Assert.Equal(expectedStatusBadForbidden, response.StatusCode);


            //login user with example bookings
            person = new(DataSeeder.people[5]);

            body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();

            client = _factory.CreateClient();
            response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);

            loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            user = DataSeeder.users[2];



            // is user
            Assert.Equal(DataSeeder.users.Any(u => u.Person == DataSeeder.people[5]), loginDTO.IsUser);


      


            // start booking
            response = await client.PutAsync(urlstart, null);

            Assert.Equal(expectedStatus, response.StatusCode);

            CarDTO car = await response.ConvertToTypeFromStringAsync<CarDTO>();

            Assert.NotNull(car);

            //Assert.Single(history);


            // booking finish badrequest
            response = await client.PutAsync(urlfinishBadRequest, null);
            Assert.Equal(expectedStatusBadRequest, response.StatusCode);



            // booking finish ok
            response = await client.PutAsync(urlfinish, null);

            Assert.Equal(expectedStatus, response.StatusCode);


           
            


            // booking history ok
            response = await client.GetAsync(urlhistory);
            Assert.Equal(expectedStatus, response.StatusCode);
            var bookingHistory = await response.ConvertToTypeFromStringAsync<IEnumerable<BookingDTOOut>>();

            Assert.NotNull(bookingHistory);

        }

    }
}