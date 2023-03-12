using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment3.Data;
using Assignment3.Models;
using System.Xml.Linq;
using System.Diagnostics;

namespace Assignment3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public PatientsController(Assignment3Context context)
        {
            _context = context;
        }

        // POST: Patients
        // Creates a patient record
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
                        var result = CreatedAtAction("GetPatient", new { id = patient.Id }, patient);

                        if (result.StatusCode == 201)
                        {
                            return new Error(201, $"Success\nPOST operation completed successfully.");
                        }
                        else
                        {
                            return new Error(500, $"Error code: 500\nAn Unexpected Error Occured.");
                        }
                    }
                    else
                    {
                        return new Error(400, $"Error code: 400\nMandatory Field Missing.");
                    }
                }
                else
                {
                    return new Error(415, $"Error code: 415\nContent is not in XML or JSON format.");
                }
            }
            else
            {
                return new Error(406, $"Error code: 406\nHTTP Accept header is invalid.");
            }
        }

        // GET: Patients/5
        // Retrieves a single patient record by the patient id.
        [HttpGet("{patientId}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid patientId)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patient = await _context.Patient.FindAsync(patientId);

                if (patient == null)
                {
                    // 404 NotFound: the requested resoure does not exist on the server
                    return StatusCode(404, $"Sorry, {patientId} is not in our database. Please try the other id\n\n" +
                        $"{new Error(404, $"Patient id: {patientId} could not be found.")}");
                }

                return patient;
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}");
            }
        }

        // PUT: Patients/5
        // Updates a patient record.
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{patientId}")]
        public async Task<ActionResult<Error>> PutPatient(Guid patientId, Patient patient)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                patient.Id = patientId;

                if (patientId != patient.Id)
                {
                    // 400 Bad Request
                    return StatusCode(400, $"Sorry, {patientId} is invalid. Error Code: {StatusCode(400).StatusCode}\n" +
                        $"{new Error(400, "Please input valid id")}");
                }
                
                _context.Entry(patient).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patientId))
                    {
                        return StatusCode(404, $"Sorry, {patientId} is invalid. Please try the other id");
                    }
                    else
                    {
                        return StatusCode(204, $"{patientId} is not here. Please try the other id Error Code: {StatusCode(204).StatusCode}");
                    }
                }
                return StatusCode(201, $"{patientId} has been updated");
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}  the client has indicated with Accept headers that \n" +
                    $"it will not accept any of the available representations of the resource");
            }
        }

        // GET: Patients?firstname={value}
        // Retrieves all patients that match the first name provided.        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientByFirstName(string firstName)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patients = from p in _context.Patient
                               where p.FirstName.ToLower().Equals(firstName.ToLower())
                               select p;

                if (patients == null)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return StatusCode(404, $"Sorry, {firstName} is not in our patient. Error Code: {StatusCode(404).StatusCode}" +
                        $"The requested resource does not exist on the server.");
                }

                return await patients.ToListAsync();
            }
            else
            {
                return StatusCode(406, $"Sorry, The HTTP Accept header is invalid. Error Code: {StatusCode(406).StatusCode}\nThe client has indicated with Accept headers that \n" +
                    $"it will not accept any of the available representations of the resource");
            }            
        }

        // GET: Patients?lastName={value}
        // Retrieves all patients that match the last name provided. 
        [HttpGet("lastName")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientByLastName(string lastName)
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

        // GET: Patients?DateOfBirth={value}
        // Retrieves all patients that match the date of birth provided. 
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
        

        // DELETE: Patients/5
        // Deletes a patient record by the patient id.
        [HttpDelete("{patientId}")]
        public async Task<IActionResult> DeletePatient(Guid patientId)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patient = await _context.Patient.FindAsync(patientId);
                if (patient == null)
                {
                    return StatusCode(404, $"Sorry, {patientId} is not in our database. Please try the other id");
                }

                _context.Patient.Remove(patient);
                await _context.SaveChangesAsync();

                return StatusCode(204, $"{patientId} is not here. Please try the other id Error Code: {StatusCode(204).StatusCode}");
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
