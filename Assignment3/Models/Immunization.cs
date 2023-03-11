using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Assignment3.Models
{
    public class Immunization : Entity
    {
        [Required, StringLength(128)]
        public string OfficialName { get; set; }

        [StringLength(128)]
        public string? TradeName { get; set; }

        [Required, StringLength(255)]
        public string LotNumber { get; set; }

        [Required]
        [JsonIgnore]
        [XmlIgnore]
        public DateTimeOffset ExpirationDate { get; set; }

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
