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

        // POST: api/Organizations
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

        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetOrganization()
        {
            return await _context.Organization.ToListAsync();
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(Guid id)
        {
            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            return organization;
        }

        // PUT: api/Organizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization(Guid id, Organization organization)
        {
            if (id != organization.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(Guid id)
        {
            var organization = await _context.Organization.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }

            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganizationExists(Guid id)
        {
            return _context.Organization.Any(e => e.Id == id);
        }
    }
}
