using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Assignment3.Models
{
    [XmlRoot]
    [XmlType]
    [JsonObject]
    [Serializable]
    public class Patient : Person
    {
        [Required]
        [XmlElement]
        [JsonProperty]
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
