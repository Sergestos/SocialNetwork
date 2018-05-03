using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Infastructure;
using SocialNetwork.BLL.BusinessLogic.EntityConverters;
using SocialNetwork.BLL.DataProvider;

namespace SocialNetwork.BLL.Modules.AnonymousModule
{
    public sealed class AnonymousModule : IAnonymoysModule
    {
        private IUnitOfWork unitOfWork;
        private UserConverter converter;

        public AnonymousModule(IUnitOfWorkFactory unitOfWorkFactory)
        {
            if (unitOfWork == null)
                throw new BusinessLogic.Exceptions.BusinessEntityNullException("UnitOfWork is not initialized");
            this.unitOfWork = unitOfWorkFactory.Create();

            this.converter = new UserConverter();
        }

        public IEnumerable<UserInfoBLL> GetAllUsers =>
             unitOfWork.Users.GetAll.Select(x => converter.ConvertToBLLEntity(x));
    }
}
