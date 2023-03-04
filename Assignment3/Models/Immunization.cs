using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Immunization:Entity
    {
        [Required, StringLength(128)]
        public string OfficialName { get; set; }
        [StringLength(128)]
        public string TradeName { get; set; }
        [Required, StringLength(255)]
        public string LotNumber { get; set; }
        [Required]
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
    }
}
