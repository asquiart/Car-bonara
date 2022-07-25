using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonaraWebAPITest.Util
{
    public static class DataSeeder
    {
        static Random random = new Random();

        public static List<Plan> plans = new()
        {
            new Plan("Basis", 30, 3.5f, 0.5f, 0.28f, 30, 10),
            new Plan("Premium", 20, 1.7f, 0.5f, 0.24f, 30, 10)
        };
        public static List<Carclass> carclasses = new()
        {
            new Carclass("Kleinwagen", 1.0f),
            new Carclass("Mittelklasse", 1.3f),
            new Carclass("Transporter", 2.5f)
        };
        public static List<Cartype> cartypes = new()
        {
            new Cartype("ForTwo", "Smart", "Super Bleifrei", carclasses[0]),
            new Cartype("ForFour", "Smart", "Diesel", carclasses[1])
        };
        public static List<Car> cars = new()
        {
            new Car(random.Next(10000), (float)random.NextDouble()*random.Next(10000), Car.CarStatus.onStation, Car.CarLockStatus.locked, cartypes[0], "HB ABC 1"),
            new Car(random.Next(10000), (float)random.NextDouble()*random.Next(10000), Car.CarStatus.onStation, Car.CarLockStatus.locked, cartypes[0], "HB ABC 2"),
            new Car(random.Next(10000), (float)random.NextDouble()*random.Next(10000), Car.CarStatus.onStation, Car.CarLockStatus.locked, cartypes[0], "HB ABC 3"),
            new Car(random.Next(10000), (float)random.NextDouble()*random.Next(10000), Car.CarStatus.onStation, Car.CarLockStatus.locked, cartypes[1], "HB ABC 4"),
            new Car(random.Next(10000), (float)random.NextDouble()*random.Next(10000), Car.CarStatus.onStation, Car.CarLockStatus.locked, cartypes[1], "HB ABC 5"),
            new Car(random.Next(10000), (float)random.NextDouble()*random.Next(10000), Car.CarStatus.onStation, Car.CarLockStatus.locked, cartypes[1], "HB ABC 6"),
            new Car(random.Next(10000), (float)random.NextDouble()*random.Next(10000), Car.CarStatus.onStation, Car.CarLockStatus.locked, cartypes[1], "HB ABC 7")
        };
        public static List<Address> addresses = new()
        {
            new Address(random.Next(100).ToString(), random.Next(10000).ToString(), "Deutschland", "Hamburg", "Sesamstraße"),
            new Address("15", "28195", "Deutschland", "Bremen", "Bahnhofsplatz"),
            new Address("30", "28199", "Deutschland", "Bremen", "Neustadtswall "),
            RandAddress(),
            RandAddress(),
            RandAddress(),
            RandAddress(),
            RandAddress()
        };
        public static List<Station> stations = new()
        {
            new Station("Sesamstraße", addresses[0], 3),
            new Station("Hauptbahnhof", addresses[1], 10),
            new Station("Hochschule Bremen", addresses[2], 1),
            new Station("Teststation", addresses[7], 10)
        };
        public static List<Person> people = new()
        {
            new Person("ENTWICKLER", "KONTO", "contact@car-bonara.de", "DEV", "MOIN", DateTime.UtcNow),
            new Person("Elmo", "Monster", "Elmo.Monster@Car-bonara.de", "", "", DateTime.UtcNow),
            new Person("Robert", "Smiley", "Robert.Smiley@Car-bonara.de", "", "", DateTime.UtcNow),
            new Person("Krümel", "Monster", "Krümel.Monster@Keks.de", "", "Möchtest du einen Keks", DateTime.UtcNow),
            new Person("Oscar", "Wild", "Oscar@Griesgram.de", "", "Wieder ein neuer Tag", DateTime.UtcNow),
            new Person("Zahl", "", "Graf.Zahl@1und1.de", "Graf", "Hallo", DateTime.UtcNow),
            new Person("Schlemihl", "Händler", "A@domainhunter.com", "Wer, ich?", "Psssst!", DateTime.UtcNow)
        };
        public static List<User> users = new()
        {
            new User(people[3], plans[0], addresses[3], "xxx", User.UserState.Authorized, 0, User.PaymentMethod.Paypal),
            new User(people[4], plans[0], addresses[4], "xxx", User.UserState.Unauthorized, 0, User.PaymentMethod.Sepa),
            new User(people[5], plans[1], addresses[5], "xxx", User.UserState.Authorized, 0, User.PaymentMethod.Visa),
            new User(people[6], plans[1], addresses[6], "xxx", User.UserState.Locked, 0, User.PaymentMethod.Mastercard)
        };
        public static List<Booking> bookings = new()
        {
            new Booking( DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow, "Initial Booking", false, true, null, cars[0].Type.Carclass, null, stations[0], cars[0], null),
            new Booking( DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow, "Initial Booking", false, true, null, cars[1].Type.Carclass, null, stations[1], cars[1], null),
            new Booking( DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow, "Initial Booking", false, true, null, cars[2].Type.Carclass, null, stations[1], cars[2], null),
            new Booking( DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow, "Initial Booking", false, true, null, cars[3].Type.Carclass, null, stations[2], cars[3], null),
            new Booking( DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow, "Initial Booking", false, true, null,cars[4].Type.Carclass, null, stations[3], cars[4], null),
            new Booking( DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow, "Initial Booking", false, true, null, cars[5].Type.Carclass, null, stations[3], cars[5], null),
            new Booking( DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-10), DateTime.UtcNow, "Initial Booking", false, true, null, cars[6].Type.Carclass, null, stations[3], cars[6], null),
            new Booking(DateTime.UtcNow.AddHours(-10), DateTime.UtcNow.AddHours(-2), DateTime.UtcNow, "Test Time Overdue", false, false, users[2], cars[4].Type.Carclass, stations[3], stations[3], cars[4], null),
            new Booking(DateTime.UtcNow.AddDays(-4).Date.AddHours(21), DateTime.UtcNow.AddDays(-2).Date.AddHours(6), DateTime.UtcNow, "Test To Late to Start", false, false, users[2], cars[5].Type.Carclass, stations[3], stations[3], null, null),
            new Booking(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1), DateTime.UtcNow, "Test Ready to Start", false, false, users[2], cars[6].Type.Carclass, stations[3], stations[3], null, null),
            new Booking(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1), DateTime.UtcNow, "Test Canceled", true, false, users[2], cars[6].Type.Carclass, stations[3], stations[3], null, null),
           };
        public static Person person = new("firstname", "lastname", "firstname.lastname@email.com", "title", "formOfAddress", DateTime.UtcNow);
        public static User user = new User(person, plans[0], RandAddress(), "xxx", User.UserState.Unauthorized, 0, User.PaymentMethod.Paypal);
       
        public static List<Employee> employees = new()
        {
            new Employee(people[0], true),
            new Employee(people[1], true),
            new Employee(people[2], false)
        };
       

        public static List<AuthData> authDatas = new()
        {
            AuthService.CreatePasswordHash(people[0].Firstname + "123").AddPerson(people[0]),
            AuthService.CreatePasswordHash(people[1].Firstname + "123").AddPerson(people[1]),
            AuthService.CreatePasswordHash(people[2].Firstname + "123").AddPerson(people[2]),
            AuthService.CreatePasswordHash(people[3].Firstname + "123").AddPerson(people[3]),
            AuthService.CreatePasswordHash(people[4].Firstname + "123").AddPerson(people[4]),
            AuthService.CreatePasswordHash(people[5].Firstname + "123").AddPerson(people[5]),
            AuthService.CreatePasswordHash(people[6].Firstname + "123").AddPerson(people[6])
        };

        public enum Role { ANONM, SGDIN, AUTHU, EMPLE, ADMIN , SGDIN2 };
        public enum HttpMethod { GET, POST, PUT, DELETE, PATCH };
        public static Dictionary<Role, Person> RolePerson = new() {
            { Role.SGDIN2, people[5] },
            { Role.SGDIN, people[4] },
            { Role.AUTHU, people[3] },
            { Role.EMPLE, people[2] },
            { Role.ADMIN, people[1] } };

        public static Address RandAddress()
        {
            return new Address(random.Next(100).ToString(), random.Next(10000).ToString(), "Deutschland", "Hamburg", "Sesamstraße");
        }

        public static int GetRandInt()
        {
            return random.Next();
        }

        public static void InitializeDb(AppDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
            dbContext.AddRange(plans);
            dbContext.AddRange(carclasses);
            dbContext.AddRange(cartypes);
            dbContext.AddRange(cars);
            dbContext.AddRange(addresses);
            dbContext.AddRange(stations);
            dbContext.AddRange(bookings);
            dbContext.AddRange(people);
            dbContext.AddRange(employees);
            dbContext.AddRange(users);
            dbContext.AddRange(authDatas);
            dbContext.SaveChanges();
        }
    }
}
