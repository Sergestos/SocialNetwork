using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.BusinessLogic
{    
    public class BusinessModuleException : Exception
    {
        public BusinessModuleException() { }
        public BusinessModuleException(string message) : base(message) { }
        public BusinessModuleException(string message, Exception inner) : base(message, inner) { }
    }
}
