using SocialNetwork.BLL.DataProvider;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Modules.UserModule;
using SocialNetwork.PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace SocialNetwork.PresentationLayer.Infastructure.EntityConverters
{
    public static class EntityConverter
    {
        public static UserView GetUserView(UserInfoBLL userInfo)
        {            
            return new UserView()
            {
                ID = userInfo.ID,
                FirstName = userInfo.FirstName,
                SurName = userInfo.SurName,
                PhoneNumber = userInfo.PhoneNumber,
                Email = userInfo.Email,
                Gender = userInfo.Gender,
                Age = DateTime.Now.Year - userInfo.BirthDate.Year,
                Country = userInfo.Country,
                Locality = userInfo.Locality,
                AvatarID = userInfo.Content.ID,
                IsCanStartDialog = userInfo.IsOthersCanStartDialog
            };
        }

        public static SettingViewModel GetSettingInfoModel(int userId, IUserModule userModule)
        {
            var user = userModule.GetMyInfo;

            return new SettingViewModel()
            {
                Surname = user.SurName,
                Name = user.FirstName,
                Country = user.Country,
                Location = user.Locality,
                Email = user.Email,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static DialogPreview GetDialogPreview(DialogBLL dialog, IUserModule userModule)
        {
            return new DialogPreview()
            {
                ID = dialog.ID,
                DialogName = dialog.Name,
                MasterAvatartID = userModule.GetAllUsers.FirstOrDefault(x => x.ID == dialog.MasterID.Value).ID
            };
        }

        public static DialogInfo GetDialogInfo(int dialogID, IUserModule userModule)
        {
            var dialogBll = userModule.GetDialog(dialogID);
            if (dialogBll == null)
                return null;

            return new DialogInfo()
            {
                ID = dialogBll.ID,
                MasterID = dialogBll.MasterID.Value,
                IsReadOnly = dialogBll.isReadOnly,
                Name = dialogBll.Name,                
                UsersInDialog = dialogBll.Members.Select(x => GetUserView(x))
            };
        }

        public static DialogView GetDialog(int dialogID, IUserModule userModule)
        {
            DialogBLL dialogBll = userModule.GetDialog(dialogID);

            List<DialogMessageView> messages = new List<DialogMessageView>();

            XDocument doc = XDocument.Load(userModule.GetContent(dialogBll.ContentID.Value).Path);
            foreach (XElement messageItem in doc.Element("dialog").Elements("message"))
            {
                int senderID = Convert.ToInt32(messageItem.Attribute("userID").Value);
                string text = messageItem.Element("text") != null ? messageItem.Element("text").Value : string.Empty;
                string dateAsString = messageItem.Attribute("sent_at") != null ? messageItem.Attribute("sent_at").Value.Replace('T', ' ') : "0:00";
                var contentIDs = new List<int>();
                foreach (XElement contentID in messageItem.Elements("contentID"))
                    contentIDs.Add(Convert.ToInt32(contentID.Value));

                messages.Add(new DialogMessageView()
                {
                    SenderID = senderID,
                    Message = text,
                    SenderName = userModule.GetAllUsers.FirstOrDefault(x => x.ID == senderID).SurName,
                    ContentsID = contentIDs,
                    SendDate = dateAsString
                });
            }

            return new DialogView()
            {
                DialogID = dialogBll.ID,
                IsReadOnly = dialogBll.isReadOnly,
                MasterID = dialogBll.MasterID.Value,
                Name = dialogBll.Name,
                QuantityOfMembers = dialogBll.Members.Count(),
                Messages = messages,                
            };
        }
    }
}