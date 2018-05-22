using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class SettingViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "SurName is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
    }
}