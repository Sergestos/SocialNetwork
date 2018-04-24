using SocialNetwork.BLL.ModelsBLL;
using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Modules.UserModule
{
    interface IUserModule
    {
        UserInfoBLL GetItselfInfo { get; }
        IEnumerable<PostBLL> GetItselfPost { get; }
        IEnumerable<UserInfoBLL> GetItselfFollewers { get; }

        IEnumerable<DialogBLL> GetDialogs { get; }        
        
        IEnumerable<UserInfoBLL> FindUsers(Func<User, Boolean> predicate);
        IEnumerable<PostBLL> GetUserPost(int userId);
    }
}
