using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CarbonaraWebAPI;
using CarbonaraWebAPI.Controllers;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPITest.Util;
using Xunit;
using static CarbonaraWebAPITest.Util.DataSeeder;

namespace CarbonaraWebAPITest
{
    [Collection("Integration Tests")]
    public class CarControllerTests :  IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
      
        public CarControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;

        }

        public async Task<string> login(Role role)
        {
            HttpStatusCode expectedStatus = HttpStatusCode.OK;
            var client = _factory.CreateClient();
            string loginUrl = "/api/auth/login";
            var loginBody = new LoginRequest(DataSeeder.RolePerson[role].Email, DataSeeder.RolePerson[role].Firstname + "123").ConvertToStringBody();
            var loginResponse = await client.PostAsync(loginUrl, loginBody);
            Assert.Equal(expectedStatus, loginResponse.StatusCode);
            LoginDTO login = await loginResponse.ConvertToTypeFromStringAsync<LoginDTO>();
            return login.Token;
        }


        public async Task< HttpClient> CreateClient(Role role)
        {
            var client = _factory.CreateClient();
            if (role != Role.ANONM)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await login(role));
            return client;
        }


        [Fact]
        public async Task GetCarOfBooking()
        {

        
            var clientUser = CreateClient(Role.SGDIN2).Result;
            Booking booking = DataSeeder.bookings[9];


            //Car is Null Before Booking
            var noCarResponse = await clientUser.GetAsync("api/car/car/" + booking.Id.ToString());
            Assert.Equal(HttpStatusCode.BadRequest, noCarResponse.StatusCode);


            //Start Booking
            var startBookingResponse = await clientUser.PutAsync("api/booking/start/" + booking.Id.ToString(), null);
            Assert.Equal(HttpStatusCode.OK, startBookingResponse.StatusCode);


            //Unlock car
            var unlockResponse = await clientUser.PutAsync("api/car/unlock/" + booking.Id.ToString(), null);
            Assert.Equal(HttpStatusCode.OK, startBookingResponse.StatusCode);
         

            //Get Car of Booking
            var getCarResponse = await clientUser.GetAsync("api/car/car/" + booking.Id.ToString());
            Assert.Equal(HttpStatusCode.OK, startBookingResponse.StatusCode);

            CarDTO car =  await getCarResponse.ConvertToTypeFromStringAsync<CarDTO>();
            Assert.Equal(booking.Carclass.Id, car.Type.Carclass.Id);
            Assert.Equal(car.LockStatus, Car.CarLockStatus.unlocked);



    


            //Lock car
            var lockResponse = await clientUser.PutAsync("api/car/lock/" + booking.Id.ToString() , null);
            Assert.Equal(HttpStatusCode.OK, startBookingResponse.StatusCode);


            //GetCarAgain to check lock
            getCarResponse = await clientUser.GetAsync("api/car/car/" + booking.Id.ToString());
            Assert.Equal(HttpStatusCode.OK, startBookingResponse.StatusCode);

            car = await getCarResponse.ConvertToTypeFromStringAsync<CarDTO>();
            Assert.Equal(car.LockStatus, Car.CarLockStatus.locked);



            //Employye gets Booking by Car.id
            var clientEmployee = CreateClient(Role.EMPLE).Result;
            var getBookingOfCarResponse = await clientEmployee.GetAsync("api/car/booking/" + car.Id.ToString());
            Assert.Equal(HttpStatusCode.OK, getBookingOfCarResponse.StatusCode);
            BookingDTOOut bookingFromCar = await getCarResponse.ConvertToTypeFromStringAsync<BookingDTOOut>();
            Assert.NotNull(bookingFromCar);


            //Check Millage
            int carMillage = car.KilometersDriven;
            var getMillageResponse = await clientEmployee.GetAsync("api/car/km/" + car.Id.ToString());
            Assert.Equal(HttpStatusCode.OK, getBookingOfCarResponse.StatusCode);
            int newMillage = await getMillageResponse.ConvertToTypeFromStringAsync<int>();
            Assert.Equal(carMillage + 1, newMillage);


            //Check Fuel
            var getFuelResponse = await clientEmployee.GetAsync("api/car/fuel/" + car.Id.ToString());
            Assert.Equal(HttpStatusCode.OK, getBookingOfCarResponse.StatusCode);
            double fuel = await getMillageResponse.ConvertToTypeFromStringAsync<int>();
            Assert.True(fuel >= 0);

        }
       



    }

    
}
