using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SocialNetwork.DAL.Entities
{
    public class Dialog
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DialogCreatedDate { get; set; }

        public int? MasterID { get; set; }        
        public int? DialogContentID { get; set; }

        public bool IsReadOnly { get; set; }        
    }
}
