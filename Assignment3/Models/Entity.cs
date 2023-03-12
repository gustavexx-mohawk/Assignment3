using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Entity
    {
        [Key]
        [Required]
        public Guid Id { get; private set; }

        [Required]
        public DateTimeOffset CreationTime { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTimeOffset.Now;
        }
    }
}