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
    public class CancelBookingTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
        public CancelBookingTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task CancelBookingForbiddenAndOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {                                                           //2 volle tage, 5 stunden in der nacht, 4 stunden am tag
            CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
            {
                Id = 1, //1
                Name = "Kleinwagen",
                PriceFaktor = 1f,
            };

            AddressDTO startStationAdress = new AddressDTO
            {
                Id = 3,
                Number = "30",
                ZIP = "28199",
                Country = "Deutschland",
                City = "Bremen",
                Street = "Neustadtswall "
            };

            StationDTO startStationDTO = new StationDTO
            {
                Id = 2,
                Name = "Hauptbahnhof",
                Address = startStationAdress,
                Capacity = 3
            };

            AddressDTO endStationAdress = new AddressDTO
            {
                Id = 3,
                Number = "15",
                ZIP = "28195",
                Country = "Deutschland",
                City = "Bremen",
                Street = "Bahnhofsplatz "
            };

            StationDTO endStationDTO = new StationDTO
            {
                Id = 2,
                Name = "Hauptbahnhof",
                Address = endStationAdress,
                Capacity = 3
            };



            BookingDTOIn oldBookingDTOIn = new BookingDTOIn
            {
                StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                Carclass = carclassDTO,
                Startstation = startStationDTO,
                Endstation = endStationDTO,
            };

            oldBookingDTOIn.Id = 5;        // herausfinden welche datenbank verwendet wird

            string urlAviable = "api/booking/available";
            string url = "/api/auth/login";
            string urlbook = "api/booking/book";
            //string urlbookchangepossible = "api/booking/isChangePossible/5";        //mit 0 kann man badrequest herbeirufen
            string urlcancelbooking = "api/booking/cancel/12";
            string urlcancelbookingForbidden = "api/booking/cancel/1";


            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            HttpStatusCode expectedStatusForbidden = HttpStatusCode.Forbidden;

            PersonDTO person = new(DataSeeder.people[3]);

            var body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();

            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);

            Assert.Equal(expectedStatus, response.StatusCode);

            LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

            User user = DataSeeder.users[0];

            // is user
            Assert.Equal(DataSeeder.users.Any(u => u.Person == DataSeeder.people[3]), loginDTO.IsUser);


            //20, 1.7, 0.5, 0.24, 30, 10

            double expectedPrice = user.Plan.PriceWholeDay * 1 + 4 * user.Plan.PriceHourDay + user.Plan.PriceHourNight * 5;

            expectedPrice = Math.Round(expectedPrice * carclassDTO.PriceFaktor, 2);





            //varclient = _factory.CreateClient();
            body = oldBookingDTOIn.ConvertToStringBody();

            response = await client.PutAsync(urlAviable, body);
            Assert.Equal(expectedStatus, response.StatusCode);

            double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
            isBookingAvailable = Math.Round(isBookingAvailable, 2);

            Assert.Equal(expectedPrice, isBookingAvailable);

            body = oldBookingDTOIn.ConvertToStringBody();


            //booking
            response = await client.PostAsync(urlbook, body);
            Assert.Equal(expectedStatus, response.StatusCode);



            //forbid cancel booking
            response = await client.PutAsync(urlcancelbookingForbidden, null);
            Assert.Equal(expectedStatusForbidden, response.StatusCode);

            //cancel booking
            response = await client.PutAsync(urlcancelbooking, null);
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async Task CancelBookingBadRequest()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {           // herausfinden welche datenbank verwendet wird


            string url = "/api/auth/login";
            string urlbook = "api/booking/book";
            //string urlbookchangepossible = "api/booking/isChangePossible/5";        //mit 0 kann man badrequest herbeirufen
            string urlcancelbooking = "api/booking/cancel/12";

            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            HttpStatusCode expectedStatusBadRequest = HttpStatusCode.BadRequest;

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


            response = await client.PutAsync(urlcancelbooking, null);
            Assert.Equal(expectedStatusBadRequest, response.StatusCode);
        }
    }
}