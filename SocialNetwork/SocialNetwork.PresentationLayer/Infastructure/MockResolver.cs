using SocialNetwork.BLL.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Infastructure
{
    public static class MockResolver
    {
        private static IUnitOfWorkFactory factory;

        static MockResolver()
        {
            //factory = new EFUnitOfWorkFactory("string1", "string2");
        }

        public static IUnitOfWorkFactory GetUnitOfWorkFactory()
        {
            return factory;
        }
    }
}