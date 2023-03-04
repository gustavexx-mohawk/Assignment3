using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Patient
    {

        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTimeOffset CreationTime { get; set; }
        [Required, StringLength(128)]
        public string FirstName { get; set; }
        [Required, StringLength(128)]
        public string LastName { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
