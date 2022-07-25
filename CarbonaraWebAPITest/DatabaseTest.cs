using CarbonaraWebAPI;
using CarbonaraWebAPI.Controllers;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPITest.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;
using static CarbonaraWebAPITest.Util.DataSeeder;

namespace CarbonaraWebAPITest
{
    [Collection("Integration Tests")]
    public class DatabaseTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly CustomWebApplicationFactory<Startup> _factory;
        public DatabaseTest(CustomWebApplicationFactory<Startup> factory)
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



        public async Task<T> DatabaseGet<T>(string url, Role role)
        {
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            var client = _factory.CreateClient();
            if (role != Role.ANONM)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await login(role));
            var response = await client.GetAsync(url);
            Assert.Equal(expectedStatus, response.StatusCode);
            return await response.ConvertToTypeFromStringAsync<T>();
        }

        public async Task<T> DatabasePost<T>(string url, Role role, T addObj)
        {
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            var client = _factory.CreateClient();
            if (role != Role.ANONM)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await login(role));
            var response = await client.PostAsync(url, addObj.ConvertToStringBody());
            Assert.Equal(expectedStatus, response.StatusCode);
            return await response.ConvertToTypeFromStringAsync<T>();
        }

        public async Task<T> DatabasePut<T>(string url, Role role, T changeObj)
        {
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            var client = _factory.CreateClient();
            if (role != Role.ANONM)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await login(role));
            var response = await client.PutAsync(url, changeObj.ConvertToStringBody());
            Assert.Equal(expectedStatus, response.StatusCode);
            return await response.ConvertToTypeFromStringAsync<T>();
        }

        public async Task DatabaseDelete(string url, Role role)
        {
            HttpStatusCode expectedStatus = HttpStatusCode.OK;

            var client = _factory.CreateClient();
            if (role != Role.ANONM)
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await login(role));
            var response = await client.DeleteAsync(url);
            Assert.Equal(expectedStatus, response.StatusCode);
            //return await response.ConvertToTypeFromStringAsync<T>();
        }



        [Fact]
        public async Task PlanDatabaseGet()
        {
            int planID = 1;
            PlanDTO plan = await DatabaseGet<PlanDTO>($"api/PlanDatabase/get/{planID}", Role.ANONM);
            Assert.Equal(new PlanDTO(DataSeeder.plans[planID-1]).ToJson(), plan.ToJson());
        }

        [Fact]
        public async Task PlanDatabaseGetAll()
        {
            IEnumerable<PlanDTO> plans = (await DatabaseGet<IEnumerable<PlanDTO>>("api/PlanDatabase/getall", Role.ANONM)).OrderBy(p => p.Id);
            List<PlanDTO> seedPlans = new();
            foreach(Plan plan in DataSeeder.plans)
                seedPlans.Add(new PlanDTO(plan));
            Assert.Equal(seedPlans.ToJson(), plans.ToList().GetRange(0, DataSeeder.plans.Count).ToJson());
        }

        [Fact]
        public async Task PlanDatabaseAdd()
        {
            PlanDTO planDTO = new(0, "name", 1, 2, 3, 4, 5, 6);
            await DatabasePost($"api/PlanDatabase/add", Role.EMPLE, planDTO);
            IEnumerable<PlanDTO> planDTOs = (await DatabaseGet<IEnumerable<PlanDTO>>($"api/PlanDatabase/getall", Role.ANONM)).OrderBy(p => p.Id);
            Assert.NotEqual(planDTO.Id, planDTOs.Last().Id);
            planDTO.Id = planDTOs.Last().Id;
            Assert.Equal(planDTO.ToJson(), planDTOs.Last().ToJson());
        }

        [Fact]
        public async Task PlanDatabaseChange()
        {
            int planID = 1;
            PlanDTO planDTO = new(planID, "name", 1, 2, 3, 4, 5, 6);
            await DatabasePut($"api/PlanDatabase/change/{planID}", Role.EMPLE, planDTO);
            PlanDTO changedPlanDTO = await DatabaseGet<PlanDTO>($"api/PlanDatabase/get/{planID}", Role.ANONM);
            Assert.Equal(planDTO.ToJson(), changedPlanDTO.ToJson());
            PlanDTO oldPlanDTO = new PlanDTO(DataSeeder.plans[planID - 1]);
            oldPlanDTO.Id = planID;
            await DatabasePut($"api/PlanDatabase/change/{planID}", Role.EMPLE, oldPlanDTO);
        }



        [Fact]
        public async Task StationDatabaseGet()
        {
            int stationID = 1;
            StationDTO station = await DatabaseGet<StationDTO>($"api/StationDatabase/get/{stationID}", Role.ANONM);
            Assert.Equal(new StationDTO(DataSeeder.stations[stationID - 1]).ToJson(), station.ToJson());
        }

        [Fact]
        public async Task StationDatabaseGetAll()
        {
            IEnumerable<StationDTO> plans = (await DatabaseGet<IEnumerable<StationDTO>>("api/StationDatabase/getall", Role.ANONM)).OrderBy(s => s.Id);
            List<StationDTO> seedStations = new();
            foreach (Station station in DataSeeder.stations)
                seedStations.Add(new StationDTO(station));
            Assert.Equal(seedStations.ToJson(), plans.ToList().GetRange(0, DataSeeder.stations.Count).ToJson());
        }

        [Fact]
        public async Task StationDatabaseAdd()
        {
            StationDTO stationDTO = new(0, "name", new AddressDTO(DataSeeder.RandAddress()), 1);
            await DatabasePost($"api/StationDatabase/add", Role.EMPLE, stationDTO);
            IEnumerable<StationDTO> stationDTOs = (await DatabaseGet<IEnumerable<StationDTO>>($"api/StationDatabase/getall", Role.ANONM)).OrderBy(s => s.Id);
            Assert.NotEqual(stationDTO.Id, stationDTOs.Last().Id);
            stationDTO.Id = stationDTOs.Last().Id;
            Assert.NotEqual(stationDTO.Address.Id, stationDTOs.Last().Address.Id);
            stationDTO.Address.Id = stationDTOs.Last().Address.Id;
            Assert.Equal(stationDTO.ToJson(), stationDTOs.Last().ToJson());
        }


        [Fact]
        public async Task StationDatabaseChange()
        {
            int stationID = 1;
            StationDTO stationDTO = new(stationID, "name", new AddressDTO(DataSeeder.RandAddress()), 1);
            DatabasePut($"api/StationDatabase/change/{stationID}", Role.EMPLE, stationDTO).Wait();
            StationDTO changedStationDTO = await DatabaseGet<StationDTO>($"api/StationDatabase/get/{stationID}", Role.ANONM);
            stationDTO.Address.Id = changedStationDTO.Address.Id;
            Assert.Equal(stationDTO.ToJson(), changedStationDTO.ToJson());
            StationDTO oldPlanDTO = new StationDTO(DataSeeder.stations[stationID - 1]);
            oldPlanDTO.Id = stationID;
            await DatabasePut($"api/StationDatabase/change/{stationID}", Role.EMPLE, oldPlanDTO);
        }



        [Fact]
        public async Task CartypeDatabaseGet()
        {
            int cartypeID = 1;
            CartypeDTO cartype = await DatabaseGet<CartypeDTO>($"api/CartypeDatabase/get/{cartypeID}", Role.ANONM);
            Assert.Equal(new CartypeDTO(DataSeeder.cartypes[cartypeID - 1]).ToJson(), cartype.ToJson());
        }

        [Fact]
        public async Task CartypeDatabaseGetAll()
        {
            IEnumerable<CartypeDTO> cartypes = (await DatabaseGet<IEnumerable<CartypeDTO>>("api/CartypeDatabase/getall", Role.ANONM)).OrderBy(c => c.Id);
            List<CartypeDTO> seedCartypes = new();
            foreach (Cartype station in DataSeeder.cartypes)
                seedCartypes.Add(new CartypeDTO(station));
            Assert.Equal(seedCartypes.ToJson(), cartypes.ToList().GetRange(0, DataSeeder.cartypes.Count).ToJson());
        }

        [Fact]
        public async Task CartypeDatabaseAdd()
        {
            int carclassID = 1;
            CartypeDTO cartypeDTO = new(new Cartype("name", "manufacturer", "fueltype", DataSeeder.carclasses[carclassID - 1]));
            cartypeDTO.Carclass.Id = carclassID;
            await DatabasePost($"api/CartypeDatabase/add", Role.EMPLE, cartypeDTO);
            IEnumerable<CartypeDTO> cartypeDTOs = (await DatabaseGet<IEnumerable<CartypeDTO>>($"api/CartypeDatabase/getall", Role.ANONM)).OrderBy(c => c.Id);
            Assert.NotEqual(cartypeDTO.Id, cartypeDTOs.Last().Id);
            cartypeDTO.Id = cartypeDTOs.Last().Id;
            Assert.Equal(cartypeDTO.ToJson(), cartypeDTOs.Last().ToJson());
        }

        [Fact]
        public async Task CartypeDatabaseChange()
        {
            int cartypeID = 1;
            CartypeDTO cartypeDTO = new(new Cartype("name", "manufacturer", "fueltype", DataSeeder.carclasses[0]));
            cartypeDTO.Id = cartypeID;
            await DatabasePut($"api/CartypeDatabase/change/{cartypeID}", Role.EMPLE, cartypeDTO);
            CartypeDTO changedPlanDTO = await DatabaseGet<CartypeDTO>($"api/CartypeDatabase/get/{cartypeID}", Role.ANONM);
            Assert.Equal(cartypeDTO.ToJson(), changedPlanDTO.ToJson());
            CartypeDTO oldCartypeDTO = new CartypeDTO(DataSeeder.cartypes[cartypeID - 1]);
            oldCartypeDTO.Id = cartypeID;
            await DatabasePut($"api/CartypeDatabase/change/{cartypeID}", Role.EMPLE, oldCartypeDTO);
        }



        [Fact]
        public async Task CarclassDatabaseGet()
        {
            int carclassID = 1;
            CarclassDTO cartype = await DatabaseGet<CarclassDTO>($"api/CarclassDatabase/get/{carclassID}", Role.ANONM);
            Assert.Equal(new CarclassDTO(DataSeeder.carclasses[carclassID - 1]).ToJson(), cartype.ToJson());
        }

        [Fact]
        public async Task CarclassDatabaseGetAll()
        {
            IEnumerable<CarclassDTO> carclasses = (await DatabaseGet<IEnumerable<CarclassDTO>>("api/CarclassDatabase/getall", Role.ANONM)).OrderBy(c => c.Id);
            List<CarclassDTO> seedCarclasses = new();
            foreach (Carclass carclass in DataSeeder.carclasses)
                seedCarclasses.Add(new CarclassDTO(carclass));
            Assert.Equal(seedCarclasses.ToJson(), carclasses.ToList().GetRange(0, DataSeeder.carclasses.Count).ToJson());
        }

        [Fact]
        public async Task CarclassDatabaseAdd()
        {
            Carclass carclassDTO = new("name", 1);
            await DatabasePost($"api/CarclassDatabase/add", Role.EMPLE, carclassDTO);
            IEnumerable<Carclass> carclassDTOs = (await DatabaseGet<IEnumerable<Carclass>>($"api/CarclassDatabase/getall", Role.ANONM)).OrderBy(p => p.Id);
            Assert.NotEqual(carclassDTO.Id, carclassDTOs.Last().Id);
            carclassDTO.Id = carclassDTOs.Last().Id;
            Assert.Equal(carclassDTO.ToJson(), carclassDTOs.Last().ToJson());
        }

        [Fact]
        public async Task CarclassDatabaseChange()
        {
            int carclassID = 1;
            Carclass carclassDTO = new("name", 1);
            carclassDTO.Id = carclassID;
            await DatabasePut($"api/CarclassDatabase/change/{carclassID}", Role.EMPLE, carclassDTO);
            Carclass changedPlanDTO = await DatabaseGet<Carclass>($"api/CarclassDatabase/get/{carclassID}", Role.ANONM);
            Assert.Equal(carclassDTO.ToJson(), changedPlanDTO.ToJson());
            Carclass oldCarclassDTO = new Carclass(DataSeeder.carclasses[carclassID - 1].Name, DataSeeder.carclasses[carclassID - 1].PriceFaktor);
            oldCarclassDTO.Id = carclassID;
            await DatabasePut($"api/CarclassDatabase/change/{carclassID}", Role.EMPLE, oldCarclassDTO);
        }



        [Fact]
        public async Task CarDatabaseGet()
        {
            int carID = 1;
            CarDTO car = await DatabaseGet<CarDTO>($"api/CarDatabase/get/{carID}", Role.EMPLE);

            // check conversion diff
            Assert.True(Math.Abs(car.TankLevel - new CarDTO(DataSeeder.cars[carID - 1]).TankLevel) < 1);
            car.TankLevel = new CarDTO(DataSeeder.cars[carID - 1]).TankLevel;

            Assert.Equal(new CarDTO(DataSeeder.cars[carID - 1]).ToJson(), car.ToJson());
        }

        [Fact]
        public async Task CarDatabaseGetAll()
        {
            IEnumerable<CarDTO> cars = (await DatabaseGet<IEnumerable<CarDTO>>("api/CarDatabase/getall", Role.EMPLE)).OrderBy(c => c.Id);
            List<CarDTO> seedCars = new();
            foreach (Car car in DataSeeder.cars)
                seedCars.Add(new CarDTO(car));
            // check conversion diff
            for (int i = 0; i < seedCars.Count; i++)
            {
                Assert.True(Math.Abs(cars.ElementAt(i).TankLevel - seedCars.ElementAt(i).TankLevel) < 1);
                cars.ElementAt(i).TankLevel = seedCars.ElementAt(i).TankLevel;
            }
            Assert.Equal(seedCars.ToJson(), cars.ToList().GetRange(0, DataSeeder.cars.Count).ToJson());
        }

        // test booking
        [Fact]
        public async Task CarDatabaseAdd()
        {
            int cartypeID = 1;
            int stationID = 1;
            CarDTO carDTO= new CarDTO(new Car(0, 1, Car.CarStatus.onStation, Car.CarLockStatus.locked, DataSeeder.cartypes[cartypeID - 1], "licensePlateNumber"));
            carDTO.Type.Id = cartypeID;
            carDTO.Type.Carclass.Id = 1;
            AddCarDTO addCarDTO = new(carDTO, new StationDTO(DataSeeder.stations[stationID - 1]), new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero).ToUnixTimeMilliseconds());
            addCarDTO.Station.Id = stationID;
            await DatabasePost($"api/CarDatabase/add", Role.EMPLE, addCarDTO);
            IEnumerable<CarDTO> carDTOs = (await DatabaseGet<IEnumerable<CarDTO>>($"api/CarDatabase/getall", Role.EMPLE)).OrderBy(c => c.Id);
            Assert.NotEqual(addCarDTO.Car.Id, carDTOs.Last().Id);
            carDTO.Id = carDTOs.Last().Id;
            Assert.Equal(carDTO.ToJson(), carDTOs.Last().ToJson());
        }

        [Fact]
        public async Task CarDatabaseChange()
        {
            int carID = 1;
            int cartypeID = 1;
            CarDTO carDTO = new(new Car(0, 1, Car.CarStatus.onStation, Car.CarLockStatus.locked, DataSeeder.cartypes[cartypeID - 1], "licensePlateNumber"));
            carDTO.Id = carID;
            await DatabasePut($"api/CarDatabase/change/{carID}", Role.EMPLE, carDTO);
            CarDTO changedPlanDTO = await DatabaseGet<CarDTO>($"api/CarDatabase/get/{carID}", Role.EMPLE);
            Assert.Equal(carDTO.ToJson(), changedPlanDTO.ToJson());
            CarDTO oldCarDTO = new CarDTO(DataSeeder.cars[carID - 1]);
            oldCarDTO.Id = carID;
            await DatabasePut($"api/CarDatabase/change/{carID}", Role.EMPLE, oldCarDTO);
        }



        [Fact]
        public async Task AdminGet()
        {
            int employeeID = 2;
            EmployeeDTO employeeDTO = await DatabaseGet<EmployeeDTO>($"api/Admin/get/{employeeID}", Role.ADMIN);
            Assert.Equal(new EmployeeDTO(DataSeeder.employees[0]).ToJson(), employeeDTO.ToJson());
        }

        [Fact]
        public async Task AdminGetAll()
        {
            IEnumerable<EmployeeDTO> employees = (await DatabaseGet<IEnumerable<EmployeeDTO>>("api/Admin/getall", Role.ADMIN)).OrderBy(e => e.person.Id);
            List<EmployeeDTO> seedEmployees = new();
            foreach (Employee employee in DataSeeder.employees)
                seedEmployees.Add(new EmployeeDTO(employee));
            for (int i = 0; i < seedEmployees.Count; i++)
            {
                // Last Login fewer than 2 Minutes ago
                //Assert.NotEqual(seedEmployees[i].person.LastLogin, employees.ToList()[i].person.LastLogin);
                //Assert.True((int)DateTime.UtcNow.Subtract(DateTimeOffset.FromUnixTimeMilliseconds(employees.ToList()[i].person.LastLogin).UtcDateTime).TotalMinutes < 2);
                seedEmployees[i].person.LastLogin = employees.ToList()[i].person.LastLogin;
            }
            Assert.Equal(seedEmployees.ToJson(), employees.ToList().GetRange(0, DataSeeder.employees.Count).ToJson());
        }

        [Fact]
        public async Task AdminAddRemove()
        {
            EmployeeDTO employeeDTO = new(new Employee(DataSeeder.person, true));
            employeeDTO.person.Email = $"{DataSeeder.GetRandInt()}" + employeeDTO.person.Email;

            await DatabasePost($"api/Admin/add", Role.ADMIN, employeeDTO);
            IEnumerable<EmployeeDTO> employeeDTOs = (await DatabaseGet<IEnumerable<EmployeeDTO>>($"api/Admin/getall", Role.ADMIN)).OrderBy(e => e.person.Id);
            Assert.NotEqual(employeeDTO.person.Id, employeeDTOs.Last().person.Id);
            int employeeID = employeeDTO.person.Id = employeeDTOs.Last().person.Id;
            employeeDTO.person.LastLogin = employeeDTOs.Last().person.LastLogin;
            Assert.Equal(employeeDTO.ToJson(), employeeDTOs.Last().ToJson());

            await DatabaseDelete($"api/Admin/delete/{employeeID}", Role.ADMIN);
            employeeDTOs = (await DatabaseGet<IEnumerable<EmployeeDTO>>($"api/Admin/getall", Role.ADMIN)).OrderBy(e => e.person.Id);
            Assert.DoesNotContain(employeeDTOs, e => e.person.Id == employeeID);

        }

        [Fact]
        public async Task AdminChange()
        {
            int employeeID = 2;
            EmployeeDTO oldEmployeeDTO = await DatabaseGet<EmployeeDTO>($"api/Admin/get/{employeeID}", Role.ADMIN);
            EmployeeDTO employeeDTO = new(new Employee(DataSeeder.person, true));
            employeeDTO.person.Email = $"{DataSeeder.GetRandInt()}" + employeeDTO.person.Email;
            employeeDTO.person.Id = employeeID;
            await DatabasePut($"api/Admin/change/{employeeID}", Role.ADMIN, employeeDTO);
            EmployeeDTO changedPlanDTO = await DatabaseGet<EmployeeDTO>($"api/Admin/get/{employeeID}", Role.ADMIN);
            employeeDTO.person.LastLogin = changedPlanDTO.person.LastLogin;
            Assert.Equal(employeeDTO.ToJson(), changedPlanDTO.ToJson());
            await DatabasePut($"api/Admin/change/{employeeID}", Role.ADMIN, oldEmployeeDTO);
        }
    }
}
