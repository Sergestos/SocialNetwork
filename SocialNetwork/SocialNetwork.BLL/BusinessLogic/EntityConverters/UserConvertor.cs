using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.BusinessLogic.EntityConverters
{
    using SocialNetwork.BLL.Models;
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    internal sealed class UserConverter : IEntityConverter<UserInfoBLL, User>
    {
        private IUnitOfWork unitOfWork;

        public UserConverter(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public UserInfoBLL ConvertToBLLEntity(User originalEntity)
        {
            var content = unitOfWork.Content.Find(x => x.ID == originalEntity.AvatarContentID).FirstOrDefault();
            var userBll = new UserInfoBLL()
            {
                ID = originalEntity.ID,
                Email = originalEntity.Email,
                Password = originalEntity.Password,
                FirstName = originalEntity.FirstName,
                SurName = originalEntity.SurName,
                Country = originalEntity.Country,
                BirthDate = originalEntity.BirthDate,
                Locality = originalEntity.Locality,
                PhoneNumber = originalEntity.PhoneNumber,
                Gender = originalEntity.Gender,                
            };

            if (content != null)
            {
                userBll.Content = new ContentBLL()
                {
                    ID = content.ID,
                    Categoty = content.Category,
                    Path = content.Path,
                    Extension = content.Extension
                };
            }
            else
            {
                userBll.Content = null;
            }

            return userBll;
        }

        public User ConvertToOriginalEntity(UserInfoBLL bllEntity)
        {
            var user = new User()
            {
                FirstName = bllEntity.FirstName,
                SurName = bllEntity.SurName,
                Email = bllEntity.Email,
                Password = bllEntity.Password,
                Country = bllEntity.Country,
                Locality = bllEntity.Locality,
                PhoneNumber = bllEntity.PhoneNumber,
                Gender = bllEntity.Gender,
                BirthDate = bllEntity.BirthDate,
                IsBanned = false,
                IsDeleted = false,
                IsOthersCanStartChat = true,
                IsShowInfoForAnonymousUsers = true,
                IsOthersCanComment = true,
                LastTimeOnlineDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
                Role = "User",
            };

            return user;
        }


        public User ConvertToOriginalEntity(UserInfoBLL bllEntity, int contentID)
        {
            var user = ConvertToOriginalEntity(bllEntity);
            user.AvatarContentID = contentID;

            return user;
        }
    }
}
