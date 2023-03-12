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
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;

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
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                if (patient.FirstName == null || patient.LastName == null || patient.DateOfBirth == null)
                {
                    return StatusCode(400, new Error(400, "Mandatory field missing."));
                }

                if (Request.Headers.ContentType == "application/xml")
                {
                    byte[] xmlPatient = SerializeToXml<Patient>(patient);
                    Patient xmlDeserializedPatient = DeserializeFromXml<Patient>(xmlPatient);

                    if (!(xmlDeserializedPatient.GetType() == typeof(Patient)))
                    {
                        return StatusCode(415, new Error(415, "Content is not in valid XML format."));
                    }
                }
                else if (Request.Headers.Accept == "application/json")
                {
                    byte[] jsonPatient = SerializeToJson<Patient>(patient);
                    Patient jsonDeserializedPatient = DeserializeFromJson<Patient>(jsonPatient);

                    if (!(jsonDeserializedPatient.GetType() == typeof(Patient)))
                    {
                        return StatusCode(415, new Error(415, "Content is not in valid Json format."));
                    }
                }
                else
                {
                    return StatusCode(415, new Error(415, "Content is not in XML or JSON format."));
                }
                _context.Patient.Add(patient);
                await _context.SaveChangesAsync();
                var result = CreatedAtAction("GetPatient", new { id = patient.Id }, patient);

                if (result.StatusCode == 201)
                {
                    return StatusCode(201, new Error(201, "The POST operation completed successfully."));
                }
                else
                {
                    return StatusCode(500, new Error(500, "An unexpected error occurred."));
                }
            }
            else
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
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
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }
        }

        // PUT: Patients/5
        // Updates a patient record.
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{patientId}")]
        public async Task<ActionResult<Patient>> PutPatient(Guid patientId, Patient patient)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                patient.Id = patientId;
                _context.Entry(patient).State = EntityState.Modified;

                if (patientId != patient.Id)
                {
                    // 400 Bad Request
                    return StatusCode(400, $"Sorry, {patientId} is invalid. Error Code: {StatusCode(400).StatusCode}\n" +
                        $"{new Error(400, "Please input valid id")}");
                }

                try
                {                
                    await _context.SaveChangesAsync();                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patientId))
                    {
                        return StatusCode(404, new Error(404, $"Sorry, {patientId} is invalid. Please try the other id"));
                    }
                    else
                    {
                        return StatusCode(204, new Error(204, $"{patientId} is not here. Please try the other id Error Code: {StatusCode(204).StatusCode}"));
                    }
                }
                return StatusCode(201, new Error(201, $"Success {patientId} has been updated"));
            }
            else
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
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

                if (patients.Count() == 0)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return StatusCode(404, new Error(404, $"Sorry, {firstName} is not in our patient. Error Code: {StatusCode(404).StatusCode}" +
                        $"The requested resource does not exist on the server."));
                }

                return await patients.ToListAsync();
            }
            else
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
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

                if (patients.Count() == 0)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return StatusCode(404, new Error(404, $"Sorry, {lastName} is not in our DB. The requested resource does not exist on the server."));
                }

                return await patients.ToListAsync();
            }
            else
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
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
                               where p.DateOfBirth.Date.Equals(DateTimeOffset.Parse(dateOfBirth).Date)
                               select p;

                if (patients.Count() == 0)
                {
                    // 404: NotFound -> the requested resoure does not exist on the server
                    return StatusCode(404, new Error(404, $"Sorry, {dateOfBirth} is not match in our patients." +
                        $"the requested resource does not exist on the server."));
                }
                return await patients.ToListAsync();
            }
            else
            {
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }
        }
        

        // DELETE: Patients/5
        // Deletes a patient record by the patient id.
        [HttpDelete("{patientId}")]
        public async Task<ActionResult<Patient>> DeletePatient(Guid patientId)
        {
            if (Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json")
            {
                try
                {
                    var patient = await _context.Patient.FindAsync(patientId);
                    if (patient.Equals(null))
                    {
                        return StatusCode(404, new Error(404, $"Sorry, {patientId} is not in our database. Please try the other id"));
                    }

                    _context.Patient.Remove(patient);
                    await _context.SaveChangesAsync();
                    return StatusCode(204, "DELETE operation completed successfully.");
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
                return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
            }            
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patient.Any(e => e.Id == id);
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

        public static byte[] SerializeToJson<T>(T instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var serializer = new JsonSerializer();
            var stringWriter = new StringWriter();

            serializer.Serialize(stringWriter, instance);

            var serializedContent = Encoding.UTF8.GetBytes(stringWriter.ToString());

            return serializedContent;
        }

        private T DeserializeFromXml<T>(byte[] instance)
        {
            // instance can't be null
            if (instance == null)
                throw new ArgumentNullException("Instance can't be null");

            var serializer = new XmlSerializer(typeof(T));
            var deserializedContent = (T)serializer.Deserialize(new MemoryStream(instance))!;

            if (deserializedContent == null)
                throw new ArgumentNullException("Object could not be deserialized, check type");

            return deserializedContent;
        }

        public static T DeserializeFromJson<T>(byte[] instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Instance can't be null");
            }

            var serializer = new JsonSerializer();

            var jsonReader = new JsonTextReader(new StringReader(Encoding.UTF8.GetString(instance)));
            var deserializedContent = serializer.Deserialize(jsonReader, typeof(T));

            if (deserializedContent == null)
                throw new ArgumentNullException("Object could not be deserialized, check type");

            return (T)deserializedContent;
        }
    }
}
