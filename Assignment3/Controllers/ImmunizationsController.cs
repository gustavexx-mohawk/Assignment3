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
using System.Text;

namespace Assignment3.Controllers
{
    [Route("/Immunization")]
    [ApiController]
    public class ImmunizationsController : ControllerBase
    {
        private readonly Assignment3Context _context;

        public ImmunizationsController(Assignment3Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets list of immunizations records
        /// </summary>
        /// <returns>A list of all the immunizations records present in the DB</returns>
        // GET: /Immunization
        [HttpGet]
        private async Task<ActionResult<IEnumerable<Immunization>>> GetImmunization()
        {
            return await _context.Immunization.ToListAsync();
        }

        /// <summary>
        /// Gets an immunization record from the DB based on the id
        /// </summary>
        /// <param name="immunizationId">Id of the immunization in format Guid, Immunization/B93955E2-0555-4261-B792-0AA0225FBFC2</param>
        /// <returns>A Immunization object</returns>
        // GET: Immunization/B93955E2-0555-4261-B792-0AA0225FBFC2
        [HttpGet("{immunizationId}")]
        public async Task<ActionResult<Immunization>> GetImmunizationByImmunizationIdAsync(Guid immunizationId)
        {
            try
            {

                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                    _context.Error.Add(error406);
                    await _context.SaveChangesAsync();
                    return StatusCode(406, error406);
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }
                Guid guidOutput;
                bool isValid = Guid.TryParse(immunizationId.ToString(), out guidOutput);
                if (!isValid)
                {
                    Error error400 = new Error(400, "Invalid immunizationId.");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                    
                }
                var immunization = await _context.Immunization.FindAsync(immunizationId);

                if (immunization == null)
                {
                    Error error404 = new Error(404, $"ImmunizationId :  {immunizationId} was not found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404);
                }

                Error error200 = new Error(200, "The GET operation completed successfully.");
                _context.Error.Add(error200);
                await _context.SaveChangesAsync();
                return StatusCode(200, immunization);

            }

            catch (Exception)
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }
        }

        /// <summary>
        /// Gets an immunization list from the DB based on the creation time
        /// </summary>
        /// <param name="creationTime">Creation time, format should be: 2023-03-11T17:50:24.0444629</param>
        /// <returns>List of immunization with the created time</returns>
        // GET: Immunization?creationTime="2023-03-11T17:50:24.0444629"
        [HttpGet("creationTime")]
        public async Task<ActionResult<Immunization>> GetImmunizationByCreatedDateAsync(DateTimeOffset creationTime)
        {
            try
            {
                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                    _context.Error.Add(error406);
                    await _context.SaveChangesAsync();
                    return StatusCode(406, error406);
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }
                DateTimeOffset dateTimeOffsetOutput;
                bool isValid = DateTimeOffset.TryParse(creationTime.ToString(), out dateTimeOffsetOutput);
                List<Immunization>? immunization = await (from e in _context.Immunization
                                                          where e.CreationTime == creationTime
                                                          select e).ToListAsync();

                if (!isValid || immunization.Count == 0)
                {
                    Error error400 = new Error(400, "No record for this date time for creation.");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                }

                

                if (immunization == null)
                {
                    Error error404 = new Error(404, $"Immunization with Creation Time {creationTime} was not found found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404);
                }

                Error error200 = new Error(200, "The GET operation completed successfully.");
                _context.Error.Add(error200);
                await _context.SaveChangesAsync();
                return StatusCode(200,  immunization);
            }
            catch (Exception) {

                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }

        }

        /// <summary>
        /// Gets an immunization list from the DB based on the official name
        /// </summary>
        /// <param name="officialName">Official name of the immunization</param>
        /// <returns>List of immunization with the that official name</returns>
        [HttpGet("officialName")]
        public async Task<ActionResult<Immunization>> GetImmunizationByOfficialNameAsync(string officialName)
        {
            try {

                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                    _context.Error.Add(error406);
                    await _context.SaveChangesAsync();
                    return StatusCode(406, error406);
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                bool isValid = !string.IsNullOrEmpty(officialName);
                if (!isValid)
                {
                    Error error400 = new Error(400, "No record for that Official name.");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                }

                List<Immunization>? immunization = await (from e in _context.Immunization
                                                          where e.OfficialName == officialName
                                                          select e).ToListAsync();

                if (immunization == null || immunization.Count == 0)
                {
                    Error error404 = new Error(404, $"Immunization with Official Name {officialName} could not be found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404);
                }

                Error error200 = new Error(200, "The Operation completed successfully.");
                _context.Error.Add(error200);
                await _context.SaveChangesAsync();
                return StatusCode(200, immunization);
            }
            catch (Exception)
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }

        }

