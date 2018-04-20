using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SocialNetwork.DAL.Entities.Entities
{
    public class Dialog
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public User MasterID { get; set; }
        public DateTime DialogCreatedDate { get; set; }
        public Xml DialogContent { get; set; }
    }
}
