using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Modules.UserModule
{
    using SocialNetwork.BLL.Models;
    using SocialNetwork.DAL.Entities;

    public interface IUserModule
    {
        UserInfoBLL GetMyInfo { get; }
        IEnumerable<PostBLL> GetMyPosts { get; }        
        IEnumerable<DialogBLL> GetDialogs { get; }

        IEnumerable<UserInfoBLL> GetMyFollewers { get; }
        IEnumerable<UserInfoBLL> GetUsersIFollowing { get; }

        IEnumerable<UserInfoBLL> GetUsersAddedToBlackList { get; }
        IEnumerable<UserInfoBLL> GetUsersAddedMeToBlackList { get; }

        IEnumerable<UserInfoBLL> GetAllUsers { get; }
        IEnumerable<UserInfoBLL> FindUsers(Func<User, Boolean> predicate);        

        IEnumerable<PostBLL> GetUserPost(int userID);

        void Follow(int userID);
        void Unfollow(int userID);

        void StartDialog(string name, bool isReadOnly, int? userID);
        void AddUserToDialog(int userID, int dialogID);
        void AppointNewMaster(int newMasterID, int dialogID);
        void LeaveFromDialog(int dialogID);
        void RemoveFromDialog(int userID, int dialogID);
        void SendMessage(int dialogID, string text, IEnumerable<Stream> content);
        void ChangeDialogSetting(int dialogID, string name, bool isReadOnly);
        DialogBLL GetDialog(int dialogID);
        ContentBLL GetContent(int contentID);

        void CreatePost(PostBLL post);
        void RemovePost(int postID);
        void LikePost(int postID);

        void AddToBlackList(int userID);
        void RemoveFromBlackList(int userID);

        void ChangeAvatar(Stream newAvatar);
        void ChangePrivacy(bool IsOthersCanComment, bool IsOthersCanStartDialog, bool IsShowInfoForAnonymousUsers);
        void ChangePassword(string password);
        void ChangeEmail(string email);
        void ChangeItselfInfo(UserInfoBLL user);
    }
}
