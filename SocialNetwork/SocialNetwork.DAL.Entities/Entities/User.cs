using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entities.Entities
{
    public class User
    {
        public int ID { get; set; }

        public string NickName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public DateTime RegistrationDate { get; set; }       
        public string Role { get; set; }
        public bool IsBanned { get; set; }
    }
}
