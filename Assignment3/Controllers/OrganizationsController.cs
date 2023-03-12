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

        // POST: api/Organization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Organization>> PostOrganization(Organization organization)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406.ToString());
            }

            //if (Request.Headers.ContentType == "application/xml")
            //{
            //    byte[] xmlOrganization = SerializeToXml<Organization>(organization);
            //    Organization xmlDeserializedOrganization = DeserializeFromXml<Organization>(xmlOrganization);

            //    if (!(xmlDeserializedOrganization.GetType() == typeof(Organization)))
            //    {
            //        Error error415 = new Error(415, "Content is not in valid XML format.");
            //        _context.Error.Add(error415);
            //        await _context.SaveChangesAsync();
            //        return StatusCode(415, error415.Message);
            //    }
            //}
            //else if (Request.Headers.ContentType == "application/json")
            //{
            //    if (!(JsonSerializer.Deserialize<Organization>(organization.ToString()).GetType() == typeof(Organization)))
            //    {
            //        Error error415 = new Error(415, "Content is not in valid Json format.");
            //        _context.Error.Add(error415);
            //        await _context.SaveChangesAsync();
            //        return StatusCode(415, error415.Message);
            //    }
            //}
            //else

            if (Request.Headers.ContentType != "application/xml" && Request.Headers.ContentType != "application/json")
            {
                Error error415 = new Error(415, "Content is not in XML or JSON format.");
                _context.Error.Add(error415);
                await _context.SaveChangesAsync();
                return StatusCode(415, error415.ToString());
            }

            if (organization.Name == null || organization.Type == null || organization.Address == null)
            {
                Error error400 = new Error(400, "Mandatory field missing.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            if (!(organization.Type.ToLower().Equals("hospital") ||
                organization.Type.ToLower().Equals("clinic") ||
                organization.Type.ToLower().Equals("pharmacy")))
            {
                Error error400 = new Error(400, "Mandatory field missing: Type must be Hospital, Clinic, or Pharmacy.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            _context.Organization.Add(organization);
            await _context.SaveChangesAsync();

            var result = CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);

            if (result.StatusCode == 201)
            {
                Error error201 = new Error(201, "The POST operation completed successfully.");
                _context.Error.Add(error201);
                await _context.SaveChangesAsync();
                return StatusCode(201, error201.ToString());
            }

            Error error500 = new Error(500, "An unexpected error occurred.");
            _context.Error.Add(error500);
            await _context.SaveChangesAsync();
            return StatusCode(500, error500.ToString());
        }

        // GET: api/Organization/96857544-ee06-45c0-a704-0a04cac90ec7
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(Guid id)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406.ToString());
            }

            if (id.Equals(null))
            {
                Error error400 = new Error(400, "Mandatory field missing: id.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                Error error404 = new Error(404, "Organization id " + id + " was not found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404.ToString());
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, organization);
        }

        // PUT: api/Organization/96857544-ee06-45c0-a704-0a04cac90ec7
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Organization>> PutOrganization(Guid id, Organization organization)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406.ToString());
            }

            //if (Request.Headers.ContentType == "application/xml")
            //{
            //    byte[] xmlOrganization = SerializeToXml<Organization>(organization);
            //    Organization xmlDeserializedOrganization = DeserializeFromXml<Organization>(xmlOrganization);

            //    if (!(xmlDeserializedOrganization.GetType() == typeof(Organization)))
            //    {
            //        Error error415 = new Error(415, "Content is not in valid XML format.");
            //        _context.Error.Add(error415);
            //        await _context.SaveChangesAsync();
            //        return StatusCode(415, error415.Message);
            //    }
            //}
            //else if (Request.Headers.ContentType == "application/json")
            //{
            //    if (!(JsonSerializer.Deserialize<Organization>(organization.ToString()).GetType() == typeof(Immunization)))
            //    {
            //        Error error415 = new Error(415, "Content is not in valid Json format.");
            //        _context.Error.Add(error415);
            //        await _context.SaveChangesAsync();
            //        return StatusCode(415, error415.Message);
            //    }
            //}
            //else

            if (Request.Headers.ContentType != "application/xml" && Request.Headers.ContentType != "application/json")
            {
                Error error415 = new Error(415, "Content is not in XML or JSON format.");
                _context.Error.Add(error415);
                await _context.SaveChangesAsync();
                return StatusCode(415, error415.ToString());
            }

            if (id.Equals(null))
            {
                Error error400 = new Error(400, "Mandatory field missing: id.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            if (organization.Name == null || organization.Type == null || organization.Address == null)
            {
                Error error400 = new Error(400, "Mandatory field missing.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            if (!(organization.Type.ToLower().Equals("hospital") ||
                organization.Type.ToLower().Equals("clinic") ||
                organization.Type.ToLower().Equals("pharmacy")))
            {
                Error error400 = new Error(400, "Mandatory field missing: Type must be Hospital, Clinic, or Pharmacy.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            organization.Id = id;

            _context.Entry(organization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(id))
                {
                    Error error404 = new Error(404, $"Organization id {id} could not be found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404.ToString());
                }
                else
                {
                    Error error500 = new Error(500, "An unexpected error occurred.");
                    _context.Error.Add(error500);
                    await _context.SaveChangesAsync();
                    return StatusCode(500, error500.ToString());
                }
            }
            Error error200 = new Error(200, "The PUT operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, error200.ToString());
        }

        // GET: api/Organization?name="value"
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizationsByName(string name)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406.ToString());
            }

            if (name == null)
            {
                Error error400 = new Error(400, "Mandatory field missing: name.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            var organizations = from o in _context.Organization
                                where o.Name == name
                                select o;

            if (!organizations.Any())
            {
                Error error404 = new Error(404, $"No organizations named {name} were found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404.ToString());
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
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406.ToString());
            }

            if (type == null)
            {
                Error error400 = new Error(400, "Mandatory field missing: type.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400.ToString());
            }

            var organizations = from o in _context.Organization
                                where o.Type == type
                                select o;

            if (!organizations.Any())
            {
                Error error404 = new Error(404, $"No organizations of type {type} were found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404.ToString());
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, await organizations.ToListAsync());
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Organization>> DeleteOrganization(Guid id)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                _context.Error.Add(error406);
                await _context.SaveChangesAsync();
                return StatusCode(406, error406.ToString());
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                Error error404 = new Error(404, $"Organization id {id} could not be found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404.ToString());
            }

            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();

            Error error204 = new Error(204, "DELETE operation completed successfully.");
            _context.Error.Add(error204);
            await _context.SaveChangesAsync();
            return StatusCode(204, error204.ToString());
        }

        private bool OrganizationExists(Guid id)
        {
            return _context.Organization.Any(e => e.Id == id);
        }

        private byte[] SerializeToXml<T>(T instance)
        {
            // type can't be null
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var serializer = new XmlSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            serializer.Serialize(memoryStream, instance);
            var serializedContent = memoryStream.ToArray();

            return serializedContent;
        }

        private T DeserializeFromXml<T>(byte[] instance)
        {
            // instance can't be null
            if (instance == null)
                throw new ArgumentNullException("Instance can't be null");

            var serializer = new XmlSerializer(typeof(T));
            var deserializedContent = (T)serializer.Deserialize(new MemoryStream(instance))!;

            if (deserializedContent == null)
                throw new ArgumentNullException("Object could not be deserialized, check type");

            return deserializedContent;
        }
    }
}