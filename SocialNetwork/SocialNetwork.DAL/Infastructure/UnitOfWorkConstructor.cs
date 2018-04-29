using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Infastructure
{
    public class UnitOfWorkConstructor : IUnitOfWork
    {
        private IRepository<User> userRepository;
        private IRepository<Content> contentRepository;
        private IRepository<UserPost> userPostRepository;
        private IRepository<Dialog> dialogRepository;
        private IRepository<Follower> followersRepository;
        private IRepository<DialogMember> dialogMembersRepository;
        private IRepository<BlackList> blackListRepository;
        private string contentDirectory;

        public UnitOfWorkConstructor(
            IRepository<User> userRepository,
            IRepository<Content> contentRepository, 
            IRepository<UserPost> userPostRepository,
            IRepository<Dialog> dialogRepository,
            IRepository<Follower> followersRepository,
            IRepository<DialogMember> dialogMembersRepository,
            IRepository<BlackList> blackListRepository,
            string contentDirectory)
        {
            this.userRepository = userRepository;
            this.contentRepository = contentRepository;
            this.userPostRepository = userPostRepository;
            this.dialogRepository = dialogRepository;
            this.followersRepository = followersRepository;
            this.dialogMembersRepository = dialogMembersRepository;
            this.blackListRepository = blackListRepository;
            this.contentDirectory = contentDirectory;
        }

        public string MainContentDirectory => string.IsNullOrEmpty(contentDirectory) ? contentDirectory
            : throw new NullReferenceException("ContentDirectory string is null or empty");

        public IRepository<User> Users                 => userRepository          ?? throw nullRefException;
        public IRepository<UserPost> UserPosts         => userPostRepository      ?? throw nullRefException;
        public IRepository<Dialog> Dialogs             => dialogRepository        ?? throw nullRefException;
        public IRepository<DialogMember> DialogMembers => dialogMembersRepository ?? throw nullRefException;
        public IRepository<Follower> Followers         => followersRepository     ?? throw nullRefException;
        public IRepository<Content> ContentPaths  => contentRepository       ?? throw nullRefException;
        public IRepository<BlackList> BlackLists       => blackListRepository     ?? throw nullRefException;        

        private NullReferenceException nullRefException = new NullReferenceException("Repository has not been initialized");
    }
}
