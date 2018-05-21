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
using System.IO;
using SocialNetwork.BLL.BusinessLogic.ContentManagement;

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

            this.converter = new UserConverter(unitOfWork);
        }

        public IEnumerable<UserInfoBLL> GetAllUsers =>
             unitOfWork.Users.GetAll.Select(x => converter.ConvertToBLLEntity(x));

        public void Registrate(UserInfoBLL user, Stream stream, string fileExtension = ".jpg")
        {
            if (unitOfWork.Users.GetAll.Where(x => x.Email == user.Email).FirstOrDefault() != null)
                throw new BusinessLogic.Exceptions.BusinessLogicException("User with current email already exists");

            IContentFileManager fileManager = new ContentFileManager(unitOfWork);
            fileManager.UploadFile(stream, out string savedPath);

            unitOfWork.Content.Add(new Content()
            {
                Category = "Avatar",
                Path = savedPath,
                Extension = fileExtension
            });

            unitOfWork.Users.Add(converter.ConvertToOriginalEntity(user, unitOfWork.Content.Find(x => x.Path == savedPath).FirstOrDefault().ID));
        }
    }
}
