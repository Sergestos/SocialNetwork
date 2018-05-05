using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Infastructure;
using SocialNetwork.BLL.BusinessLogic.EntityConverters;
using SocialNetwork.BLL.DataProvider;
using SocialNetwork.DAL.Entities;

namespace SocialNetwork.BLL.Modules.AnonymousModule
{
    public sealed class AnonymousModule : IAnonymoysModule
    {
        private IUnitOfWork unitOfWork;
        private UserConverter converter;

        public AnonymousModule(IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (unitOfWorkFactory == null)
                throw new BusinessLogic.Exceptions.BusinessEntityNullException("UnitOfWork is not initialized");
            this.unitOfWork = unitOfWorkFactory.Create();

            this.converter = new UserConverter();
        }

        public IEnumerable<UserInfoBLL> GetAllUsers =>
             unitOfWork.Users.GetAll.Select(x => converter.ConvertToBLLEntity(x));

        public void Registrate(UserInfoBLL user)
        {
            unitOfWork.Users.Add(new User()
            {
                FirstName = user.FirstName,
                SurName = user.SurName,
                Email = user.Email,
                Password = user.Password,
                Country = user.Country,
                Locality = user.Locality,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                IsBanned = false,
                IsDeleted = false,
                IsOthersCanStartChat = true,
                IsShowInfoForAnonymousUsers = true,
                IsOthersCanComment = true,
                LastTimeOnlineDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
                Role = "User"
                // Avatar Content ID
            });            
        }
    }
}
