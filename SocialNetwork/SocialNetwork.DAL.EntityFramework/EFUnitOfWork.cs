using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.EntityFramework
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;
    using SocialNetwork.DAL.EntityFramework.Repositories;

    public class EFUnitOfWork : IUnitOfWork
    {
        private SocialNetworkModelContext context;

        private EFUserRepository userRepository;
        private EFContentRepository contentRepository;
        private EFUserPostRepository userPostRepository;
        private EFDialogRepository dialogRepository;
        private EFFollowersFramework followersRepository;
        private EFDialogMembersRepository dialogMembersRepository;
        private EFBlackListRepository blackListRepository;

        public EFUnitOfWork(string connectionString)
        {
            context = new SocialNetworkModelContext(connectionString);
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new EFUserRepository(context);
                return userRepository;
            }
        }

        public IRepository<Content> ContentCollection
        {
            get
            {
                if (contentRepository == null)
                    contentRepository = new EFContentRepository(context);
                return contentRepository;
            }
        }

        public IRepository<UserPost> UserPosts
        {
            get
            {
                if (userPostRepository == null)
                    userPostRepository = new EFUserPostRepository(context);
                return userPostRepository;
            }
        }

        public IRepository<Dialog> Dialogs
        {
            get
            {
                if (dialogRepository == null)
                    dialogRepository = new EFDialogRepository(context);
                return dialogRepository;
            }
        }

        public IRepository<DialogMember> DialogMembers
        {
            get
            {
                if (dialogMembersRepository == null)
                    dialogMembersRepository = new EFDialogMembersRepository(context);
                return dialogMembersRepository;
            }
        }

        public IRepository<Follower> Followers
        {
            get
            {
                if (followersRepository == null)
                    followersRepository = new EFFollowersFramework(context);
                return followersRepository;
            }
        }

        public IRepository<BlackList> BlackLists
        {
            get
            {
                if (blackListRepository == null)
                    blackListRepository = new EFBlackListRepository(context);
                return blackListRepository;
            }
        }
    }
}
