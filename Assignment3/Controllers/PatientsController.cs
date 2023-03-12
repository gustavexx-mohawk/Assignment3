﻿using System;
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
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;
using System.Data;

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
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {                
                await _context.SaveChangesAsync();
                return logErrorMsg(406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.ContentType != "application/xml" && Request.Headers.ContentType != "application/json")
            {                
                await _context.SaveChangesAsync();
                return logErrorMsg(415);
            }

            if (string.IsNullOrEmpty(patient.FirstName) || string.IsNullOrEmpty(patient.LastName) || PatientExists(patient.Id))
            {                
                await _context.SaveChangesAsync();
                return logErrorMsg(400);
            }

            try
            {
                _context.Patient.Add(patient);
                await _context.SaveChangesAsync();
                var result = CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
                return logErrorMsg(201);
            }
            catch (HttpRequestException e)
            {
                return logErrorMsg(500);
            }
        }       

        // PUT: Patients/5
        // Updates a patient record.
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{patientId}")]
        public async Task<ActionResult<Patient>> PutPatient(Guid patientId, Patient patient)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                await _context.SaveChangesAsync();
                return logErrorMsg(406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.ContentType != "application/xml" && Request.Headers.ContentType != "application/json")
            {
                await _context.SaveChangesAsync();
                return logErrorMsg(415);
            }

            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                patient.Id = patientId;
                _context.Entry(patient).State = EntityState.Modified;

                if (patientId != patient.Id)
                {
                    // 400 Bad Request
                    return logErrorMsg(400);
                }

                try
                {                
                    await _context.SaveChangesAsync();                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    return logErrorMsg(500);
                }
                return logErrorMsg(201);
            }
            else
            {
                return logErrorMsg(406);
            }
        }

        // GET: Patients/5
        // Retrieves a single patient record by the patient id.
        [HttpGet("{patientId}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid patientId)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                await _context.SaveChangesAsync();
                return logErrorMsg(406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patient = await _context.Patient.FindAsync(patientId);

                if (patient == null)
                {
                    // 404 NotFound: the requested resoure does not exist on the server
                    return logErrorMsg(404);
                }

                return patient;
            }
            else
            {
                return logErrorMsg(406);
            }
        }

        // GET: Patients?firstname={value}
        // Retrieves all patients that match the first name provided.        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                await _context.SaveChangesAsync();
                return logErrorMsg(406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patients = from p in _context.Patient
                               where p.FirstName.ToLower().Equals(firstName.ToLower())
                               select p;

                if (patients.Count() == 0)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return logErrorMsg(404);
                }

                return await patients.ToListAsync();
            }
            else
            {
                return logErrorMsg(406);
            }            
        }

        // GET: Patients?lastName={value}
        // Retrieves all patients that match the last name provided. 
        [HttpGet("lastName")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {
                await _context.SaveChangesAsync();
                return logErrorMsg(406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                var patients = from p in _context.Patient
                               where p.LastName.ToLower().Equals(lastName.ToLower())
                               select p;

                if (patients.Count() == 0)
                {
                    return logErrorMsg(404);
                }

                return await patients.ToListAsync();
            }
            else
            {
                return logErrorMsg(406);
            }
        }

        // GET: Patients?DateOfBirth={value}
        // Retrieves all patients that match the date of birth provided. 
        [HttpGet("DateOfBirth")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientByDOB(string dateOfBirth)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {                
                return logErrorMsg(406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }

            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {                
                var patients = from p in _context.Patient
                               where p.DateOfBirth.Date.Equals(DateTimeOffset.Parse(dateOfBirth).Date)
                               select p;

                if (patients.Count() == 0)
                {                    
                    return logErrorMsg(404);
                }
                return await patients.ToListAsync();
            }
            else
            {
                return logErrorMsg(406);
            }
        }
        

        // DELETE: Patients/5
        // Deletes a patient record by the patient id.
        [HttpDelete("{patientId}")]
        public async Task<ActionResult<Patient>> DeletePatient(Guid patientId)
        {
            if (string.IsNullOrEmpty(Request.Headers.Accept))
            {                
                return logErrorMsg(406);
            }

            if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
            {
                Request.Headers.Accept = "application/json";
            }
            
            
            if (!PatientExists(patientId))
            {
                return logErrorMsg(404);
            }
            var patient = await _context.Patient.FindAsync(patientId);
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return logErrorMsg(204);            
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }

        // log Error msg to a database
        private ActionResult logErrorMsg(int code)
        {
            var map = new Dictionary<int, string>();
            map.Add(200, "The operation completed successfully.");
            map.Add(201, "The POST operation completed successfully.");
            map.Add(204, "The DELETE operation completed successfully. ");
            map.Add(400, "If a mandatory field is missing. ");
            map.Add(404, "The resource was not found on the API");
            map.Add(405, "Incorrect HTTP verb on an endpoint.");
            map.Add(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
            map.Add(415, "Content is not in XML or JSON format.");
            map.Add(500, "An unexpected error occurs");
            string position = "Patient Controller|| ";
            Error error = new Error(code, position + map[code]);
            _context.Error.Add(error);
            _context.SaveChangesAsync();
            return StatusCode(code, error);
        }
    }
}
