using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Entity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTimeOffset CreationTime { get; set; }
    }
}
