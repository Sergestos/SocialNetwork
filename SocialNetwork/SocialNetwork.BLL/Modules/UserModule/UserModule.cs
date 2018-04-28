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
    using SocialNetwork.BLL.BusinessLogic.Exceptions;
    using SocialNetwork.BLL.BusinessLogic.EntityConverters;
    using System.Text.RegularExpressions;

    public sealed class UserModule : IUserModule
    {
        private IUnitOfWork unitOfWork;        
        private int currentUserID;

        private UserConverter userConverter;

        private const int passwordMaxLength = 32;
        private const int emailMaxLength = 32;

        public UserModule(IUnitOfWork unitOfWork, int userID)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("UnitOfWork instance is null", default(Exception));
            this.unitOfWork = unitOfWork;

            if (unitOfWork.Users.Get(userID) == null)
                throw new BusinessEntityNullException("User is not found");
            this.currentUserID = userID;

            this.userConverter = new UserConverter();
        }

        private void ValidateUserID(int userID,
            string sameUsersIDErrorMessage = "This operation can\'t be applied to itself",
            string userIsNorFoundErrorMessage = "User with current id is not found")
        {
            if (userID == currentUserID)
                throw new BusinessLogicException(sameUsersIDErrorMessage);

            var user = unitOfWork.Users.Get(userID);
            if (user == null)
                throw new BusinessEntityNullException(userIsNorFoundErrorMessage);

        }

        public UserInfoBLL GetItselfInfo
        {
            get
            {
                var user = unitOfWork.Users.Get(currentUserID);
                return userConverter.ConvertToBLLEntity(user);
            }            
        }

        public IEnumerable<PostBLL> GetItselfPosts
        {
            get
            {
                return unitOfWork.UserPosts
                    .Find(x => x.ID == currentUserID)
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
            ValidateUserID(userID, "User can\'t follow to the himself");
          
            var follower = unitOfWork.Followers
                .Find(x => (x.FollowerID == currentUserID) && (x.FollowedToID == userID))
                .FirstOrDefault();
            if (follower != null)
                throw new BusinessLogicException("User can\'t subscribe twice");

            var bannedUser = unitOfWork.BlackLists
                .Find(x => x.UserIDBanned == userID && x.UserIDBanner == currentUserID)
                .FirstOrDefault();
            if (bannedUser != null)
                throw new BusinessLogicException("User can\'t subscribe to the user in the blacklist");

            unitOfWork.Followers.Add(new Follower()
            {
                FollowedToID = userID,
                FollowerID = currentUserID
            });
        }

        public void Unfollow(int userID)
        {
            ValidateUserID(userID, "User can\'t unfollow from the himself");

            var follower = unitOfWork.Followers
                .Find(x => (x.FollowerID == currentUserID) && (x.FollowedToID == userID))
                .FirstOrDefault();
            if (follower == null)
                throw new BusinessEntityNullException("AuthorizedUser is not subscribed to the current user");

            unitOfWork.Followers.Delete(follower.ID);
        }

        public IEnumerable<UserInfoBLL> GetItselfFollewers
        {
            get
            {
                return unitOfWork.Followers
                    .Find(x => x.FollowedToID == currentUserID)
                    .Select(x => userConverter.ConvertToBLLEntity(unitOfWork.Users.Get(x.FollowerID)));
            }
        }

        public void ChangeEmail(string newEmail)
        {
            if (newEmail.Length >= emailMaxLength)
                throw new BusinessLogicException($"Email length must be less then {emailMaxLength}");

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(newEmail);
            if (!match.Success)
                throw new FormatException("The Email has wrong format");

            var user = unitOfWork.Users.Get(currentUserID);
            if (newEmail == user.Email)
                throw new BusinessLogicException("New email has to be different to the current");
            
            user.Email = newEmail;
            unitOfWork.Users.Update(user);
        }

        public void ChangePassword(string newPassword)
        {
            if (newPassword.Length >= passwordMaxLength)
                throw new BusinessLogicException($"New Password length must be less then {passwordMaxLength}");

            var user = unitOfWork.Users.Get(currentUserID);
            if (newPassword == user.Password)
                throw new BusinessLogicException("New password has to be different to the current");

            user.Password = newPassword;
            unitOfWork.Users.Update(user);
        }

        public IEnumerable<UserInfoBLL> GetUsersAddedToBlackList
        {
            get
            {
                return unitOfWork.BlackLists
                    .Find(x => x.UserIDBanner == currentUserID)
                    .Select(x => userConverter.ConvertToBLLEntity(unitOfWork.Users.Get(x.UserIDBanned)));
            }
        }

        public void AddToBlackList(int userID)
        {
            ValidateUserID(userID, "User can\'t add or remove itself to the blacklist");

            if (unitOfWork.BlackLists.GetAll.Any(x => x.UserIDBanner == currentUserID && x.UserIDBanned == userID))
                throw new BusinessLogicException("User has already been added to the blacklist");

            var currentUserFollowed = unitOfWork.Followers
                .Find(x => x.FollowerID == currentUserID && x.FollowedToID == userID)
                .FirstOrDefault();
            if (currentUserFollowed != null)
                unitOfWork.Followers.Delete(currentUserFollowed.ID);

            var badUserFollowed = unitOfWork.Followers
                .Find(x => x.FollowerID == userID && x.FollowedToID == currentUserID)
                .FirstOrDefault();
            if(badUserFollowed != null)
                unitOfWork.Followers.Delete(badUserFollowed.ID);

            unitOfWork.BlackLists.Add(new BlackList()
            {
                UserIDBanned = userID,
                UserIDBanner = currentUserID
            });
        }

        public void RemoveFromBlackList(int userID)
        {
            ValidateUserID(userID, "User can\'t add or remove itself to the blacklist");

            var bannedUser = unitOfWork.BlackLists
                .Find(x => x.UserIDBanned == userID && x.UserIDBanner == currentUserID)
                .FirstOrDefault();
            if (bannedUser == null)
                throw new BusinessLogicException("User hasn't been added to the blacklist");

            unitOfWork.BlackLists.Delete(bannedUser.ID);
        }

        public IEnumerable<UserInfoBLL> FindUsers(Func<User, bool> predicate)
        {
            return unitOfWork.Users
                .Find(predicate).Where(x => x.ID != currentUserID)
                .Select(x => userConverter.ConvertToBLLEntity(x));
        }

        public IEnumerable<DialogBLL> GetDialogs
        {
            get
            {
                var currentUserDialogs = unitOfWork.DialogMembers.Find(x => x.MemberID == currentUserID);
                var usersInDialogs = unitOfWork.Users
                    .Find(x => currentUserDialogs.Any(y => y.MemberID == x.ID))
                    .Select(x => userConverter.ConvertToBLLEntity(x));

                return unitOfWork.Dialogs
                    .Find(x => currentUserDialogs.Any(y => y.DialogID == x.ID))
                    .Select(x => new DialogBLL()
                    {
                        ID = x.ID,
                        MasterID = x.MasterID,
                        Name = x.Name,
                        ContentPath = x.DialogContentSidePath,
                        DialogCreatedDate = x.DialogCreatedDate,
                        isReadOnly = x.IsReadOnly,
                        Members = new List<UserInfoBLL>(usersInDialogs.Where(y => currentUserDialogs.Any(z => z.MemberID == y.ID)))
                    });                
            }
        }

        public void SendMessage(int chatID, object Content)
        {
            throw new NotImplementedException();
        }

        public void StartChat(int userID)
        {
            throw new NotImplementedException();
        }

        public void ChangeItselfInfo(UserInfoBLL user)
        {
            throw new NotImplementedException();
        }        

        public void CreatePost(PostBLL post)
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

        public void RemovePost(int postID)
        {
            throw new NotImplementedException();
        }        

        public void DeleteAccount() => throw new NotImplementedException();

        public void LogOut() => throw new NotImplementedException();
    }
}
