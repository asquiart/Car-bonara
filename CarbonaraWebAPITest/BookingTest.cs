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
    public class BookingTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
        public BookingTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAvailableCarClassesOk()
        {
           
                CarclassDTO carclassDTO = new CarclassDTO
                {
                    Id = 0,
                    Name = "PLACEHOLDER",
                    PriceFaktor = 1
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
                Id = 1,
                Name = "Sesamstraße",
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
                    Capacity = 5
                };



                BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {
                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(3) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                HttpStatusCode expectedStatus = HttpStatusCode.OK;

                string urlAviable = "api/booking/availableCarclasses";

                var client = _factory.CreateClient();
                var body = aviableCarclassesDTO.ConvertToStringBody();

                var response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                IEnumerable<CarclassDTO> carclasses = await response.ConvertToTypeFromStringAsync<IEnumerable<CarclassDTO>>();

                Assert.NotNull(carclasses);

                Assert.Single(carclasses);

            CarclassDTO expectedClassCar = new CarclassDTO
            {
                Id = 1,
                Name = "Kleinwagen",
                PriceFaktor = 1f,
            };

            Assert.Equal(expectedClassCar.ToJson(), carclasses.First().ToJson());
        }

        //CalculatePrice
        [Fact]
        public async Task IsBookingAvailableAllowAnonymousOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {                                                           //2 volle tage, 5 stunden in der nacht, 4 stunden am tag
            CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
            {
                Id = 1,
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
                Id = 1,
                Name = "Sesamstraße",
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
                    Capacity = 5
                };



                BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {

                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(3) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                //20, 1.7, 0.5, 0.24, 30, 10

                double expectedPrice = 20 * 2 + 4 * 1.7 + 0.5 * 5;
                expectedPrice = Math.Round(expectedPrice * carclassDTO.PriceFaktor, 2);



                HttpStatusCode expectedStatus = HttpStatusCode.OK;

                string urlAviable = "api/booking/available";

                var client = _factory.CreateClient();
                var body = aviableCarclassesDTO.ConvertToStringBody();

                var response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
                isBookingAvailable = Math.Round(isBookingAvailable, 2);


                Assert.Equal(expectedPrice, isBookingAvailable);
            
        }

        [Fact]
        public async Task IsBookingAvailableAuthorizedOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {                                                           //2 volle tage, 5 stunden in der nacht, 4 stunden am tag
            CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
            {
                Id = 1,
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
                Id = 1,
                Name = "Sesamstraße",
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
                    Capacity = 5
                };



                BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {

                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                string urlAviable = "api/booking/available";
                string url = "/api/auth/login";
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


                //20, 1.7, 0.5, 0.24, 30, 10

                double expectedPrice = user.Plan.PriceWholeDay * 1 + 4 * user.Plan.PriceHourDay + user.Plan.PriceHourNight * 5;

                expectedPrice = Math.Round(expectedPrice * carclassDTO.PriceFaktor, 2);





                //varclient = _factory.CreateClient();
                body = aviableCarclassesDTO.ConvertToStringBody();

                response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
                isBookingAvailable = Math.Round(isBookingAvailable, 2);


                Assert.Equal(expectedPrice, isBookingAvailable);
            
        }

        [Fact]
        public async Task AutherizedAviablePriceDayNotAviable()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {
            //2 volle tage, 5 stunden in der nacht, 4 stunden am tag
                CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
                {
                    Id = 2,
                    Name = "Mittelklasse",
                    PriceFaktor = 1.3f,
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
                    Id = 3,
                    Name = "Hochschule Bremen",
                    Address = startStationAdress,
                    Capacity = 1
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
                    Capacity = 5
                };



                BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {

                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                string urlAviable = "api/booking/available";
                string url = "/api/auth/login";
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



                double expectedPrice = -1;

                body = aviableCarclassesDTO.ConvertToStringBody();

                response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
                isBookingAvailable = Math.Round(isBookingAvailable, 2);


                Assert.Equal(expectedPrice, isBookingAvailable);

            
        }

        [Fact]
        public async Task AutherizedAviablePriceDayNotAviable2()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {                                                           //2 volle tage, 5 stunden in der nacht, 4 stunden am tag
            CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
            {
                Id = 2,
                Name = "Mittelklasse",
                PriceFaktor = 1.3f,
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
                Id = 3,
                Name = "Hochschule Bremen",
                Address = startStationAdress,
                Capacity = 1
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
                Capacity = 5
            };



            BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
            {

                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(5) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                string urlAviable = "api/booking/available";
                string url = "/api/auth/login";
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



                double expectedPrice = -1;

                body = aviableCarclassesDTO.ConvertToStringBody();

                response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
                isBookingAvailable = Math.Round(isBookingAvailable, 2);


                Assert.Equal(expectedPrice, isBookingAvailable);
            
        }


        [Fact]
        public async Task MakeBookingOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {
            //2 volle tage, 5 stunden in der nacht, 4 stunden am tag



            string url = "/api/auth/login";
            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            PersonDTO person = new(DataSeeder.people[5]);
            var body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();
            var client = _factory.CreateClient();
            var response = await client.PostAsync(url, body);






            CarclassDTO carclassDTO = new CarclassDTO(DataSeeder.carclasses[1]);
            StationDTO startStationDTO = new StationDTO(DataSeeder.stations[2]);
            StationDTO endStationDTO = new StationDTO(DataSeeder.stations[1]);

            BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {

                StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                Carclass = carclassDTO,
                Startstation = startStationDTO,
                Endstation = endStationDTO,
            };

                string urlAviable = "api/booking/available";
              
                string urlbook = "api/booking/book";
                string urlcancel = "api/booking/cancel/12";



                Assert.Equal(expectedStatus, response.StatusCode);

                LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

                User user = DataSeeder.users[2];

                // is user
                Assert.Equal(DataSeeder.users.Any(u => u.Person == DataSeeder.people[5]), loginDTO.IsUser);


                //20, 1.7, 0.5, 0.24, 30, 10

                double expectedPrice = user.Plan.PriceWholeDay * 1 + 4 * user.Plan.PriceHourDay + user.Plan.PriceHourNight * 5;

                expectedPrice = Math.Round(expectedPrice * carclassDTO.PriceFaktor, 2);





                //varclient = _factory.CreateClient();
                body = aviableCarclassesDTO.ConvertToStringBody();

                response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
                isBookingAvailable = Math.Round(isBookingAvailable, 2);

                Assert.Equal(expectedPrice, isBookingAvailable);

                body = aviableCarclassesDTO.ConvertToStringBody();


                //booking 
                response = await client.PostAsync(urlbook, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                //cancel booking
                response = await client.PutAsync(urlcancel, null);
                Assert.Equal(expectedStatus, response.StatusCode);

            
        }

        [Fact]
        public async Task IsChangeBookingAllowedOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {                                                           //2 volle tage, 5 stunden in der nacht, 4 stunden am tag
            CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
            {
                Id = 1,
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
                Id = 1,
                Name = "Sesamstraße",
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
                    Capacity = 5
                };



                BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {
                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                aviableCarclassesDTO.Id = 5;        // herausfinden welche datenbank verwendet wird

                string urlAviable = "api/booking/available";
                string url = "/api/auth/login";
                string urlbook = "api/booking/book";
                string urlbookchangepossible = "api/booking/isChangePossible/12";        //mit 0 kann man badrequest herbeirufen

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


                //20, 1.7, 0.5, 0.24, 30, 10

                double expectedPrice = user.Plan.PriceWholeDay * 1 + 4 * user.Plan.PriceHourDay + user.Plan.PriceHourNight * 5;

                expectedPrice = Math.Round(expectedPrice * carclassDTO.PriceFaktor, 2);





                //varclient = _factory.CreateClient();
                body = aviableCarclassesDTO.ConvertToStringBody();

                response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
                isBookingAvailable = Math.Round(isBookingAvailable, 2);

                Assert.Equal(expectedPrice, isBookingAvailable);

                body = aviableCarclassesDTO.ConvertToStringBody();


                //booking test
                response = await client.PostAsync(urlbook, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                //
                response = await client.PutAsync(urlbookchangepossible, body);

                Assert.Equal(expectedStatus, response.StatusCode);

                bool isBookingChangePossible = await response.ConvertToTypeFromStringAsync<bool>();

                Assert.True(isBookingChangePossible);
            
        }

        [Fact]
        public async Task IsChangeBookingAllowedBadRequest()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {                                                           //2 volle tage, 5 stunden in der nacht, 4 stunden am tag
            CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
            {
                Id = 3,
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
                    Id = 3,
                    Name = "Hochschule Bremen",
                    Address = startStationAdress,
                    Capacity = 1
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
                    Capacity = 5
                };



                BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {
                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                aviableCarclassesDTO.Id = 5;        // herausfinden welche datenbank verwendet wird

                string urlAviable = "api/booking/available";
                string url = "/api/auth/login";
                string urlbook = "api/booking/book";
                string urlbookchangepossible = "api/booking/isChangePossible/0";        //mit 0 kann man badrequest herbeirufen

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


        
            response = await client.PutAsync(urlbookchangepossible, body);

            Assert.Equal(expectedStatusBadRequest, response.StatusCode);

        }

        [Fact]
        public async Task IsChangeBookingPossibleTestForbidden()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
        {
            //2 volle tage, 5 stunden in der nacht, 4 stunden am tag

           

                CarclassDTO carclassDTO = new CarclassDTO               //mit den billigeren plan bei anon berechnen
                {
                    Id = 2,
                    Name = "Mittelklasse",
                    PriceFaktor = 1.3f,
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
                    Id = 3,
                    Name = "Hochschule Bremen",
                    Address = startStationAdress,
                    Capacity = 1
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
                    Capacity = 5
                };



                BookingDTOIn aviableCarclassesDTO = new BookingDTOIn
                {
                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

            aviableCarclassesDTO.Id = 5;        // herausfinden welche datenbank verwendet wird
            var client = _factory.CreateClient();
            //string urlAviable = "api/booking/available";
            string url = "/api/auth/login";
            //string urlbook = "api/booking/book";
            string urlbookchangepossible = "api/booking/isChangePossible/7";        //mit 0 kann man badrequest herbeirufen

            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            HttpStatusCode expectedStatusForbidden = HttpStatusCode.Forbidden;

            PersonDTO person = new(DataSeeder.people[5]);

                var body = new LoginRequest(person.Email, person.Firstname + "123").ConvertToStringBody();

            
            var response = await client.PostAsync(url, body);

                Assert.Equal(expectedStatus, response.StatusCode);

                LoginDTO loginDTO = await response.ConvertToTypeFromStringAsync<LoginDTO>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDTO.Token);

                User user = DataSeeder.users[2];

                // is user
                Assert.Equal(DataSeeder.users.Any(u => u.Person == DataSeeder.people[5]), loginDTO.IsUser);


                //20, 1.7, 0.5, 0.24, 30, 10

                double expectedPrice = user.Plan.PriceWholeDay * 1 + 4 * user.Plan.PriceHourDay + user.Plan.PriceHourNight * 5;

                expectedPrice = Math.Round(expectedPrice * carclassDTO.PriceFaktor, 2);





            //varclient = _factory.CreateClient();
            /*body = aviableCarclassesDTO.ConvertToStringBody();

                response = await client.PutAsync(urlAviable, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                double isBookingAvailable = await response.ConvertToTypeFromStringAsync<float>();
                isBookingAvailable = Math.Round(isBookingAvailable, 2);

            Assert.Equal(expectedPrice, isBookingAvailable);*/

            


            //booking test
            //response = await client.PostAsync(urlbook, body);
            //Assert.Equal(expectedStatus, response.StatusCode);

            //
            body = aviableCarclassesDTO.ConvertToStringBody();

            response = await client.PutAsync(urlbookchangepossible, body);

            Assert.Equal(expectedStatusForbidden, response.StatusCode);

         

        }

        [Fact]
        public async Task ChangeBookingOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
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
                Id = 1,
                Name = "Sesamstraße",
                Address = endStationAdress,
                Capacity = 5
            };



                BookingDTOIn oldBookingDTOIn = new BookingDTOIn
                {
                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(1) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                oldBookingDTOIn.Id = 12;        // herausfinden welche datenbank verwendet wird

            string urlAviable = "api/booking/available";
            string url = "/api/auth/login";
            string urlbook = "api/booking/book";
            string urlbookchangepossible = "api/booking/isChangePossible/12";        //mit 0 kann man badrequest herbeirufen
            string urlchangebooking = "api/booking/change/12";

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


                //booking test
                response = await client.PostAsync(urlbook, body);
                Assert.Equal(expectedStatus, response.StatusCode);

                //
                response = await client.PutAsync(urlbookchangepossible, body);

                Assert.Equal(expectedStatus, response.StatusCode);

                bool isBookingChangePossible = await response.ConvertToTypeFromStringAsync<bool>();

                Assert.True(isBookingChangePossible);

                BookingDTOIn newBookingDTOIn = new BookingDTOIn
                {
                    StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(3) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                    EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(9) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                    Carclass = carclassDTO,
                    Startstation = startStationDTO,
                    Endstation = endStationDTO,
                };

                //newBookingDTOIn.Id = 5;


                body = newBookingDTOIn.ConvertToStringBody();

                response = await client.PutAsync(urlchangebooking, body);
                Assert.Equal(expectedStatus, response.StatusCode); 
        }


        //[Fact]
        public async Task CancelBookingOk()        //bestimmten tag 9:00 uhr aufhöhren 2 tage später 9:00 uhr, -> einen vollen tag in der Mitte
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


            //booking test
            response = await client.PostAsync(urlbook, body);
            Assert.Equal(expectedStatus, response.StatusCode);

            //
            //response = await client.PutAsync(urlbookchangepossible, body);

            //Assert.Equal(expectedStatus, response.StatusCode);

            //bool isBookingChangePossible = await response.ConvertToTypeFromStringAsync<bool>();

            //Assert.True(isBookingChangePossible);

            /*BookingDTOIn newBookingDTOIn = new BookingDTOIn
            {
                StartTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(2) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(),
                EndTime = new DateTimeOffset((DateTime.UtcNow.Date.AddDays(3) + new TimeSpan(9, 0, 0))).ToUnixTimeMilliseconds(), //=> 1 whole day deckt mehr branches in bill ab
                Carclass = carclassDTO,
                Startstation = startStationDTO,
                Endstation = endStationDTO,
            };

            //newBookingDTOIn.Id = 5;


            body = newBookingDTOIn.ConvertToStringBody();*/

            response = await client.PutAsync(urlcancelbooking, null);
            Assert.Equal(expectedStatus, response.StatusCode);
        }
    }
}