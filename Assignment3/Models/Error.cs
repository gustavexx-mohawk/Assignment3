/*
  Student names: Jongeun Kim, Gustavo Marcano Valero, Piper Sicard, and Amanda Venier.
  Student numbers: 000826393, 000812644, 000824338, 000764961
  Date: March 12, 2023

  Purpose: To represent an error in a health care server application.

  Statement of Authorship: We, Jongeun Kim (000826393), Gustavo Marcano Valero (000812644), Piper Sicard (000824338), and Amanda Venier (000764961) certify that this material is our original work.
                           No other person's work has been used without due acknowledgement.
*/

using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    /// <summary>
    /// This class represents an error in a health care server application.
    /// </summary>
    public class Error : Entity
    {
        /// <value>
        /// Message containing the issue that caused the error.
        /// </value>
        [Required]
        public string Message { get; set; }

        /// <value>
        /// The HTTP status code of the error (e.g., 400, 404, 500, etc).
        /// </value>
        [Required]
        public int StatusCode { get; set; }

        /// <summary>
        /// A constructor for Error that calls the base contructor and sets the message and status code.
        /// </summary>
        /// <param name="statusCode">The status code of Error.</param>
        /// <param name="message"></param>
        public Error(int statusCode, string message) : base()
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}