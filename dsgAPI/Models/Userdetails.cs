using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dsgAPI.Models
{
    public class Userdetails
    {
        [Required(ErrorMessage="User must enter email")]
        public string email { get; set; }
        [Required(ErrorMessage = "User must enter password")]
        public string password { get; set; }

    }
}
