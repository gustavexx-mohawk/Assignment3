/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: To represent an patient in a health care server application.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), 
                           Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement. 
  
 */

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Assignment3.Models
{
    /// <summary>
    /// This class represents an patient.
    /// </summary>
    [XmlRoot]
    [XmlType]
    [JsonObject]
    [Serializable]
    public class Patient : Person
    {
        /// <value>
        /// The date of birth of patient
        /// </value>
        [Required]
        [XmlElement]
        [JsonProperty]
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
