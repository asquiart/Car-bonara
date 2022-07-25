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
    public class StartBookingTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
        public StartBookingTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task StartBookingOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {           
            string url = "/api/auth/login";
            string urlstart = "api/booking/start/2";
            string urlget = "api/booking/get/2";

            //string urlget = "api/booking/get/5";

            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            PersonDTO person = new(DataSeeder.people[5]);

            var body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();

            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);

            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            User user = DataSeeder.users[2];
            


            // is user
            Assert.Equal(DataSeeder.users.Any(u => u.Person == DataSeeder.people[5]), loginDTO.IsUser);

            response = await client.PutAsync(urlstart, null);

            Assert.Equal(expectedStatus, response.StatusCode);

            CarDTO car = await response.ConvertToTypeFromStringAsync<CarDTO>();

            Assert.NotNull(car);

            //Assert.Single(history);

            Assert.Equal(expectedStatus, response.StatusCode);




            // braucht eine fertige buchung
            /*response = await client.GetAsync(urlget);

            Assert.Equal(expectedStatus, response.StatusCode);

            BookingDTOOut booking = await response.ConvertToTypeFromStringAsync<BookingDTOOut>();

            Assert.NotNull(booking);
            Assert.Equal(5, booking.Id);*/




        }
    }
}