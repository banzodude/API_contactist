using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dsgAPI.Models
{
    public class Contacts
    {
      

        public int ID { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string client { get; set; }
        public string company { get; set; }


        public Contacts()
        {

        }

        public Contacts(int ID, string name, string surname, string email, string client, string company)
        {
            this.ID = ID;
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.client = client;
            this.company = company;
        }
    }
}
