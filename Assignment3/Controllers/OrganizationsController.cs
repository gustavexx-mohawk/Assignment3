using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment3.Data;
using Assignment3.Models;

namespace Assignment3.Controllers
{
    [Route("api/Organization")]
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
        public async Task<ActionResult<Error>> PostOrganization(Organization organization)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                if (Request.Headers.ContentType == "application/xml" || Request.Headers.ContentType == "application/json")
                {
                    if (organization.Name != null && organization.Type != null && organization.Address != null)
                    {
                        if (organization.Type.ToLower().Equals("hospital") ||
                            organization.Type.ToLower().Equals("clinic") ||
                            organization.Type.ToLower().Equals("pharmacy"))
                        {
                            if (!OrganizationExists(organization.Id))
                            {
                                _context.Organization.Add(organization);
                                await _context.SaveChangesAsync();

                                var result = CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);

                                if (result.StatusCode == 201)
                                {
                                    return new Error(201, "POST operation completed successfully.");
                                }

                            }
                            return new Error(500, "An unexpected error occurred.");
                        }
                        else
                        {
                            return new Error(400, "Mandatory field missing: Type must be Hospital, Clinic, or Pharmacy.");
                        }
                    }
                    else
                    {
                        return new Error(400, "Mandatory field missing.");
                    }
                }
                else
                {
                    return new Error(415, "Content is not in XML or JSON format.");
                }
            }
            else
            {
                return new Error(406, "HTTP Accept header is invalid.");
            }
        }

        // GET: api/Organization/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(Guid id)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var organization = await _context.Organization.FindAsync(id);

                if (organization == null)
                {
                    //return new Error(404, "Organization id " + id + " was not found.");
                    return StatusCode(404);
                }
                else
                {
                    //return new Error(200, "The GET operation completed successfully.");
                    return organization;
                }
            }
            else
            {
                //return new Error(406, "HTTP Accept header is invalid.");
                return StatusCode(406);
            }
        }

        // PUT: api/Organization/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Error>> PutOrganization(Guid id, Organization organization)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                if (Request.Headers.ContentType == "application/xml" || Request.Headers.ContentType == "application/json")
                {
                    if (organization.Name != null && organization.Type != null && organization.Address != null)
                    {
                        if (organization.Type.ToLower().Equals("hospital") ||
                            organization.Type.ToLower().Equals("clinic") ||
                            organization.Type.ToLower().Equals("pharmacy"))
                        {
                            if (id != organization.Id)
                            {
                                return new Error(400, "Mandatory field missing: Type must be Hospital, Clinic, or Pharmacy.");
                            }

                            _context.Entry(organization).State = EntityState.Modified;

                            try
                            {
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                if (!OrganizationExists(id))
                                {
                                    return new Error(404, "Organization id " + id + " could not be found.");
                                }
                                else
                                {
                                    // throw;
                                    return new Error(500, "An unexpected error occurred.");
                                }
                            }
                            return new Error(200, "The PUT operation completed successfully.");
                        }
                        else
                        {
                            return new Error(400, "Mandatory field missing: Type must be Hospital, Clinic, or Pharmacy.");
                        }
                    }
                    else
                    {
                        return new Error(400, "Mandatory field missing.");
                    }
                }
                else
                {
                    return new Error(415, "Content is not in XML or JSON format.");
                }
            }
            else
            {
                return new Error(406, "HTTP Accept header is invalid.");
            }
        }

        // GET: api/Organization?name="value"
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizationsByName(string name)
        { 
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var organizations = from o in _context.Organization
                                    where o.Name == name
                                    select o;

                //return new Error(200, "The GET operation completed successfully.");
                return await organizations.ToListAsync();
            }
            else
            {
                //return new Error(406, "HTTP Accept header is invalid.");
                return StatusCode(406);
            }
        }

        // GET: api/Organization?type="value"
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizationsByType(string type)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var organizations = from o in _context.Organization
                                    where o.Type == type
                                    select o;

                //return new Error(200, "The GET operation completed successfully.");
                return await organizations.ToListAsync();
            }
            else
            {
                //return new Error(406, "HTTP Accept header is invalid.");
                return StatusCode(406);
            }
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<Error> DeleteOrganization(Guid id)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var organization = await _context.Organization.FindAsync(id);
                if (organization == null)
                {
                    return new Error(404, "Organization id " + id + " was not found.");
                }

                _context.Organization.Remove(organization);
                await _context.SaveChangesAsync();

                return new Error(204, "DELETE operation completed successfully.");
            }
            else
            {
                return new Error(406, "HTTP Accept header is invalid.");
            }
        }

        private bool OrganizationExists(Guid id)
        {
            return _context.Organization.Any(e => e.Id == id);
        }
    }
}
