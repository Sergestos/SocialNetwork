using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.BusinessLogic.Exceptions
{
    public class BusinessWrongRootException : Exception
    {
        public BusinessWrongRootException() { }
        public BusinessWrongRootException(string message) : base(message) { }
        public BusinessWrongRootException(string message, Exception inner) : base(message, inner) { }
    }
}
