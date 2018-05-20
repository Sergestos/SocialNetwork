using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class SettingViewModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }
        public string Email { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
    }
}