using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.DAL.Entities;

namespace SocialNetwork.UnitTests.FakeDataProviders
{
    internal sealed class FakeUnitOfWork : IUnitOfWork
    {
        private IRepository<User> userRepository;
        private IRepository<UserPost> userPostsRepository;

        public FakeUnitOfWork()
        {
            userRepository = new FakeUserRepository();
            userPostsRepository = new FakeUsersPostRepository();
        }

        public IRepository<User> Users => userRepository;
        public IRepository<UserPost> UserPosts => userPostsRepository;

        public IRepository<Dialog> Dialogs => throw new NotImplementedException();
        public IRepository<DialogMember> DialogMembers => throw new NotImplementedException();
        public IRepository<Follower> Followers => throw new NotImplementedException();
        public IRepository<Content> ContentCollection => throw new NotImplementedException();
        public IRepository<BlackList> BlackLists => throw new NotImplementedException();
    }
}
