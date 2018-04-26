using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Modules.UserModule
{
    using SocialNetwork.BLL.ModelsBLL;
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;
    using SocialNetwork.BLL.BusinessLogic;
    using SocialNetwork.BLL.BusinessLogic.EntityConverters;

    public sealed class UserModule : IUserModule
    {
        private IUnitOfWork unitOfWork;        
        private int authorizedUserID;

        private UserConverter userConverter;

        public UserModule(IUnitOfWork unitOfWork, int userID)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("UnitOfWork instance is null", default(Exception));
            this.unitOfWork = unitOfWork;

            if (unitOfWork.Users.Get(userID) == null)
                throw new BusinessModuleException("User is not found");
            this.authorizedUserID = userID;

            this.userConverter = new UserConverter();
        }

        public UserInfoBLL GetItselfInfo
        {
            get
            {
                var user = unitOfWork.Users.Get(authorizedUserID);
                return userConverter.ConvertToBLLEntity(user);
            }            
        }

        public IEnumerable<PostBLL> GetItselfPosts
        {
            get
            {
                return unitOfWork.UserPosts
                    .Find(x => x.ID == authorizedUserID)
                    .Select(x=> new PostBLL()
                        {
                            ID = x.ID,
                            LikeCounter = x.LikeCounter,
                            RepostCounter = x.RepostCounter,                            
                            CreatorID = x.CreatorID,
                            PostContentPath = x.PostContentSidePath,
                            RepostedID = x.RepostedID,
                            CommentsContentSidePath = x.CommentsContentSidePath,
                            PostCreatedDate = x.PostCreatedDate
                        });
            }
        }

        public void FollowTo(int userID)
        {
            var user = unitOfWork.Users.Get(userID);
            if (user == null)
                throw new BusinessModuleException("User with current id is not found");

            unitOfWork.Followers.Add(new Follower()
            {
                FollowedToID = userID,
                FollowerID = authorizedUserID
            });
        }

        public void Unfollow(int userID)
        {
            var user = unitOfWork.Users.Get(userID);
            if (user == null)
                throw new BusinessModuleException("User with current id is not found");

            var follower = unitOfWork.Followers
                .Find(x => (x.FollowerID == authorizedUserID) && (x.FollowedToID == userID))
                .FirstOrDefault();
            if (follower == null)
                throw new BusinessModuleException("AuthorizedUser is not subscribed to the current user");

            unitOfWork.Followers.Delete(follower.ID);
        }

        public IEnumerable<UserInfoBLL> GetItselfFollewers
        {
            get
            {
                return unitOfWork.Followers
                    .Find(x => x.FollowedToID == authorizedUserID)
                    .Select(x => userConverter.ConvertToBLLEntity(unitOfWork.Users.Get(x.FollowerID)));
            }
        }

        public IEnumerable<DialogBLL> GetDialogs => throw new NotImplementedException();

        public IEnumerable<BlackList> GetItselfBlackList => throw new NotImplementedException();

        public void CreatePost(PostBLL post)
        {
            throw new NotImplementedException();
        }

        public void AddToBlackList(int userID)
        {
            throw new NotImplementedException();
        }

        public void ChangeEmail(string newEmail)
        {
            throw new NotImplementedException();
        }

        public void ChangeItselfInfo(UserInfoBLL user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserInfoBLL> FindUsers(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PostBLL> GetUserPost(int userID)
        {
            throw new NotImplementedException();
        }

        public void LeaveFromChat(int chatID)
        {
            throw new NotImplementedException();
        }

        public void LikePost(int postID)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromBlackList(int userID)
        {
            throw new NotImplementedException();
        }

        public void RemovePost(int postID)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(int chatID, object Content)
        {
            throw new NotImplementedException();
        }

        public void StartChat(int userID)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(string password)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount() => throw new NotImplementedException();

        public void LogOut() => throw new NotImplementedException();
    }
}
