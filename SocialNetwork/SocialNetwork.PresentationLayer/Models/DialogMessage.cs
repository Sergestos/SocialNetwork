using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Models
{
    public class DialogMessageView
    {
        public int SenderID { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public string SendDate { get; set; }
        public List<int> ContentsID { get; set; }
    }
}