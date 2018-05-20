using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class DialogInfo
    {
        public int ID { get; set; }
        public int MasterID { get; set; }
        public bool IsReadOnly { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserView> UsersInDialog { get; set; }        
    }
}