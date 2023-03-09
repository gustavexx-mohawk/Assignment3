using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Organization : Entity
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
