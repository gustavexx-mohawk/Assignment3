using Assignment3.Controllers;
using Assignment3.Data;
using Assignment3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnitTests

    // https://dotnetthoughts.net/how-to-mock-dbcontext-for-unit-testing/
{
    [TestClass]
    public class UnitTest1
    {
        private static Guid organizationId;
        private async Task<Assignment3Context> GetAssignment3Context()
        {
            var options = new DbContextOptionsBuilder<Assignment3Context>()
                .UseSqlServer()
                .Options;

            var assignment3Context = new Assignment3Context(options);
            assignment3Context.Database.EnsureCreated();
            if (await assignment3Context.Organization.CountAsync() <= 0)
            {
                Organization organization = new Organization();
                organization.Name = "Rexall";
                organization.Type = "Pharmacy";
                organization.Address = "930 Upper Paradise Rd #13, Hamilton, ON L9B 2N1";

                organizationId = organization.Id;

                assignment3Context.Organization.Add(organization);

                await assignment3Context.SaveChangesAsync();
            }

            return assignment3Context;
        }

        [TestMethod]
        public async Task GetOrganization_ValidId()
        {
            // Arrange
            var dbContext = await GetAssignment3Context();

            OrganizationsController organizationsController = new OrganizationsController(dbContext);

            // Act
            ActionResult<Organization> organization = await organizationsController.GetOrganization(organizationId);
            
            //Assert
            Assert.IsNotNull(organization.Value);
        }
    }
}