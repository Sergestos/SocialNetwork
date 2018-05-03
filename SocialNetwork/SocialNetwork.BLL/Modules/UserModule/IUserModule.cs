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
        UserInfoBLL GetItselfInfo { get; }
        IEnumerable<PostBLL> GetItselfPosts { get; }
        IEnumerable<UserInfoBLL> GetItselfFollewers { get; }
        IEnumerable<DialogBLL> GetDialogs { get; }        
        IEnumerable<UserInfoBLL> GetUsersAddedToBlackList { get; }
        
        IEnumerable<UserInfoBLL> FindUsers(Func<User, Boolean> predicate);
        IEnumerable<PostBLL> GetUserPost(int userID);

        void FollowTo(int userID);
        void Unfollow(int userID);

        void StartDialog(string name, bool isReadOnly, int? userID);
        void AddUserToDialog(int userID, int dialogID);
        void AppointNewMaster(int newMasterID, int dialogID);
        void LeaveFromDialog(int dialogID);
        void SendMessage(int dialogID, string text, IEnumerable<FileStream> content);

        void CreatePost(PostBLL post);
        void RemovePost(int postID);
        void LikePost(int postID);

        void AddToBlackList(int userID);
        void RemoveFromBlackList(int userID);

        void ChangeEmail(string newEmail);
        void ChangePassword(string password);
        void ChangeItselfInfo(UserInfoBLL user);

        void DeleteAccount();
        void LogOut();
    }
}
