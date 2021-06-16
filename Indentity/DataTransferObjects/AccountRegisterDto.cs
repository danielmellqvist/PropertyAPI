using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DataTransferObjects
{
    /// <summary>
    /// Dto used in the AccountsController for registering a user.
    /// </summary>
    public class AccountRegisterDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter an Email adress!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter the password again.")]
        public string ConfirmPassword { get; set; }
    }
}
