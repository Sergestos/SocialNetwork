using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Infastructure
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<UserPost> UserPosts { get; }
        IRepository<Dialog> Dialogs { get; }
        IRepository<DialogMember> DialogMembers { get; }
        IRepository<Follower> Followers { get; }
        IRepository<Content> ContentCollection { get; }
    }
}
