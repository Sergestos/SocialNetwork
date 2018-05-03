using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.DataProvider
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
