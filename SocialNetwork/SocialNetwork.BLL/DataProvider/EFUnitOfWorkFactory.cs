using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.DAL.Infastructure;
using SocialNetwork.DAL.EntityFramework;

namespace SocialNetwork.BLL.DataProvider
{
    public sealed class EFUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private string connectionString;
        private string contentPath;

        public EFUnitOfWorkFactory(string connectionString, string contentPath)
        {
            this.connectionString = connectionString;
            this.contentPath = contentPath;
        }

        public IUnitOfWork Create()
        {
            return new EFUnitOfWork(connectionString, contentPath);
        }
    }
}
