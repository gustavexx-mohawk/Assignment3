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
    public class PatientsController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public PatientsController(Assignment3Context context)
        {
            _context = context;
        }

        // GET: api/Patients?firstname={value}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientBtFirstName(string firstName)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patients = from p in _context.Patient
                               where p.FirstName.ToLower().Equals(firstName.ToLower())
                               select p;

                if (patients == null)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return StatusCode(404, $"Sorry, {firstName} is not in our patient. Error Code: {StatusCode(404).StatusCode}  " +
                        $"the requested resource does not exist on the server.");
                }

                return await patients.ToListAsync();
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}  the client has indicated with Accept headers that \n" +
                    $"it will not accept any of the available representations of the resource");
            }            
        }

        // GET: api/Patients?lastName={value}
        [HttpGet("lastName")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientBtLastName(string lastName)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patients = from p in _context.Patient
                               where p.LastName.ToLower().Equals(lastName.ToLower())
                               select p;

                if (patients == null)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return StatusCode(404, $"Sorry, {lastName} is not in our patient. Error Code: {StatusCode(404).StatusCode}  " +
                        $"the requested resource does not exist on the server.");
                }

                return await patients.ToListAsync();
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}  the client has indicated with Accept headers that \n" +
                    $"it will not accept any of the available representations of the resource");
            }
        }

        // GET: api/Patients?DateOfBirth={value}
        [HttpGet("DateOfBirth")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientByDOB(string dateOfBirth)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patients = from p in _context.Patient
                               where p.DateOfBirth.Equals(dateOfBirth)
                               select p;

                if (patients == null)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return StatusCode(404, $"Sorry, {dateOfBirth} is match in our patients. Error Code: {StatusCode(404).StatusCode}  " +
                        $"the requested resource does not exist on the server.");
                }
                return await patients.ToListAsync();
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}  the client has indicated with Accept headers that \n" +
                    $"it will not accept any of the available representations of the resource");
            }
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patient = await _context.Patient.FindAsync(id);

                if (patient == null)
                {
                    // 404 NotFound: the requested resoure does not exist on the server
                    return StatusCode(404, $"Sorry, {id} is not in our database. Please try the other id");
                }

                return patient;
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}");
            }
            
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(Guid id, Patient patient)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                if (id != patient.Id)
                {
                    // 400 Bad Request
                    return StatusCode(400, $"Sorry, {id} is invalid. Error Code: {StatusCode(400).StatusCode}");
                }

                _context.Entry(patient).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(id))
                    {
                        return StatusCode(404, $"Sorry, {id} is invalid. Please try the other id");
                    }
                    else
                    {
                        throw;
                    }
                }
                return StatusCode(204, $"{id} is not here. Please try the other id Error Code: {StatusCode(204).StatusCode}");
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}  the client has indicated with Accept headers that \n" +
                    $"it will not accept any of the available representations of the resource");
            }
            
        }

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Error>> PostPatient(Patient patient)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                if (Request.Headers.ContentType == "application/xml" || Request.Headers.ContentType == "application/json")
                {
                    if (patient.Id != null && patient.FirstName != null && patient.LastName != null && patient.CreationTime != null)
                    {
                        _context.Patient.Add(patient);
                        await _context.SaveChangesAsync();
                        var result = CreatedAtAction("GetProvider", new { id = patient.Id }, patient);

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
                    return new Error(415, "Content is not in XML or JSON format.");                    
                }
            }
            else
            {
                return new Error(406, "HTTP Accept header is invalid.");
            }
            
            
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patient = await _context.Patient.FindAsync(id);
                if (patient == null)
                {
                    return StatusCode(404, $"Sorry, {id} is not in our database. Please try the other id");
                }

                _context.Patient.Remove(patient);
                await _context.SaveChangesAsync();

                return StatusCode(204, $"{id} is not here. Please try the other id Error Code: {StatusCode(204).StatusCode}");
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}  the client has indicated with Accept headers that \n" +
                    $"it will not accept any of the available representations of the resource");
            }            
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }
    }
}
