using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class RegistrationUserModel
    {
        [Required(ErrorMessage = "Name is required")]        
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Gender { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }

        //public DateTime BirthDayDate { get; set; }
    }
}