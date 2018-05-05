using SocialNetwork.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Modules.AnonymousModule
{
    public interface IAnonymoysModule
    {
        IEnumerable<UserInfoBLL> GetAllUsers { get; }
        void Registrate(UserInfoBLL user);
    }
}
