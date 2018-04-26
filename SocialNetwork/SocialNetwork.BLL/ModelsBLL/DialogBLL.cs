using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.ModelsBLL
{
    using System.Xml;
    using SocialNetwork.DAL.Entities;

    public class DialogBLL
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DialogCreatedDate { get; set; }

        public User MasterID { get; set; }
        public List<User> Members{ get; set; }

        public XmlDocument Content { get; set; }
    }
}
