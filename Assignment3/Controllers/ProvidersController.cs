/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: A controller for Provider that determines what response to send back to a user when a HTTP request is made.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement.
*/
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

    /// <summary>
    /// This class represents a controller for Provider that determines what response to send back to a user when a HTTP request is made.
    /// </summary>
    [Route("/Provider")]
[ApiController]
public class ProvidersController : ControllerBase
{
    /// <value>
    /// The DbContext for the health care server application.
    /// </value>
    private readonly Assignment3Context _context;

    /// <summary>
    /// A constructor for ProviderController that sets its Assignment3Context.
    /// </summary>
    /// <param name="context">The Assignment3Context of teh health care server application.</param>
    public ProvidersController(Assignment3Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a single Provider record by the Provider id.
    /// </summary>
    /// <param name="id">The id of the requested provider.</param>
    /// <returns>The record in XML or JSON if it is found in the database, otherwise a appropriate HTTP StatusCode with a Error object in XML or JSON.</returns>
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

    /// <summary>
    /// Retrieves all Provider records that match the name provided.
    /// </summary>
    /// <param firstName="firstName">The name of the requested provider.</param>
    /// <returns>The records in XML or JSON any records in the database match the provided name, otherwise a appropriate HTTP StatusCode with a Error object in XML or JSON.</returns>
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

    /// <summary>
    /// Retrieves all Provider records that match the type provided.
    /// </summary>
    /// <param lastName="lastName">The type of the requested provider.</param>
    /// <returns>The records in XML or JSON any records in the database match the provided type, otherwise a appropriate HTTP StatusCode with a Error object in XML or JSON.</returns>
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

    /// <summary>
    /// Updates a provider record.
    /// </summary>
    /// <param name="id">The id of the requested provider.</param>
    /// <param name="provider">The incoming Provider.</param>
    /// <returns>An appropriate HTTP StatusCode with a Error object in XML or JSON.</returns>
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

    /// <summary>
    /// Creates a provider record.
    /// </summary>
    /// <param name="provider">The incoming Provider.</param>
    /// <returns>An appropriate HTTP StatusCode with a Error object in XML or JSON.</returns>
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

    /// <summary>
    /// Deletes a provider record by the provider id.
    /// </summary>
    /// <param name="id">The id of the provider to be deleted.</param>
    /// <returns>An appropriate HTTP StatusCode with a Error object in XML or JSON.</returns>
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

    /// <summary>
    /// Determines whether a Provider is in the database.
    /// </summary>
    /// <param name="id">The id of the provider.</param>
    /// <returns>True if the provider is in the database, false otherwise.</returns>
    private bool ProviderExists(Guid id)
    {
        return _context.Provider.Any(e => e.Id == id);
    }

}
}