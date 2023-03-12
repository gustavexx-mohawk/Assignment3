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
    [Route("/Organization")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public OrganizationsController(Assignment3Context context)
        {
            _context = context;
        }

        // GET: api/Organization/96857544-ee06-45c0-a704-0a04cac90ec7
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(Guid id)
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

            if (id.Equals(null))
            {
                Error error400 = new Error(400, "Mandatory field missing: id.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                Error error404 = new Error(404, "Organization id " + id + " was not found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, organization);
        }

        // GET: api/Organization?name="value"
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

        // GET: api/Organization?type="value"
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

        // PUT: api/Organization/96857544-ee06-45c0-a704-0a04cac90ec7
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Organization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // DELETE: api/Organizations/5
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

        private bool OrganizationExists(Guid id)
        {
            return _context.Organization.Any(e => e.Id == id);
        }

    }
}