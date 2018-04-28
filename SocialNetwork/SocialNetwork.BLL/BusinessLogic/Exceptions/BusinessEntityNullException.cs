using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.BusinessLogic.Exceptions
{
    public class BusinessEntityNullException : Exception
    {
        public BusinessEntityNullException() { }
        public BusinessEntityNullException(string message) : base(message) { }
        public BusinessEntityNullException(string message, Exception inner) : base(message, inner) { }
    }
}
