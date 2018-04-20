using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entities.Entities
{
    public class Friends
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int UserFriendID { get; set; }
    }
}
