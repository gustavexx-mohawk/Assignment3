/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: To represent an organization in a health care server application.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement.
*/

using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    /// <summary>
    /// This class represents an organization.
    /// </summary>
    public class Organization : Entity
    {
        /// <value>
        /// The name of Organization.
        /// </value>
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        /// <value>
        /// The type of Organization.
        /// </value>
        [Required]
        public string Type { get; set; }

        /// <value>
        /// The address of Organization.
        /// </value>
        [Required]
        public string Address { get; set; }
    }
}
