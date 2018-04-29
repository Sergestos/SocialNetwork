using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.ModelsBLL
{
    using System.Xml;
    using SocialNetwork.DAL.Entities;

    public class ChatBLL
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DialogCreatedDate { get; set; }

        public int? MasterID { get; set; }
        public List<UserInfoBLL> Members{ get; set; }

        public int? ContentPath { get; set; }

        public bool isReadOnly { get; set; }
    }
}
