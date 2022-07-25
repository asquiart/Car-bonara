using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CarbonaraWebAPI;
using CarbonaraWebAPI.Controllers;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPITest.Util;
using Xunit;
using static CarbonaraWebAPITest.Util.DataSeeder;

namespace CarbonaraWebAPITest
{
    [Collection("Integration Tests")]
    public class BillTest :  IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
      
        public BillTest(CustomWebApplicationFactory<Startup> factory)
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
        public async Task GetBillErrorNoBillForBooking()
        {

            var client = CreateClient(Role.SGDIN2).Result;
            var booking = DataSeeder.bookings[2].Id;


            var billResponse = await client.GetAsync("api/booking/getBill/" + booking.ToString());

            Assert.Equal(HttpStatusCode.OK, billResponse.StatusCode);
            //Empty 
            Assert.Equal("{\"Headers\":[]}" , billResponse.Content.ToJson());


        }


        [Fact]
        public async Task GetBillWithAllPositionsAfterStartingToLate()
        {

            var client = CreateClient(Role.SGDIN2).Result;
            var booking = DataSeeder.bookings[8];


            var finishResponse = await client.PutAsync("api/booking/start/" + booking.Id.ToString(), null);


            Assert.Equal(HttpStatusCode.BadRequest, finishResponse.StatusCode);


            var billResponse = await client.GetAsync("api/booking/getBill/" + booking.Id.ToString());

            BillDTO bill = await billResponse.ConvertToTypeFromStringAsync<BillDTO>();
           
            Assert.Equal(HttpStatusCode.OK, billResponse.StatusCode);

            Assert.NotNull(bill);
            
            Assert.Equal(3 , bill.Positions.Length);

        }


        [Fact]
        public async Task GetBillAfterEndingBooking()
        {

            var client = CreateClient(Role.SGDIN2).Result;
            var booking = DataSeeder.bookings[7].Id;

            var finishResponse = await client.PutAsync("api/booking/finish/" + booking.ToString(), null);

           
            Assert.Equal(HttpStatusCode.OK, finishResponse.StatusCode);


            var billResponse = await client.GetAsync("api/booking/getBill/" + booking.ToString());

            BillDTO bill = await billResponse.ConvertToTypeFromStringAsync<BillDTO>();
            Assert.Equal(HttpStatusCode.OK, billResponse.StatusCode);
            bool isToLate = false;
            Assert.NotNull(bill);
            foreach(BillPositionDTO position in bill.Positions)
            {
                if (position.Item == "Stunden (Überzogen)")
                    isToLate = true;
            }
            Assert.True(isToLate);         

        }

        [Fact]
        public async Task GetEmptyBills()
        {

                var client = CreateClient(Role.SGDIN).Result;
                string url = "api/booking/bills"; 
            var response = await client.GetAsync(url);
         
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                IEnumerable<BillDTO> bills = await response.ConvertToTypeFromStringAsync<IEnumerable<BillDTO>>();        
                Assert.NotNull(bills);
                Assert.Equal(0, bills.Count());

        }


        }

    
}
