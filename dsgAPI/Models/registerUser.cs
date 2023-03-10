using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dsgAPI.Models
{
    [Keyless]
    public class registerUser
    {
    
        public string email { get; set; }
        public string password { get; set; }
    }
}
