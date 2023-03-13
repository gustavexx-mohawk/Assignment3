/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: To represent a person in a health care server application.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement.
*/
using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    /// <summary>
    /// This class represents a person.
    /// </summary>
    public class Person : Entity
    {
        /// <value>
        /// The FirstName of a Person.
        /// </value>
        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        /// <value>
        /// The LastName of a Person.
        /// </value>
        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

    }
}
