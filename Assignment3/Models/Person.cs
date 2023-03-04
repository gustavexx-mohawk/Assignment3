using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Person : Entity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
