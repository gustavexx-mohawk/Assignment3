using Assignment3.Controllers;
using Assignment3.Data;
using Assignment3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using NuGet.RuntimeModel;
using System.Text;
using System.Text.Json;

namespace Assignment3UnitTest
{
    [TestClass]
    public class UnitTests:ControllerBase
    {
        private Assignment3Context db;

        /// <summary>
        /// Tests the Immunization Controller PostImmunization method, 
        /// retrieving the status code. 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Immunization_PostRequest()
        {

            var options = new DbContextOptionsBuilder<Assignment3Context>()
                .UseSqlServer()
                .Options;
            db = new Assignment3Context(options);

            //Create database
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            //Immunization record
            Immunization immunization = new Immunization()
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTimeOffset.Now,
                OfficialName = "Iboprohen",
                LotNumber = "B43JJ5",
                ExpirationDate = DateTimeOffset.Now.AddMonths(1)
            };

            var immunizationController = new ImmunizationsController(db);
            immunizationController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            immunizationController.ControllerContext.HttpContext = new DefaultHttpContext();
            immunizationController.Request.Headers.Add("Accept", "application/json");

            var serializedToJson = JsonSerializer.Serialize(immunization);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedToJson));

            immunizationController.Request.Body = stream;
            immunizationController.HttpContext.Response.ContentType = "application/json";

            var test3 = immunizationController.Response;
            //Act
            ImmunizationsController response = new ImmunizationsController(db);
            //var test2 = await response.PostImmunization(immunization); // = await immunizationController.PostImmunization(immunization);
           // var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7267/Immunization");
            //request.Headers.Add("Accept", "application/json");

            //Microsoft.AspNetCore.Mvc.ActionResult<Error> cres =  (Microsoft.AspNetCore.Mvc.ActionResult<Error>)response;

            //var request = new HttpRequestMessage(HttpMethod.Post, "http://stackoverflow");
            //request.Headers.Add("Accept", "application/json");
            //// = request;

            //var test = response.Value.StatusCode;
            //Assert
            Assert.AreEqual(200, test3.StatusCode);
        }

        [TestMethod]
        public async Task Organization_PostRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Assignment3Context>()
                .UseSqlServer()
                .Options;
            db = new Assignment3Context(options);

            // Create database
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Organization record
            Organization organization = new Organization()
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTimeOffset.Now,
                Name = "Rexall",
                Type = "Pharmacy",
                Address = "930 Upper Paradise Rd #13, Hamilton, ON L9B 2N1"
            };

            var organizationController = new OrganizationsController(db);
            organizationController.ControllerContext = new ControllerContext();
            organizationController.ControllerContext.HttpContext = new DefaultHttpContext();
            organizationController.Request.Headers.Add("Accept", "application/json");

            var serializedToJson = JsonSerializer.Serialize(organization);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedToJson));

            organizationController.Request.Body = stream;
            organizationController.HttpContext.Response.ContentType = "application/json";

            // Act
            var actualResult = organizationController.Response;
            
            //Assert
            Assert.AreEqual(200, actualResult.StatusCode);
        }

        [TestMethod]
        public async Task Patient_PostRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Assignment3Context>()
                .UseSqlServer()
                .Options;
            db = new Assignment3Context(options);

            // Create database
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Patient record
            Patient patient = new Patient()
            {
                Id = Guid.NewGuid(),
                CreationTime = DateTimeOffset.Now,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTimeOffset.Parse("1987-07-06").Date
            };

            var patientController = new PatientsController(db);
            patientController.ControllerContext = new ControllerContext();
            patientController.ControllerContext.HttpContext = new DefaultHttpContext();
            patientController.Request.Headers.Add("Accept", "application/json");

            var serializedToJson = JsonSerializer.Serialize(patient);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedToJson));

            patientController.Request.Body = stream;
            patientController.HttpContext.Response.ContentType = "application/json";

            // Act
            var actualResult = patientController.Response;

            //Assert
            Assert.AreEqual(200, actualResult.StatusCode);

        }
    }
}