using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Assignment3.Models
{
    public class Entity
    {
        [Key]
        [Required]
        [JsonIgnore]
        [XmlIgnore]
        public Guid Id { get; set; }

        [Required]
        [JsonIgnore]
        [XmlIgnore]
        public DateTimeOffset CreationTime { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTimeOffset.Now;
        }
    }
}