using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DataTransferObjects
{
    public class AccountLoginDto
    {
        [Required(ErrorMessage ="Please enter your Email adress!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password!")]
        public string Password { get; set; }
    }
}
