using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dsgAPI.Models
{
    public class clientContext: DbContext
    {
        public clientContext(DbContextOptions<clientContext> options)
           : base(options)
        {
        }
        public DbSet<dsgAPI.Models.Contacts> Contacts { get; set; }
        public DbSet<dsgAPI.Models.accessRequest> accessRequest { get; set; }
      
    }
}