        /// <summary>
        /// Gets an immunization list from the DB based on the trade name
        /// </summary>
        /// <param name="tradeName">Trade name of the immunization</param>
        /// <returns>List of immunization with the trade name</returns>
        [HttpGet("tradeName")]
        public async Task<ActionResult<Immunization>> GetImmunizationByTradelNameAsync(string? tradeName)
        {
            try {

                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                    _context.Error.Add(error406);
                    await _context.SaveChangesAsync();
                    return StatusCode(406, error406);
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                bool isValid = !string.IsNullOrEmpty(tradeName);
                if (!isValid)
                {
                    Error error400 = new Error(400, "No record for that Trade name.");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                }

                List<Immunization>? immunization = await (from e in _context.Immunization
                                                          where e.TradeName == tradeName
                                                          select e).ToListAsync();

                if (immunization == null || immunization.Count == 0)
                {
                    Error error404 = new Error(404, $"Immunization with Trade Name: {tradeName} was not found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404);
                }

                Error error200 = new Error(200, "The operation completed successfully.");
                _context.Error.Add(error200);
                await _context.SaveChangesAsync();
                return StatusCode(200, immunization);
            }
            catch (Exception)
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }
        }


        /// <summary>
        /// Gets an immunization list from the DB based on the lot number
        /// </summary>
        /// <param name="lotNumber">Lot number of the immunization</param>
        /// <returns>List of immunization with the lot number</returns>
        [HttpGet("lotNumber")]
        public async Task<ActionResult<Immunization>> GetImmunizationByLotNumberAsync(string lotNumber)
        {
            try 
            {
                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                    _context.Error.Add(error406);
                    await _context.SaveChangesAsync();
                    return StatusCode(406, error406);
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                bool isValid = !string.IsNullOrEmpty(lotNumber);
                if (!isValid)
                {
                    Error error400 = new Error(400, "Mandatory field missing: lotNumber.");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                }

                List<Immunization>? immunization = await (from e in _context.Immunization
                                                          where e.LotNumber == lotNumber
                                                          select e).ToListAsync();

                if (immunization == null || immunization.Count > 0)
                {
                    Error error404 = new Error(404, $"Immunization Lot Number {lotNumber} was not found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404);
                }

                Error error200 = new Error(200, "The operation completed successfully.");
                _context.Error.Add(error200);
                await _context.SaveChangesAsync();
                return StatusCode(200, immunization);
            }
            
            catch (Exception)
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }

        }


        /// <summary>
        /// Updates a record from the DB based on the id of the immunization
        /// </summary>
        /// <param name="immunizationId">Id of the immunization</param>
        /// <param name="immunization">Immunization object (json) with the updates applied</param>
        /// <returns>Status code of the server response</returns>
        // PUT: api/Immunizations/3F0D0E7E-370D-4980-AD50-003271D2C117
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{immunizationId}")]
        public async Task<IActionResult> PutImmunization(Guid immunizationId, Immunization immunization)
        {
            try 
            {
                // if the Content-Type application/json
                if (Request.Headers.ContentType == "application/json")
                {
                    // if the body of the request is not a valid json
                    if (!((JsonSerializer.Deserialize<Immunization>(immunization.ToString())).GetType() == typeof(Immunization)))
                    {
                        Error error415 = new Error(415, "Content is not in XML or JSON format.");
                        _context.Error.Add(error415);
                        await _context.SaveChangesAsync();
                        return StatusCode(415, error415);
                    }
                }

                // if the Content-Type application/xml
                if (Request.Headers.ContentType == "application/xml")
                {
                    // if the body of the request is not a valid xml
                    byte[] xmlimmunization = SerializeToXml<Immunization>(immunization);
                    Immunization xmlDeserializedImmunization = DeserializeFromXml<Immunization>(xmlimmunization);
                    if (!(xmlDeserializedImmunization.GetType() == typeof(Immunization)))
                    {
                        Error error415 = new Error(415, "Content is not in XML or JSON format.");
                        _context.Error.Add(error415);
                        await _context.SaveChangesAsync();
                        return StatusCode(415, error415);
                    }
                }
                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                    _context.Error.Add(error406);
                    await _context.SaveChangesAsync();
                    return StatusCode(406, error406);
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                Guid guidOutput;
                bool isValid = Guid.TryParse(immunizationId.ToString(), out guidOutput);
                if (!isValid)
                {
                    Error error400 = new Error(400, "Invalid immunizationId or mandatory field missing.");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                }



                Immunization? immunizationObj = await _context.Immunization.FirstOrDefaultAsync(a => a.Id == immunizationId);

                if (immunization == null && immunizationObj == null)
                {
                    Error error404 = new Error(404, $"ImmunizationId :  {immunizationId} was not found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404);
                }
                immunizationObj!.OfficialName = immunization!.OfficialName;
                immunizationObj.TradeName = immunization.TradeName;
                immunizationObj.LotNumber = immunization.LotNumber;
                immunizationObj.ExpirationDate = immunization.ExpirationDate;

                await _context.SaveChangesAsync();

                Error error202 = new Error(202, "The PUT operation completed successfully.");
                _context.Error.Add(error202);
                await _context.SaveChangesAsync();
                return StatusCode(202, error202);

            }
            catch (Exception)
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }
        }

