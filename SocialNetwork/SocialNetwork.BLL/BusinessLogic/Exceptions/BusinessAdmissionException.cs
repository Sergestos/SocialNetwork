using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.BusinessLogic.Exceptions
{
    public class BusinessAdmissionException : Exception
    {
        public BusinessAdmissionException() { }
        public BusinessAdmissionException(string message) : base(message) { }
        public BusinessAdmissionException(string message, Exception inner) : base(message, inner) { }
    }
}
