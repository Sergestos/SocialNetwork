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

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }        

        [Display(Name = "Birthday")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime BirthDayDate { get; set; }

        [Required, Microsoft.Web.Mvc.FileExtensions(Extensions = "jpg,png",ErrorMessage = "Specify file")]
        public HttpPostedFileBase Avatar { get; set; }
    }
}