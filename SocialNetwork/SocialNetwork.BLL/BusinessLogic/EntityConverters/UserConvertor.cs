using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.BusinessLogic.EntityConverters
{
    using SocialNetwork.BLL.Models;
    using SocialNetwork.DAL.Entities;

    internal sealed class UserConverter : IEntityConverter<UserInfoBLL, User>
    {
        public UserInfoBLL ConvertToBLLEntity(User originalEntity)
        {
            return new UserInfoBLL()
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
                Gender = originalEntity.Gender
            };
        }

        public User ConvertToOriginalEntity(UserInfoBLL bllEntity)
        {
            throw new NotImplementedException();
        }
    }
}
