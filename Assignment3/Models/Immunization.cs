using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Assignment3.Models
{
    public class Immunization : Entity
    {
        /// <summary>
        /// Offcial Name of the immunization
        /// </summary>
        [Required, StringLength(128)]
        public string? OfficialName { get; set; }

        /// <summary>
        /// Trade Name of the immunization
        /// </summary>
        [StringLength(128)]
        public string? TradeName { get; set; }

        /// <summary>
        /// Lot Number of the immunization
        /// </summary>
        [Required, StringLength(255)]
        public string? LotNumber { get; set; }

        /// <summary>
        /// Expiration Date of the immunization
        /// </summary>
        [Required]
        public DateTimeOffset? ExpirationDate { get; set; }

        /// <summary>
        /// Updated Time of the immunization
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public DateTimeOffset? UpdatedTime { get; set; }

        public Immunization()
        {
            
            UpdatedTime = DateTimeOffset.Now;
            
        }

        public override string ToString() => JsonSerializer.Serialize(this);

    }
}
