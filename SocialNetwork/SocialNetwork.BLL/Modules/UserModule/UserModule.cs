using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.BLL.Modules.UserModule
{
    using SocialNetwork.BLL.Models;
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;
    using SocialNetwork.BLL.DataProvider;
    using SocialNetwork.BLL.BusinessLogic.Exceptions;
    using SocialNetwork.BLL.BusinessLogic.EntityConverters;
    using SocialNetwork.BLL.BusinessLogic.ContentManagement;
    using System.Text.RegularExpressions;
    using System.IO;    

    public sealed class UserModule : IUserModule
    {
        private IUnitOfWork unitOfWork;        
        private int currentUserID;

        private UserConverter userConverter;
        private ContentFileManager fileManager;

        private const int passwordMaxLength = 32;
        private const int emailMaxLength = 32;        

        public UserModule(IUnitOfWorkFactory unitOfWorkFactory, int userID)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWorkFactory instance is null", default(Exception));
            this.unitOfWork = unitOfWorkFactory.Create();

            if (unitOfWork.Users.Get(userID) == null)
                throw new BusinessEntityNullException("User is not found");
            this.currentUserID = userID;

            this.userConverter = new UserConverter();
            this.fileManager = new ContentFileManager(unitOfWork);
        }

        private void ValidateUser(int userID,
            string sameUsersIDErrorMessage = "This operation can\'t be applied to itself",
            string userIsNorFoundErrorMessage = "User with current id is not found")
        {
            if (userID == currentUserID)
                throw new BusinessLogicException(sameUsersIDErrorMessage);

            var user = unitOfWork.Users.Get(userID);
            if (user == null)
                throw new BusinessEntityNullException(userIsNorFoundErrorMessage);

        }

        private void ValidateDialog(int dialogID)
        {
            var dialog = unitOfWork.Dialogs.Get(dialogID);
            if (dialog == null)
                throw new BusinessEntityNullException("Dialog with this ID has not found");

            var dialogMember = unitOfWork.DialogMembers
                .Find(x => x.DialogID == dialogID && x.MemberID == currentUserID)
                .FirstOrDefault();
            if (dialogMember == null)
                throw new BusinessLogicException("User is not a member of this dialog");
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
            ValidateUser(userID, "User can\'t follow to the himself");
          
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
            ValidateUser(userID, "User can\'t unfollow from the himself");

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
            ValidateUser(userID, "User can\'t add or remove itself to the blacklist");

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
            ValidateUser(userID, "User can\'t add or remove itself to the blacklist");

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
                // Из DialogMembers
                var userDialogsID = unitOfWork.DialogMembers
                    // Выбираем такие, где есть ID пользователя
                    .Find(x => x.MemberID == currentUserID)
                    // Селектим только ID
                    .Select(x => x.DialogID);

                // Из DialogMembers
                var usersInDialogsID = unitOfWork.DialogMembers
                    // Выбираем такие, где ID диалога совпадает с ID диалогов пользователя
                    .Find(x => userDialogsID.Contains(x.DialogID))
                    // Селектим ID учасников
                    .Select(x => x.MemberID)
                    // Убираем повторы тех, кто есть в разных диалогах
                    .Distinct();

                // из Users
                var userEntities = unitOfWork.Users
                    // Выбираем таких, ID которых совпадает с usersInDialogsID
                   .Find(x => usersInDialogsID.Contains(x.ID))
                   // Конвертируем в UserInfoBLL
                   .Select(y => userConverter.ConvertToBLLEntity(y));


                var dialogs = unitOfWork.Dialogs
                    .Find(x => userDialogsID.Contains(x.ID))
                    .Select(x => new DialogBLL()
                    {
                        ID = x.ID,
                        MasterID = x.MasterID,
                        Name = x.Name,
                        ContentPath = x.DialogContentID,
                        DialogCreatedDate = x.DialogCreatedDate,
                        isReadOnly = x.IsReadOnly,
                        Members = new List<UserInfoBLL>(userEntities.Where(y => userDialogsID.Contains(y.ID)))
                    });

                return null;                             
            }
        }

        public void SendMessage(int dialogID, string text, IEnumerable<FileStream> content)
        {
            ValidateDialog(dialogID);

            var dialogPath = unitOfWork.ContentPaths.Get(
                unitOfWork.Dialogs
                .Find(x => x.ID == dialogID)
                .FirstOrDefault()
                .DialogContentID.Value).Path;

            fileManager.WriteToDialog(dialogPath, currentUserID, text, content);
        }

        public void StartDialog(string name, bool isReadOnly, int? userID)
        {
            if (userID != null)
            {
                ValidateUser(userID.Value, "User can\'t add itself to the dialog");

                if (!unitOfWork.Users.Get(userID.Value).IsOthersCanStartChat)
                    throw new BusinessAdmissionException("Selected user blocked inviting");
            }

            string fullPath = fileManager.Create(name, currentUserID);

            var content = new Content() { Category = "Dialog", Path = fullPath };
            unitOfWork.ContentPaths.Add(content);

            var dialog = new Dialog()
            {
                Name = name,
                IsReadOnly = isReadOnly,
                MasterID = currentUserID,
                DialogCreatedDate = DateTime.Now,
                DialogContentID = unitOfWork.ContentPaths.Find(x => x.Path == fullPath).First().ID
            };
            
            unitOfWork.Dialogs.Add(dialog);
            unitOfWork.DialogMembers.Add(new DialogMember()
            {
                DialogID = dialog.ID,
                MemberID = currentUserID
            });

            if (userID != null)
            {
                unitOfWork.DialogMembers.Add(new DialogMember()
                {
                    DialogID = dialog.ID,
                    MemberID = currentUserID
                });
            }
        }

        public void AddUserToDialog(int userID, int dialogID)
        {
            ValidateUser(userID, "User is already in this dialog");
            ValidateDialog(dialogID);

            var dialog = unitOfWork.Dialogs.Get(dialogID);

            if (dialog.MasterID != currentUserID) 
                throw new BusinessAdmissionException("User can'\t grant others because of he is not a master");

            if (!unitOfWork.Users.Get(userID).IsOthersCanStartChat)
                throw new BusinessAdmissionException("Selected user blocked inviting");            

            unitOfWork.DialogMembers.Add(new DialogMember()
            {
                DialogID = dialogID,
                MemberID = userID
            });
        }

        public void AppointNewMaster(int newMasterID, int dialogID)
        {
            ValidateUser(newMasterID, "User is already master of this dialog");
            ValidateDialog(dialogID);

            var dialog = unitOfWork.Dialogs.Get(dialogID);

            if (dialog.MasterID != currentUserID)
                throw new BusinessAdmissionException("User can'\t grant others because of he is not a master");

            if (unitOfWork.DialogMembers.Find(x => x.MemberID == newMasterID).FirstOrDefault() == null)
                throw new BusinessLogicException("Granting User is not a member of this dialog");
           
            dialog.MasterID = newMasterID;
        }

        public void LeaveFromDialog(int dialogID)
        {            
            ValidateDialog(dialogID);

            var dialog = unitOfWork.Dialogs.Get(dialogID);

            if (dialog.MasterID == currentUserID)
                dialog.MasterID = null;

            var dialogMember = unitOfWork.DialogMembers
                .Find(x => x.MemberID == currentUserID && x.DialogID == dialogID)
                .FirstOrDefault();

            unitOfWork.DialogMembers.Delete(dialogMember.ID);
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