        // POST: api/Immunization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new Immunization record
        /// </summary>
        /// <param name="immunization"> Immonization object</param>
        /// <returns>Status code of the server response</returns>
        [HttpPost]
        public async Task<ActionResult<Error>> PostImmunization(Immunization immunization)
        {
            try
            {
                DateTimeOffset invaliDateFlag = new DateTimeOffset();
                DateTimeOffset.TryParse("0001-01-01 12:00:00 AM +00:00", out invaliDateFlag);

                // If the date does not comply or is null or content is empty
                if (immunization.ExpirationDate.Equals(invaliDateFlag) ||
                    string.IsNullOrEmpty(immunization.ExpirationDate.ToString()) ||
                    immunization.OfficialName == null ||
                    immunization.LotNumber == null)
                {
                    Error error400 = new Error(400, "Mandatory field missing.");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                }


                // if the Content-Type application/json
                if (Request.Headers.ContentType == "application/json")
                {
                    // if the body of the request is not a valid json
                    if (!((JsonSerializer.Deserialize<Immunization>(immunization.ToString())).GetType() == typeof(Immunization)))
                    {
                        Error error415 = new Error(415, "Content is not in XML or JSON format.");
                        _context.Error.Add(error415);
                        await _context.SaveChangesAsync();
                        return StatusCode(415, error415);
                    }
                }

                // if the Content-Type application/xml
                if (Request.Headers.ContentType == "application/xml")
                {
                    // if the body of the request is not a valid xml
                    byte[] xmlimmunization = SerializeToXml<Immunization>(immunization);
                    Immunization xmlDeserializedImmunization = DeserializeFromXml<Immunization>(xmlimmunization);
                    if (!(xmlDeserializedImmunization.GetType() == typeof(Immunization)))
                    {
                        Error error415 = new Error(415, "Content is not in XML or JSON format.");
                        _context.Error.Add(error415);
                        await _context.SaveChangesAsync();
                        return StatusCode(415, error415);
                    }
                }

                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept)) {
                    Error error406 = new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json.");
                    _context.Error.Add(error406);
                    await _context.SaveChangesAsync();
                    return StatusCode(406, error406);
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                _context.Immunization.Add(immunization);
                await _context.SaveChangesAsync();

                var result = CreatedAtAction("GetImmunizationByImmunizationIdAsync", new { id = immunization.Id }, immunization);


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
            catch (Exception ex)
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }

        }

        /// <summary>
        /// Deletes a Immunization record based on the Id of the immunization
        /// </summary>
        /// <param name="immunizationId">id of the immunization</param>
        /// <returns>Status code of the server response</returns>
        // DELETE: api/Immunizations/7CC3C310-4F4A-432B-AC48-0AC354B217D8
        [HttpDelete("{immunizationId}")]
        public async Task<ActionResult<Error>> DeleteImmunization(Guid immunizationId)
        {
            try 
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

                Guid guidOutput;
                bool isValid = Guid.TryParse(immunizationId.ToString(), out guidOutput);
                if (!isValid)
                {
                    Error error400 = new Error(400, "Invalid immunizationId");
                    _context.Error.Add(error400);
                    await _context.SaveChangesAsync();
                    return StatusCode(400, error400);
                }

                var immunization = await _context.Immunization.FindAsync(immunizationId);

                if (immunization == null)
                {
                    Error error404 = new Error(404, $"Immunization with Id: {immunizationId} was not found.");
                    _context.Error.Add(error404);
                    await _context.SaveChangesAsync();
                    return StatusCode(404, error404);
                }

                _context.Immunization.Remove(immunization);
                await _context.SaveChangesAsync();

                Error error202 = new Error(202, "The DELETE operation completed successfully.");
                _context.Error.Add(error202);
                await _context.SaveChangesAsync();
                return StatusCode(202, error202);
            }

            catch (Exception )
            {
                Error error500 = new Error(500, "An unexpected error occurred.");
                _context.Error.Add(error500);
                await _context.SaveChangesAsync();
                return StatusCode(500, error500);
            }
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

