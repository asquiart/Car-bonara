using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /*public void OnModelCreating(ModelBuilder  modelbuilder) {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Plan>().HasData(TestDataSeeder.plans);
            modelbuilder.Entity<Carclass>().HasData(TestDataSeeder.carclasses);
            modelbuilder.Entity<Cartype>().HasData(TestDataSeeder.cartypes);
            modelbuilder.Entity<Car>().HasData(TestDataSeeder.cars);
            modelbuilder.Entity<Address>().HasData(TestDataSeeder.addresses);
            modelbuilder.Entity<Station>().HasData(TestDataSeeder.stations);
            modelbuilder.Entity<Booking>().HasData(TestDataSeeder.bookings);
            modelbuilder.Entity<Person>().HasData(TestDataSeeder.people);
            modelbuilder.Entity<Employee>().HasData(TestDataSeeder.employees);
            modelbuilder.Entity<User>().HasData(TestDataSeeder.users);
            modelbuilder.Entity<AuthData>().HasData(TestDataSeeder.authDatas);
        }*/

        public DbSet<AuthData> AuthData { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Carclass> Carclass { get; set; }
        public DbSet<Cartype> Cartype { get; set; }
        public DbSet<Cleaning> Cleaning { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Maintanance> Maintanance { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Bill> Bill { get; set; }
    }
}
