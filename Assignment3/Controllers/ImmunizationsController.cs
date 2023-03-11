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
                string test = Request.Headers.Accept;

                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    return StatusCode(406, new Error(406, "Accept-Header Invalid, only 'aplication/json' or application/xml allowed"));// new Error(406, "Accept-Header Invalid, only 'aplication/json' or application/xml allowed");

                }


                _context.Immunization.Add(immunization);
                await _context.SaveChangesAsync();

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
    }
}
