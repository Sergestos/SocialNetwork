using SocialNetwork.BLL.ModelsBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.PresentationLayer.Infastructure
{
    public class UserAuthentication : IAuthentication
    {
        private IEnumerable<UserInfoBLL> users;

        public UserAuthentication(IEnumerable<UserInfoBLL> users)
        {
            this.users = users;
        }

        public bool IsValid(string login, string password)
        {
            return users.Where(x => x.Email == login && x.Password == password).FirstOrDefault() != null;
        }

        public bool LogOut()
        {
            return true;
        }
    }
}
