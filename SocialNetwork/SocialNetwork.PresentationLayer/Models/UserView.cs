using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class UserView
    {
        public int ID { get; set; }
        public string Email { get; set; }        
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }        
        public int Age { get; set; }
        public int AvatarID { get; set; }
        public bool IsCanStartDialog { get; set; }
    }
}