using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.PresentationLayer.Infastructure
{
    public interface IAuthentication
    {
        bool IsValid(string login, string password);
        bool LogOut();
    }
}
