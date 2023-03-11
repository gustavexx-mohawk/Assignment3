using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment3.Data;
using Assignment3.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Serialization;

namespace Assignment3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImmunizationsController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public ImmunizationsController(Assignment3Context context)
        {
            _context = context;
        }

        // GET: api/Immunizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Immunization>>> GetImmunization()
        {
            return await _context.Immunization.ToListAsync();
        }

        // GET: api/Immunizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Immunization>> GetImmunization(Guid id)
        {
            var immunization = await _context.Immunization.FindAsync(id);

            if (immunization == null)
            {
                return NotFound();
            }

            return immunization;
        }

        // PUT: api/Immunizations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImmunization(Guid id, Immunization immunization)
        {
            if (id != immunization.Id)
            {
                return BadRequest();
            }

            _context.Entry(immunization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImmunizationExists(id))
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

        // POST: api/Immunizations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Error>> PostImmunization(Immunization immunization)
        {
            try
            {
                var test = immunization.ToString();
                //if (string.IsNullOrEmpty(test)) { 
                //    Debug.WriteLine($"test : {test}");
                // 
                // if the Content-Type application/json
                if (Request.Headers.ContentType == "application/json")
                {
                    // if the body of the request is a valid json
                    if (!((JsonSerializer.Deserialize<Immunization>(immunization.ToString())).GetType() == typeof(Immunization)))
                    {
                        return StatusCode(415, new Error(415, "Content must be a valid xml or json"));

                    }
                }

                if (Request.Headers.ContentType == "application/xml")
                {
                    // if the body of the request is a valid json
                    byte[] xmlimmunization =  SerializeToXml<Immunization>(immunization);
                    Immunization xmlDeserializedImmunization = DeserializeFromXml<Immunization>(xmlimmunization);
                    if (xmlDeserializedImmunization.GetType() == typeof(Immunization))
                    {
                        return StatusCode(415, new Error(415, "Content must be a valid xml or json"));

                    }
                }
                //}
                //if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "Accept-Header Invalid, only 'aplication/json' or application/xml allowed"));// new Error(406, "Accept-Header Invalid, only 'aplication/json' or application/xml allowed");
                }
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")) {
                    Request.Headers.Accept = "application/json";

                    _context.Immunization.Add(immunization);
                    await _context.SaveChangesAsync();

                    return StatusCode(201, new Error(201, "he POST operation completed successfully"));
                }
                




                //_context.Immunization.Add(immunization);
                //await _context.SaveChangesAsync();

                return CreatedAtAction("GetImmunization", new { id = immunization.Id }, immunization);
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        // DELETE: api/Immunizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImmunization(Guid id)
        {
            var immunization = await _context.Immunization.FindAsync(id);
            if (immunization == null)
            {
                return NotFound();
            }

            _context.Immunization.Remove(immunization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImmunizationExists(Guid id)
        {
            return _context.Immunization.Any(e => e.Id == id);
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
            var deserializedObject = (T)serializer.Deserialize(new MemoryStream(instance))!;

            if (deserializedObject == null)
                throw new ArgumentNullException("Object could not be deserialized, check type");

            return deserializedObject;
        }
    }
}
