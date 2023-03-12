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
    [Route("/Provider")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public ProvidersController(Assignment3Context context)
        {
            _context = context;
        }

        // GET: api/Provider/96857544-ee06-45c0-a704-0a04cac90ec7
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProvider(Guid id)
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

            var provider = await _context.Provider.FindAsync(id);

            if (provider == null)
            {
                Error error404 = new Error(404, "Provider id " + id + " was not found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, provider);
        }

        // GET: api/Provider?name="value"
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProvidersByFirstName(string firstName)
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

            if (firstName == null)
            {
                Error error400 = new Error(400, "Mandatory field missing: name.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            var providers = from p in _context.Provider
                            where p.FirstName == firstName
                            select p;

            if (!providers.Any())
            {
                Error error404 = new Error(404, $"No providers named {firstName} were found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, await providers.ToListAsync());
        }

        // GET: api/Provider?lastName="value"
        [HttpGet("type")]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProvidersByLastName(string lastName)
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

            if (lastName == null)
            {
                Error error400 = new Error(400, "Mandatory field missing: lastName.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            var providers = from p in _context.Provider
                            where p.LastName == lastName
                            select p;

            if (!providers.Any())
            {
                Error error404 = new Error(404, $"No providers named {lastName} were found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            Error error200 = new Error(200, "The GET operation completed successfully.");
            _context.Error.Add(error200);
            await _context.SaveChangesAsync();
            return StatusCode(200, await providers.ToListAsync());
        }

        // PUT: api/Provider/96857544-ee06-45c0-a704-0a04cac90ec7
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Provider>> PutProvider(Guid id, Provider provider)
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

            if (provider.FirstName == null || provider.LastName == null || provider.LicenseNumber == null)
            {
                Error error400 = new Error(400, "Mandatory field missing.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (!ProviderExists(id))
            {
                Error error404 = new Error(404, $"Provider id {id} could not be found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            if (provider.Id != id)
            {
                Error error400 = new Error(400, "The provider id must match the id provided in the PUT body.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (provider.FirstName.Length > 128)
            {
                Error error400 = new Error(400, "Name must be 256 characters or less.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (provider.LastName.Length > 128)
            {
                Error error400 = new Error(400, "Name must be 128 characters or less.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            _context.Entry(provider).State = EntityState.Modified;

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

        // POST: api/Provider
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Provider>> PostProvider(Provider provider)
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

            if (string.IsNullOrEmpty(provider.FirstName) || string.IsNullOrEmpty(provider.LastName) || string.IsNullOrEmpty(provider.LicenseNumber.ToString()))
            {
                Error error400 = new Error(400, "Mandatory field missing.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (ProviderExists(provider.Id))
            {
                Error error400 = new Error(400, "Id already exists in the database.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (provider.FirstName.Length > 128)
            {
                Error error400 = new Error(400, "Name must be 128 characters or less.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            if (provider.LastName.Length > 128)
            {
                Error error400 = new Error(400, "Name must be 128 characters or less.");
                _context.Error.Add(error400);
                await _context.SaveChangesAsync();
                return StatusCode(400, error400);
            }

            _context.Provider.Add(provider);
            await _context.SaveChangesAsync();

            var result = CreatedAtAction("GetProvider", new { id = provider.Id }, provider);

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

        // DELETE: api/Providers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Provider>> DeleteProvider(Guid id)
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

            var provider = await _context.Provider.FindAsync(id);

            if (provider == null)
            {
                Error error404 = new Error(404, $"Provider id {id} could not be found.");
                _context.Error.Add(error404);
                await _context.SaveChangesAsync();
                return StatusCode(404, error404);
            }

            _context.Provider.Remove(provider);
            await _context.SaveChangesAsync();

            Error error204 = new Error(204, "DELETE operation completed successfully.");
            _context.Error.Add(error204);
            await _context.SaveChangesAsync();
            return StatusCode(204, error204);
        }

        private bool ProviderExists(Guid id)
        {
            return _context.Provider.Any(e => e.Id == id);
        }

    }
}