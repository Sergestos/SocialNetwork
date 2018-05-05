using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.DAL.Infastructure;
using SocialNetwork.FakeDataProviders;

namespace SocialNetwork.BLL.DataProvider
{
    public sealed class FakeUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private string contentPath;

        public FakeUnitOfWorkFactory(string contentPath = null)
        {
            this.contentPath = contentPath;
        }

        public IUnitOfWork Create()
        {
            return new FakeUnitOfWork(contentPath);
        }
    }
}
