using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class DialogView
    {
        public int DialogID { get; set; }
        public int MasterID { get; set; }
        public string Name { get; set; }        
        public int QuantityOfMembers { get; set; }
        public bool IsReadOnly { get; set; }
        public IEnumerable<DialogMessageView> Messages { get; set; }
    }
}