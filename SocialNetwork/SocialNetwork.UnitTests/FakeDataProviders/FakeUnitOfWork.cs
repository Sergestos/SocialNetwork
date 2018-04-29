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
        private IRepository<Follower> followersRepository;
        private IRepository<BlackList> blackListRepository;
        private IRepository<Dialog> dialogRepository;

        public FakeUnitOfWork()
        {
            userRepository = new FakeUserRepository();
            userPostsRepository = new FakeUsersPostRepository();
            followersRepository = new FakeFollowersRepository();
            blackListRepository = new FakeBlacklistRepository();
            dialogRepository = new FakeDialogRepository();
        }

        public IRepository<User> Users => userRepository;
        public IRepository<UserPost> UserPosts => userPostsRepository;
        public IRepository<Follower> Followers => followersRepository;
        public IRepository<BlackList> BlackLists => blackListRepository;
        public IRepository<Dialog> Dialogs => dialogRepository;

        public IRepository<DialogMember> DialogMembers => throw new NotImplementedException();        
        public IRepository<Content> ContentPaths => throw new NotImplementedException();

        public string MainContentDirectory => throw new NotImplementedException();
    }
}
