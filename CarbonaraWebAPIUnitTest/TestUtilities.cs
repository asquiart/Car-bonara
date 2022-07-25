using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarbonaraWebAPIUnitTest
{
    public static class TestUtilities
    {

        const string INMEMORY_CONNECTION_STRING = "Data Source=./db/testing.sqlite;Mode=ReadWriteCreate";
        static private AppDbContext sharedDbContext;

        public static AppDbContext GetTestAppDbContext()
        {
            if (sharedDbContext == null)
            {
                sharedDbContext = SetupDbContext();
            }
            return sharedDbContext;
        }

        private static AppDbContext SetupDbContext()
        {
            Directory.CreateDirectory("db");
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>();
            contextOptions.UseSqlite(INMEMORY_CONNECTION_STRING);
            AppDbContext context = new AppDbContext(contextOptions.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            DataSeeder.InitializeDb(context, false);
            context.SaveChanges();
            return context;
        }


        public static void SetUser(this ControllerBase controller, int id)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("User", id.ToString()));
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        public static int GetResponseStatus(this ActionResult? result)
        {
            if (result == null)
                return -1;

            ObjectResult? objectResult = result as ObjectResult;
            if (objectResult == null)
                return -1;

            return objectResult.StatusCode ?? -1;
        }

        public static T?  GetResponseValue<T>(this ActionResult? result) where T : class
        {
            if (result == null)
                return null;

            ObjectResult? objectResult = result as ObjectResult;
            if (objectResult == null)
                return null;



            return objectResult.Value as T;
        }
    }
}
