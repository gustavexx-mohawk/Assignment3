using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment3.Data;
using Assignment3.Models;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

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
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }

            if (Request.Headers.ContentType == "application/xml")
            {
                byte[] xmlOrganization = SerializeToXml<Organization>(organization);
                Organization xmlDeserializedOrganization = DeserializeFromXml<Organization>(xmlOrganization);

                if (!(xmlDeserializedOrganization.GetType() == typeof(Organization)))
                {
                    return StatusCode(415, new Error(415, "Content is not in valid XML format."));
                }
            }
            else if (Request.Headers.ContentType == "application/json")
            {
                byte[] jsonOrganization = SerializeToJson<Organization>(organization);
                Organization jsonDeserializedOrganization = DeserializeFromJson<Organization>(jsonOrganization);

                if (!(jsonDeserializedOrganization.GetType() == typeof(Organization)))
                {
                    return StatusCode(415, new Error(415, "Content is not in valid Json format."));
                }
            }
            else
            {
                return StatusCode(415, new Error(415, "Content is not in XML or JSON format."));
            }

            if (organization.Name == null || organization.Type == null || organization.Address == null)
            {
                return StatusCode(400, new Error(400, "Mandatory field missing."));
            }

            if (!(organization.Type.ToLower().Equals("hospital") ||
                organization.Type.ToLower().Equals("clinic") ||
                organization.Type.ToLower().Equals("pharmacy")))
            {
                return StatusCode(400, new Error(400, "Mandatory field missing: Type must be Hospital, Clinic, or Pharmacy."));
            }

            _context.Organization.Add(organization);
            await _context.SaveChangesAsync();

            var result = CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);

            if (result.StatusCode == 201)
            {
                return StatusCode(201, new Error(201, "The POST operation completed successfully."));
            }

            return StatusCode(500, new Error(500, "An unexpected error occurred."));
        }

        // GET: api/Organization/96857544-ee06-45c0-a704-0a04cac90ec7
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(Guid id)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }

            if(id.Equals(null))
            {
                return StatusCode(400, new Error(400, "Mandatory field missing: id."));
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                return StatusCode(404, new Error(404, "Organization id " + id + " was not found."));
            }

            return StatusCode(200, organization);
        }

        // PUT: api/Organization/96857544-ee06-45c0-a704-0a04cac90ec7
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Error>> PutOrganization(Guid id, Organization organization)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }

            if (Request.Headers.ContentType == "application/xml")
            {
                byte[] xmlOrganization = SerializeToXml<Organization>(organization);
                Organization xmlDeserializedOrganization = DeserializeFromXml<Organization>(xmlOrganization);

                if (!(xmlDeserializedOrganization.GetType() == typeof(Organization)))
                {
                    return StatusCode(415, new Error(415, "Content is not in valid XML format."));
                }
            }
            else if (Request.Headers.ContentType == "application/json")
            {
                byte[] jsonOrganization = SerializeToJson<Organization>(organization);
                Organization jsonDeserializedOrganization = DeserializeFromJson<Organization>(jsonOrganization);

                if (!(jsonDeserializedOrganization.GetType() == typeof(Organization)))
                {
                    return StatusCode(415, new Error(415, "Content is not in valid Json format."));
                }
            }
            else
            {
                return StatusCode(415, new Error(415, "Content is not in XML or JSON format."));
            }

            if (id.Equals(null))
            {
                return StatusCode(400, new Error(400, "Mandatory field missing: id."));
            }

            if (organization.Name == null || organization.Type == null || organization.Address == null)
            {
                return StatusCode(400, new Error(400, "Mandatory field missing."));
            }

            if (!(organization.Type.ToLower().Equals("hospital") ||
                organization.Type.ToLower().Equals("clinic") ||
                organization.Type.ToLower().Equals("pharmacy")))
            {
                return StatusCode(400, new Error(400, "Mandatory field missing: Type must be Hospital, Clinic, or Pharmacy."));
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
                    return StatusCode(404, new Error(404, $"Organization id {id} could not be found."));
                }
                else
                {
                    // throw;
                    return StatusCode(500, new Error(500, "An unexpected error occurred."));
                }
            }

            return StatusCode(200, new Error(200, "The PUT operation completed successfully."));
        }

        // GET: api/Organization?name="value"
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizationsByName(string name)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }

            if (name == null)
            {
                return StatusCode(400, new Error(400, "Mandatory field missing: name."));
            }

            var organizations = from o in _context.Organization
                                where o.Name == name
                                select o;

            if (!organizations.Any())
            {
                return StatusCode(404, new Error(404, $"No organizations named {name} were found."));
            }

            return StatusCode(200, await organizations.ToListAsync());
        }

        // GET: api/Organization?type="value"
        [HttpGet("type")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizationsByType(string type)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }

            if (type == null)
            {
                return StatusCode(400, new Error(400, "Mandatory field missing: type."));
            }

            var organizations = from o in _context.Organization
                                where o.Type == type
                                select o;

            if (!organizations.Any())
            {
                return StatusCode(404, new Error(404, $"No organizations of type {type} were found."));
            }

            return StatusCode(200, await organizations.ToListAsync());
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Error>> DeleteOrganization(Guid id)
        {
            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }

            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                return StatusCode(404, new Error(404, $"Organization id {id} could not be found."));
            }

            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();

            return StatusCode(204, new Error(204, "DELETE operation completed successfully."));
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

        public static byte[] SerializeToJson<T>(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var serializer = new JsonSerializer();
            var stringWriter = new StringWriter();

            serializer.Serialize(stringWriter, instance);

            var serializedContent = Encoding.UTF8.GetBytes(stringWriter.ToString());

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

        public static T DeserializeFromJson<T>(byte[] instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Instance can't be null");
            }

            var serializer = new JsonSerializer();

            var jsonReader = new JsonTextReader(new StringReader(Encoding.UTF8.GetString(instance)));
            var deserializedContent = serializer.Deserialize(jsonReader, typeof(T));

            if (deserializedContent == null)
                throw new ArgumentNullException("Object could not be deserialized, check type");

            return (T)deserializedContent;
        }
    }
}

