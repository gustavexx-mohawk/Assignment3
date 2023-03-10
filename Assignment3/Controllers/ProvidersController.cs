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
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public ProvidersController(Assignment3Context context)
        {
            _context = context;
        }

        // GET: api/Providers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProvider()
        {
            return await _context.Provider.ToListAsync();
        }

        // GET: api/Providers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProvider(Guid id)
        {
            var provider = await _context.Provider.FindAsync(id);

            if (provider == null)
            {
                return NotFound();
            }

            return provider;
        }

        // PUT: api/Providers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvider(Guid id, Provider provider)
        {
            if (id != provider.Id)
            {
                return BadRequest();
            }

            _context.Entry(provider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderExists(id))
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

        // POST: api/Providers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Error>> PostProvider(Provider provider)
        {
            if (Request.Headers.ContentType == "application/xml" || Request.Headers.ContentType == "application/json")
            {
                if (provider.Id != null && provider.FirstName != null && provider.LastName != null && provider.LicenseNumber != null
                && provider.CreationTime != null)
                {
                    _context.Provider.Add(provider);
                    await _context.SaveChangesAsync();
                    var result = CreatedAtAction("GetProvider", new { id = provider.Id }, provider);

                    if (result.StatusCode == 201)
                    {
                        return new Error(201, "POST operation completed successfully.");
                    }
                    else
                    {
                        return new Error(500, "An Unexpected Error Occured.");
                    }
                }
                else
                {
                    return new Error(400, "Mandatory Field Missing.");
                }
            }
            else
            {
                return new Error(406, "HTTP Accept header is invalid.");
            }
        }

        // DELETE: api/Providers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(Guid id)
        {
            var provider = await _context.Provider.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }

            _context.Provider.Remove(provider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProviderExists(Guid id)
        {
            return _context.Provider.Any(e => e.Id == id);
        }
    }
}