using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entities
{
    public class User
    {
        public int ID { get; set; }

        public string FirstName { get; set; }        
        public string SurName { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public DateTime LastTimeOnlineDate { get; set; }
        public DateTime RegistrationDate { get; set; }       
        public string Role { get; set; }
        public bool IsBanned { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsShowInfoForAnonymousUsers { get; set; }
        public bool IsOthersCanStartChat { get; set; }
        public bool IsOthersCanComment { get; set; }
    }
}
