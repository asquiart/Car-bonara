using CarbonaraWebAPI.Controllers;
using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using CarbonaraWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static CarbonaraWebAPIUnitTest.TestUtilities;

namespace CarbonaraWebAPIUnitTest
{
    public class EndBookingTest
    {

        AppDbContext context;
        CarService carService;
        BookingService bookingService;
        BookingController bookingController;

        [SetUp]
        public void Setup()
        {
            context = GetTestAppDbContext();
            carService = new CarService(context);
            bookingService = new BookingService(context, carService);
            bookingController = new BookingController(bookingService, carService, context);
        }


       

       


      
    }
}