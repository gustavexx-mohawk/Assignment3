using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Organization
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTimeOffset CreationTime { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
