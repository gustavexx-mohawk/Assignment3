using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Person : Entity
    {
        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

    }
}
