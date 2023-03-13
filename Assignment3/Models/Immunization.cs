/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: To represent an immunization in a health care server application.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement.
*/

using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Assignment3.Models
{
    /// <summary>
    /// This class represents an immunization in a health care server application.
    /// </summary>
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
