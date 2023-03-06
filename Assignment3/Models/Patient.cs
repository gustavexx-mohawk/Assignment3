using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Patient : Person
    {
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
