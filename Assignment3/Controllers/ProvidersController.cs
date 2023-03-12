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
    [Route("api/Provider")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public ProvidersController(Assignment3Context context)
        {
            _context = context;
        }

        // POST: api/Provider
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Error>> PostProvider(Provider provider)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                if (Request.Headers.ContentType == "application/xml" || Request.Headers.ContentType == "application/json")
                {
                    if (provider.Id != null && provider.FirstName != null && provider.LastName != null
                        && provider.CreationTime != null && provider.LicenseNumber != null)
                    {
                        if (!ProviderExists(provider.Id))
                        {
                            _context.Provider.Add(provider);
                            await _context.SaveChangesAsync();

                            var result = CreatedAtAction("GetProvider", new { id = provider.Id }, provider);

                            if (result.StatusCode == 201)
                            {
                                return new Error(201, "POST operation completed successfully.");
                            }

                        }
                        return new Error(500, "An unexpected error occurred.");
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

        // GET: api/Provider/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProvider(Guid id)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var provider = await _context.Provider.FindAsync(id);

                if (provider == null)
                {
                    //return new Error(404, "Provider id " + id + " was not found.");
                    return StatusCode(404);
                }
                else
                {
                    //return new Error(200, "The GET operation completed successfully.");
                    return provider;
                }
            }
            else
            {
                //return new Error(406, "HTTP Accept header is invalid.");
                return StatusCode(406);
            }
        }

        // PUT: api/Provider/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Error>> PutProvider(Guid id, Provider provider)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                if (Request.Headers.ContentType == "application/xml" || Request.Headers.ContentType == "application/json")
                {
                    if (provider.Id != null && provider.FirstName != null && provider.LastName != null && provider.LicenseNumber != null
                        && provider.CreationTime != null)
                    {
                        _context.Entry(provider).State = EntityState.Modified;

                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ProviderExists(id))
                            {
                                return new Error(404, "Provider id " + id + " could not be found.");
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

        // GET: api/Provider?firstName="value"
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Provider>>> GetOrganizationsByName(string firstName)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var providers = from p in _context.Provider
                                    where p.FirstName == firstName
                                    select p;

                //return new Error(200, "The GET operation completed successfully.");
                return await providers.ToListAsync();
            }
            else
            {
                //return new Error(406, "HTTP Accept header is invalid.");
                return StatusCode(406);
            }
        }

        // GET: api/Provider?lastName="value"
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Provider>>> GetOrganizationsByType(string lastName)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var providers = from p in _context.Provider
                                    where p.LastName == lastName
                                    select p;

                //return new Error(200, "The GET operation completed successfully.");
                return await providers.ToListAsync();
            }
            else
            {
                //return new Error(406, "HTTP Accept header is invalid.");
                return StatusCode(406);
            }
        }

        // DELETE: api/Providers/5
        [HttpDelete("{id}")]
        public async Task<Error> DeleteProvider(Guid id)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var provider = await _context.Provider.FindAsync(id);
                if (provider == null)
                {
                    return new Error(404, "Provider id " + id + " was not found.");
                }

                _context.Provider.Remove(provider);
                await _context.SaveChangesAsync();

                return new Error(204, "DELETE operation completed successfully.");
            }
            else
            {
                return new Error(406, "HTTP Accept header is invalid.");
            }
        }

        private bool ProviderExists(Guid id)
        {
            return _context.Provider.Any(e => e.Id == id);
        }
    }
}