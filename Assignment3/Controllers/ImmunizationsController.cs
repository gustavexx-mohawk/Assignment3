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

        // GET: /Immunization
        [HttpGet]
        private async Task<ActionResult<IEnumerable<Immunization>>> GetImmunization()
        {
            return await _context.Immunization.ToListAsync();
        }

        // GET: Immunization/B93955E2-0555-4261-B792-0AA0225FBFC2
        [HttpGet("{immunizationId}")]
        public async Task<ActionResult<Immunization>> GetImmunizationByImmunizationIdAsync(Guid immunizationId)
        {
            try
            {
                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
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
                    return StatusCode(400, new Error(400, "Invalid immunizationId."));
                }
                var immunization = await _context.Immunization.FindAsync(immunizationId);

                if (immunization == null)
                {
                    return StatusCode(404, new Error(404, "ImmunizationId: " + immunizationId + " was not found."));
                }

                return StatusCode(200, immunization);

            }

            catch (Exception)
            {
                return StatusCode(500, new Error(500, "Internal Server Error occurred"));
            }
        }


        // GET: Immunization?creationTime="2023-03-11T17:50:24.0444629"
        [HttpGet("creationTime")]
        public async Task<ActionResult<Immunization>> GetImmunizationByCreatedDateAsync(DateTimeOffset creationTime)
        {
            try
            {
                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
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
                    return StatusCode(400, new Error(400, "No record for this date time for creation."));
                }

                

                if (immunization == null)
                {
                    return StatusCode(404, new Error(404, "creationTime: " + creationTime + " was not found."));
                }

                return StatusCode(200, immunization);
            }
            catch (Exception) { 
                
                return StatusCode(500, new Error(500, "Internal Server Error occurred"));
            }

        }

        [HttpGet("officialName")]
        public async Task<ActionResult<Immunization>> GetImmunizationByOfficialNameAsync(string officialName)
        {
            try {

                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                bool isValid = !string.IsNullOrEmpty(officialName);
                if (!isValid)
                {
                    return StatusCode(400, new Error(400, "No record for that Official name."));
                }

                List<Immunization>? immunization = await (from e in _context.Immunization
                                                          where e.OfficialName == officialName
                                                          select e).ToListAsync();

                if (immunization == null)
                {
                    return StatusCode(404, new Error(404, "Official Name: " + officialName + " was not found."));
                }

                return StatusCode(200, immunization);
            }
            catch (Exception)
            {
                return StatusCode(500, new Error(500, "Internal Server Error occurred"));
            }

        }

        [HttpGet("tradeName")]
        public async Task<ActionResult<Immunization>> GetImmunizationByTradelNameAsync(string? tradeName)
        {
            try {

                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                bool isValid = !string.IsNullOrEmpty(tradeName);
                if (!isValid)
                {
                    return StatusCode(400, new Error(400, "No record for that Trade name."));
                }

                List<Immunization>? immunization = await (from e in _context.Immunization
                                                          where e.TradeName == tradeName
                                                          select e).ToListAsync();

                if (immunization == null)
                {
                    return StatusCode(404, new Error(404, "Trade Name: " + tradeName + " was not found."));
                }

                return StatusCode(200, immunization);
            }
            catch (Exception)
            {
                return StatusCode(500, new Error(500, "Internal Server Error occurred"));
            }
        }

        [HttpGet("lotNumber")]
        public async Task<ActionResult<Immunization>> GetImmunizationByLotNumberAsync(string lotNumber)
        {
            try 
            {
                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                bool isValid = !string.IsNullOrEmpty(lotNumber);
                if (!isValid)
                {
                    return StatusCode(400, new Error(400, "No record for that Trade name."));
                }

                List<Immunization>? immunization = await (from e in _context.Immunization
                                                          where e.LotNumber == lotNumber
                                                          select e).ToListAsync();

                if (immunization == null)
                {
                    return StatusCode(404, new Error(404, "Lot Number: " + lotNumber + " was not found."));
                }

                return StatusCode(200, immunization);
            }
            
            catch (Exception)
            {
                return StatusCode(500, new Error(500, "Internal Server Error occurred"));
            }

        }



        // PUT: api/Immunizations/3F0D0E7E-370D-4980-AD50-003271D2C117
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{immunizationId}")]
        public async Task<IActionResult> PutImmunization(Guid immunizationId, Immunization immunization)
        {
            try 
            {
                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
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
                    return StatusCode(400, new Error(400, "Invalid immunizationId."));
                }



                Immunization? immunizationObj = await _context.Immunization.FirstOrDefaultAsync(a => a.Id == immunizationId);

                if (immunization == null && immunizationObj == null)
                {
                    return StatusCode(404, new Error(404, "Id: " + immunizationId + " was not found."));
                }
                immunizationObj!.OfficialName = immunization!.OfficialName;
                immunizationObj.TradeName = immunization.TradeName;
                immunizationObj.LotNumber = immunization.LotNumber;
                immunizationObj.ExpirationDate = immunization.ExpirationDate;

                await _context.SaveChangesAsync();

                return StatusCode(202, new Error(202, $"Id: {immunizationId} was modified succesfully"));


            }
            catch (Exception)
            {
                return StatusCode(500, new Error(500, "Internal Server Error occurred"));
            }
        }

        // POST: api/Immunization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Sample of an XML request
        ///     <?xml version="1.0" encoding="UTF-8"?>
        ///         <Immunization>
        ///             <OfficialName>string</OfficialName>
        ///             <TradeName>string</TradeName>
        ///             <LotNumber>string</LotNumber>
        ///             <ExpirationDate>2023-03-12T15:43:48.527Z</ExpirationDate>
        ///         </Immunization>
        /// </summary>
        /// <param name="immunization"></param>
        /// <remarks>
        /// 
        /// Sample of a request
        ///     <?xml version="1.0" encoding="UTF-8"?>
        ///         <Immunization>
        ///             <OfficialName>string</OfficialName>
        ///             <TradeName>string</TradeName>
        ///             <LotNumber>string</LotNumber>
        ///             <ExpirationDate>2023-03-12T15:43:48.527Z</ExpirationDate>
        ///         </Immunization>
        /// </remarks>
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
                    return StatusCode(400, new Error(400, "Missing fields: 'ExpirationDate', 'OfficialName', 'LotNumber', 'UpdatedTime' cannot be missing"));
                }


                // if the Content-Type application/json
                if (Request.Headers.ContentType == "application/json")
                {
                    // if the body of the request is not a valid json
                    if (!((JsonSerializer.Deserialize<Immunization>(immunization.ToString())).GetType() == typeof(Immunization)))
                    {
                        return StatusCode(415, new Error(415, "Content must be a valid xml or json"));
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
                        return StatusCode(415, new Error(415, "Content must be a valid xml or json"));
                    }
                }

                // if there is no Accept-header
                if (string.IsNullOrEmpty(Request.Headers.Accept)) { 
                    
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
                }
                // if the Accept-header is invalid
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                _context.Immunization.Add(immunization);
                await _context.SaveChangesAsync();

                return StatusCode(201, new Error(201, "The POST operation completed successfully"));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new Error(500, "Unexpected error occured"));
            }

        }

        // DELETE: api/Immunizations/7CC3C310-4F4A-432B-AC48-0AC354B217D8
        [HttpDelete("{immunizationId}")]
        public async Task<ActionResult<Error>> DeleteImmunization(Guid immunizationId)
        {
            try 
            {
                if (string.IsNullOrEmpty(Request.Headers.Accept))
                {
                    return StatusCode(406, new Error(406, "HTTP Accept header is invalid. It must be application/xml or application/json."));
                }
                if (!(Request.Headers.Accept == "application/xml" || Request.Headers.Accept == "application/json"))
                {
                    Request.Headers.Accept = "application/json";
                }

                Guid guidOutput;
                bool isValid = Guid.TryParse(immunizationId.ToString(), out guidOutput);
                if (!isValid)
                {
                    return StatusCode(400, new Error(400, "Invalid immunizationId."));
                }

                var immunization = await _context.Immunization.FindAsync(immunizationId);

                if (immunization == null)
                {
                    return StatusCode(404, new Error(404, "Id: " + immunizationId + " was not found."));
                }

                _context.Immunization.Remove(immunization);
                await _context.SaveChangesAsync();

                return StatusCode(202, new Error(202, "The DELETE operation completed successfully"));
            }

            catch (Exception )
            {
                return StatusCode(500, new Error(500, "Unexpected error occured"));
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

