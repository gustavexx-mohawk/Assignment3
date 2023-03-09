using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            if (Request.Headers.ContentType == "application/xml" || Request.Headers.ContentType == "application/json")
            {
               if (organization.Id != null && organization.CreationTime != null && organization.Name != null 
                    && organization.Type != null && organization.Address != null)
                {
                    if (organization.Type.ToLower() == "Hospital" || 
                        organization.Type.ToLower() == "Clinic" ||
                        organization.Type.ToLower() == "Pharmacy")
                    {
                        _context.Organization.Add(organization);
                        await _context.SaveChangesAsync();

                        var result = CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);

                        if (result.StatusCode == 201)
                        {
                            return new Error(201, "POST operation completed successfully.");
                        }
                        // TODO: Put the thing here for verifying the format is xml or json
                        // Check what starts with? Try to deserialize?
                        else
                        {
                            return new Error(500, "An unexpected error occurred.");
                        }
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
