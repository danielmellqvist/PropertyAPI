using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DataTransferObjects
{
    /// <summary>
    /// Dto used in the AccountsController for log in.
    /// </summary>
    public class AccountLoginDto
    {
        [Required(ErrorMessage ="Please enter your Email adress!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password!")]
        public string Password { get; set; }
    }
}
