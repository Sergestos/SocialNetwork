using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.UnitTests.BusinessLayerTests
{
    using NUnit.Framework;
    using SocialNetwork.BLL.ModelsBLL;
    using SocialNetwork.BLL.BusinessLogic;    
    using SocialNetwork.BLL.Modules.UserModule;
    using SocialNetwork.DAL.Infastructure;
    using SocialNetwork.UnitTests.FakeDataProviders;

    [TestFixture]
    [Category("UserModule")]
    public class UserModuleUnitTests
    {
        [Test]
        public void UserModule_Initialization_UnitOfWorkNotNullRef()
        {
            IUnitOfWork unitOfWork = new FakeUnitOfWork();

            IUserModule userModule = new UserModule(unitOfWork, 0);

            Assert.NotNull(userModule);
        }

        [TestCase("UnitOfWork instance is null")]
        public void UserModule_Initialization_CatchExceptionArgumentIsNull(string errorMessage)
        {
            IUnitOfWork unitOfWork = new FakeUnitOfWork();
            IUserModule userModule;

            try
            {
                userModule = new UserModule(null, 0);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual(errorMessage, ex.Message);
            }            
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(3, false)]
        public void UserModule_Initialization_DifferentUserID(int id, bool expected)
        {
            IUnitOfWork unitOfWork = new FakeUnitOfWork();
            IUserModule userModule = null;

            try
            {
                userModule = new UserModule(unitOfWork, id);
            }
            catch { }            
            
            Assert.AreEqual(expected, userModule != null);
        }

        [TestCase(3, "User is not found")]
        [TestCase(-1, "User is not found")]
        public void UserModule_Initialization_CatchExceptionDueWrongUserID(int userID, string errorMessage)
        {
            IUnitOfWork unitOfWork = new FakeUnitOfWork();
            IUserModule userModule;

            try
            {
                userModule = new UserModule(unitOfWork, userID);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessModuleException), ex.GetType());
                Assert.AreEqual(errorMessage, ex.Message);
            }
        }
        
        [TestCase(0)]
        public void UserModule_GetItselfInfo_GetCorrectUserFromUnitOfWork(int userID)
        {
            IUnitOfWork unitOfWork = new FakeUnitOfWork();
            IUserModule userModule = new UserModule(unitOfWork, userID);

            var userFromUnitOfWorkID = unitOfWork.Users.Get(userID).ID;
            var userFromUserModuleID = userModule.GetItselfInfo.ID;

            Assert.AreEqual(userFromUnitOfWorkID, userFromUserModuleID);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void UserModule_GetItselfPosts_GetPosts(int userID)
        {
            IUnitOfWork unitOfWork = new FakeUnitOfWork();
            IUserModule userModule = new UserModule(unitOfWork, userID);

            var postsFromModele = userModule.GetItselfPosts.Select(x => x.ID);
            var postFromUnit = unitOfWork.UserPosts.Find(x => x.ID == userID).Select(x => x.ID);

            Assert.NotNull(postsFromModele);
            CollectionAssert.AreEqual(postsFromModele, postFromUnit);
        }
        
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(1, 2)]
        public void UserModule_FollowFuncs_FollowToTheUser(int followerID, int followedToID)
        {

        }
    }
}
