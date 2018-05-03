using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Models
{
    public class PostBLL
    {
        public int ID { get; set; }
        public int? CreatorID { get; set; }
        public string PostContentPath { get; set; }
        public int? RepostedID { get; set; }
        public string CommentsContentSidePath { get; set; }

        public int LikeCounter { get; set; }
        public int RepostCounter { get; set; }

        public DateTime PostCreatedDate { get; set; }
    }
}
