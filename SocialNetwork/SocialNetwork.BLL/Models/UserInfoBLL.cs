using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Models
{
    public class UserInfoBLL
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public ContentBLL Content { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
