using SocialNetwork.BLL.Modules.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class UserSession
    {
        public IUserModule userModule { get; set; }
        public bool IsLogged { get; set; }
        public DateTime LastReqest { get; set; }        
    }
}