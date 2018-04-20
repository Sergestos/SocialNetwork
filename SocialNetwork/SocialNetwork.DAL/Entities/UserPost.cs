using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entities
{
    public class UserPost
    {
        public int ID { get; set; }
        public int CreatorID { get; set; }
        public int PostContentSideID { get; set; }
        public int RepostedID { get; set; }
        public int CommentsContentSideID { get; set; }

        public int LikeCounter { get; set; }
        public int RepostCounter { get; set; }

        public DateTime PostCreatedDate { get; set; }
    }
}
