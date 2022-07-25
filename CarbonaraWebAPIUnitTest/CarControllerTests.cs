using CarbonaraWebAPI.Controllers;
using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CarbonaraWebAPIUnitTest
{
    public class CarControllerTests
    {
        AppDbContext dbContext;
        CarController carController;

        [SetUp]
        public void Setup()
        {
            dbContext = TestUtilities.GetTestAppDbContext();
            var carService = new CarService(dbContext);
            carController = new CarController(carService, dbContext);
        }



    }
}