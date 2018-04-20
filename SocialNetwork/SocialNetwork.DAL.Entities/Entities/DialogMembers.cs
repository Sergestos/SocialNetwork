using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entities.Entities
{
    public class DialogMembers
    {
        public int ID { get; set; }
        public int DialogID { get; set; }
        public int MemberID { get; set; }
    }
}
