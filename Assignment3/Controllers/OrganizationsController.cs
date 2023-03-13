/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: A controller for Organization that determines what response to send back to a user when an HTTP request is made.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment3.Data;
using Assignment3.Models;
using System.Xml.Serialization;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using System.Text.Json;

namespace Assignment3.Controllers
{
    /// <summary>
    /// This class represents a controller for Organization that determines what response to send back to a user when an HTTP request is made.
    /// </summary>
    [Route("/Organization")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        /// <value>
        /// The DbContext for the health care server application.
        /// </value>
        private readonly Assignment3Context _context;

        /// <summary>
        /// A constructor for OrganizationController that sets its Assignment3Context.
        /// </summary>
        /// <param name="context">The Assignment3Context of teh health care server application.</param>
        public OrganizationsController(Assignment3Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a single Organization record by the Organization id.
        /// </summary>
        /// <param name="organizationId">The id of the requested organization.</param>
        /// <returns>The record in XML or JSON if it is found in the database, otherwise an appropriate HTTP StatusCode with an Error object in XML or JSON.</returns>
        [HttpGet("{organizationId}")]
        public async Task<ActionResult<Organization>> GetOrganization(Guid organizationId)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (organizationId.Equals(null))
            {
                Error error400 = new Error(400, "Mandatory field missing: id.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            var organization = await _context.Organization.FindAsync(organizationId);

            if (organization == null)
            {
                Error error404 = new Error(404, $"Organization id {organizationId} was not found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, organization);
        }

        /// <summary>
        /// Retrieves all Organization records that match the name provided.
        /// </summary>
        /// <param name="name">The name of the requested organization.</param>
        /// <returns>The records in XML or JSON any records in the database match the provided name, otherwise an appropriate HTTP StatusCode with an Error object in XML or JSON.</returns>
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizationsByName(string name)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (name == null)
            {
                Error error400 = new Error(400, "Mandatory field missing: name.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            var organizations = from o in _context.Organization
                                where o.Name == name
                                select o;

            if (!organizations.Any())
            {
                Error error404 = new Error(404, $"No organizations named {name} were found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, await organizations.ToListAsync());
        }

        /// <summary>
        /// Retrieves all Organization records that match the type provided.
        /// </summary>
        /// <param name="type">The type of the requested organization.</param>
        /// <returns>The records in XML or JSON any records in the database match the provided type, otherwise an appropriate HTTP StatusCode with an Error object in XML or JSON.</returns>
        [HttpGet("type")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizationsByType(string type)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (type == null)
            {
                Error error400 = new Error(400, "Mandatory field missing: type.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            var organizations = from o in _context.Organization
                                where o.Type == type
                                select o;

            if (!organizations.Any())
            {
                Error error404 = new Error(404, $"No organizations of type {type} were found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, await organizations.ToListAsync());
        }

        /// <summary>
        /// Updates an organization record.
        /// </summary>
        /// <param name="id">The id of the requested organization.</param>
        /// <param name="organization">The incoming Organization.</param>
        /// <returns>An appropriate HTTP StatusCode with an Error object in XML or JSON.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Organization>> PutOrganization(Guid id, Organization organization)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.ContentType != "application/xml" && Request.Headers.ContentType != "application/json")
            {
                Error error415 = new Error(415, "Content is not in XML or JSON format.");
                _context.Error.Add(error415);
                await _context.SaveChangesAsync();
                return StatusCode(415, error415);
            }

            if (id.Equals(null))
            {
                Error error400 = new Error(400, "Mandatory field missing: id.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (organization.Name == null || organization.Type == null || organization.Address == null)
            {
                Error error400 = new Error(400, "Mandatory field missing.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (!OrganizationExists(id))
            {
                Error error404 = new Error(404, $"Organization id {id} could not be found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            if (organization.Id != id)
            {
                Error error400 = new Error(400, "The organization id must match the id provided in the PUT body.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (!(organization.Type.ToLower().Equals("hospital") ||
                organization.Type.ToLower().Equals("clinic") ||
                organization.Type.ToLower().Equals("pharmacy")))
            {
                Error error400 = new Error(400, "Type must be Hospital, Clinic, or Pharmacy.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (organization.Name.Length > 256)
            {
                Error error400 = new Error(400, "Name must be 256 characters or less.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            _context.Entry(organization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }

            Error error200 = new Error(200, "The PUT operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, error200);
        }

        /// <summary>
        /// Creates an organization record.
        /// </summary>
        /// <param name="organization">The incoming Organization.</param>
        /// <returns>An appropriate HTTP StatusCode with an Error object in XML or JSON.</returns>
        [HttpPost]
        public async Task<ActionResult<Organization>> PostOrganization(Organization organization)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.ContentType != "application/xml" && Request.Headers.ContentType != "application/json")
            {
                Error error415 = new Error(415, "Content is not in XML or JSON format.");
                _context.Error.Add(error415);
                await _context.SaveChangesAsync();
                return StatusCode(415, error415);
            }

            if (string.IsNullOrEmpty(organization.Name) || string.IsNullOrEmpty(organization.Type) || string.IsNullOrEmpty(organization.Address))
            {
                Error error400 = new Error(400, "Mandatory field missing.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (OrganizationExists(organization.Id))
            {
                Error error400 = new Error(400, "Id already exists in the database.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (!(organization.Type.ToLower().Equals("hospital") ||
                organization.Type.ToLower().Equals("clinic") ||
                organization.Type.ToLower().Equals("pharmacy")))
            {
                Error error400 = new Error(400, "Type must be Hospital, Clinic, or Pharmacy.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (organization.Name.Length > 256)
            {
                Error error400 = new Error(400, "Name must be 256 characters or less.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            _context.Organization.Add(organization);
            await _context.SaveChangesAsync();

            var result = CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);

            if (result.StatusCode == 201)
            {
                Error error201 = new Error(201, "The POST operation completed successfully.");
                _context.Error.Add(error201);
                await _context.SaveChangesAsync();
                return StatusCode(201, error201);
            }

            Error error500 = new Error(500, "An unexpected error occurred.");
            _context.Error.Add(error500);
            await _context.SaveChangesAsync();
            return StatusCode(500, error500);
        }

        /// <summary>
        /// Deletes an organization record by the organization id.
        /// </summary>
        /// <param name="id">The id of the organization to be deleted.</param>
        /// <returns>An appropriate HTTP StatusCode with an Error object in XML or JSON.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Organization>> DeleteOrganization(Guid id)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                Error error404 = new Error(404, $"Organization id {id} could not be found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();

            Error error204 = new Error(204, "DELETE operation completed successfully.");
            _context.Error.Add(error204);
            await _context.SaveChangesAsync();
            return StatusCode(204, error204);
        }

        /// <summary>
        /// Determines whether an Organization is in the database.
        /// </summary>
        /// <param name="id">The id of the organization.</param>
        /// <returns>True if the organization is in the database, false otherwise.</returns>
        private bool OrganizationExists(Guid id)
        {
            return _context.Organization.Any(e => e.Id == id);
        }

    }
}