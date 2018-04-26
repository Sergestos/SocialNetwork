using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entities
{
    public class Follower
    {
        public int ID { get; set; }
        public int FollowedToID { get; set; }
        public int FollowerID { get; set; }
    }
}
