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
    using SocialNetwork.BLL.BusinessLogic.Exceptions;
    using SocialNetwork.DAL.Infastructure;
    using SocialNetwork.UnitTests.FakeDataProviders;    

    [TestFixture]
    [Category("UserModule")]
    public class UserModuleUnitTests
    {
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new FakeUnitOfWork();
        }

        [Test]
        public void UserModule_Initialization_UnitOfWorkNotNullRef()
        {
            IUserModule userModule = new UserModule(unitOfWork, 0);

            Assert.NotNull(userModule);
        }

        [Test]
        public void UserModule_Initialization_CatchExceptionArgumentIsNull()
        {
            IUserModule userModule;

            try
            {
                userModule = new UserModule(null, 0);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual("UnitOfWork instance is null", ex.Message);
            }            
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(3, false)]
        public void UserModule_Initialization_DifferentUserID(int id, bool expected)
        {
            IUserModule userModule = null;

            try
            {
                userModule = new UserModule(unitOfWork, id);
            }
            catch { }            
            
            Assert.AreEqual(expected, userModule != null);
        }

        [TestCase(3)]
        [TestCase(-1)]
        public void UserModule_Initialization_CatchExceptionDueWrongUserID(int userID)
        {
            IUserModule userModule;

            try
            {
                userModule = new UserModule(unitOfWork, userID);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessEntityNullException), ex.GetType());
                Assert.AreEqual("User is not found", ex.Message);
            }
        }
        
        [Test]
        public void UserModule_GetItselfInfo_GetCorrectUserFromUnitOfWork()
        {
            int userID = 0;
            IUserModule userModule = new UserModule(unitOfWork, userID);

            var userFromUnitOfWorkID = unitOfWork.Users.Get(userID).ID;
            var userFromUserModuleID = userModule.GetItselfInfo.ID;

            Assert.AreEqual(userFromUnitOfWorkID, userFromUserModuleID);
        }

        [Ignore("The Test doesn't check case when user gets a lot of post instance")]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void UserModule_GetItselfPosts_GetPosts(int userID)
        {
            IUserModule userModule = new UserModule(unitOfWork, userID);

            var postsFromModele = userModule.GetItselfPosts.Select(x => x.ID);
            var postFromUnit = unitOfWork.UserPosts.Find(x => x.ID == userID).Select(x => x.ID);

            Assert.NotNull(postsFromModele);
            CollectionAssert.AreEqual(postsFromModele, postFromUnit);
        }
                
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        public void UserModule_Follow_FollowToTheOneUser(int followerID, int followedToID)
        {
            IUserModule userModule = new UserModule(unitOfWork, followerID);

            userModule.FollowTo(followedToID);
            var follower = unitOfWork.Followers.Find(x => x.FollowerID == followerID).FirstOrDefault();

            Assert.NotNull(followerID);
            Assert.AreEqual(follower.FollowedToID, followedToID);
        }

        [TestCase(0, 1, 2)]
        public void UserModule_Follow_GetItselfFollowers(int userID, int firstSubscriberID, int secondSubscriberID)
        {
            IUserModule mainUserSubscriberModule = new UserModule(unitOfWork, userID);
            IUserModule firstSubscriberModule = new UserModule(unitOfWork, firstSubscriberID);
            IUserModule secondSubscriberModule = new UserModule(unitOfWork, secondSubscriberID);            

            firstSubscriberModule.FollowTo(userID);
            secondSubscriberModule.FollowTo(userID);
            var followers = mainUserSubscriberModule.GetItselfFollewers;

            Assert.NotNull(followers);
            Assert.AreEqual(2, followers.Count());
            Assert.AreEqual(new[] { 1, 2 }, followers.Select(x => x.ID).ToArray());
        }

        [TestCase(0, 1)]
        public void UserModule_Follow_UnfollowSuccessfully(int userSubscriberID, int userTaret)
        {
            IUserModule userModule = new UserModule(unitOfWork, userSubscriberID);

            userModule.FollowTo(userTaret);
            userModule.Unfollow(userTaret);
            var follower = unitOfWork.Followers
                .Find(x => x.FollowedToID == userTaret && x.FollowerID == userSubscriberID)
                .FirstOrDefault();

            Assert.Null(follower);
        }

        [Test]
        public void UserModule_Follow_TryToFolloewTwice_GetException()
        {
            IUserModule userModule = new UserModule(unitOfWork, 0);

            try
            {
                userModule.FollowTo(1);
                userModule.FollowTo(1);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessLogicException), ex.GetType());
                Assert.AreEqual("User can\'t subscibe twice", ex.Message);
            }
        }

        [Test]
        public void UserModule_Follow_FollowToHimself_GetException()
        {
            int userID = 0;
            IUserModule userModule = new UserModule(unitOfWork, userID);

            try
            {
                userModule.FollowTo(userID);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessLogicException), ex.GetType());
                Assert.AreEqual("User can\'t follow to the himself", ex.Message);
            }
        }

        [Test]
        public void UserModule_Follow_UnfollowFromHimself_GetException()
        {
            int userID = 0;
            IUserModule userModule = new UserModule(unitOfWork, userID);

            try
            {
                userModule.Unfollow(userID);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessLogicException), ex.GetType());
                Assert.AreEqual("User can\'t unfollow from the himself", ex.Message);
            }
        }

        [Test]
        public void UserModule_Follow_FollowToTheNonExistingUser()
        {            
            IUserModule userModule = new UserModule(unitOfWork, 0);

            try
            {
                userModule.FollowTo(-1);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessEntityNullException), ex.GetType());
                Assert.AreEqual("User with current id is not found", ex.Message);
            }
        }

        [Test]
        public void UserModule_Follow_UnfollowFromTheNonExistingUser()
        {
            IUserModule userModule = new UserModule(unitOfWork, 0);

            try
            {
                userModule.Unfollow(-1);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessEntityNullException), ex.GetType());
                Assert.AreEqual("User with current id is not found", ex.Message);
            }
        }

        [Test]
        public void UserModule_Follow_UnfollowFromUserWhichWasNotFollowed_GetException()
        {
            IUserModule userModule = new UserModule(unitOfWork, 0);

            try
            {
                userModule.Unfollow(1);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessEntityNullException), ex.GetType());
                Assert.AreEqual("AuthorizedUser is not subscribed to the current user", ex.Message);
            }

        }

        [TestCase("MyNewMail@gmail.com")]
        [TestCase("info@rottenberg.nth")]                
        [TestCase("company.info@rottenberg.nth")]
        public void UserModule_ChangeEmail_ChangesAppliedSuccessfully(string newEmail)
        {
            int userID = 0;
            IUserModule userModule = new UserModule(unitOfWork, userID);

            var oldPassword = unitOfWork.Users.Get(userID).Email;
            userModule.ChangeEmail(newEmail);

            Assert.AreNotEqual(oldPassword, unitOfWork.Users.Get(userID).Email);
            Assert.AreEqual(newEmail, unitOfWork.Users.Get(userID).Email);
        }

        [TestCase("@d@mail.com")]
        [TestCase("@d@mail.comCoM")]
        [TestCase("@d@mail.__com")]
        [TestCase("@d@ma#il##$$.com")]
        [TestCase("email")]
        [TestCase(".com")]
        [TestCase("@.com")]        
        [TestCase("mail@..com")]
        [TestCase("ma*il@.com")]
        public void UserModule_ChangeEmail_BadEmailFormat_GetException(string email)
        {
            IUserModule userModule = new UserModule(unitOfWork, 0);

            try
            {
                userModule.ChangeEmail(email);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(FormatException), ex.GetType());
                Assert.AreEqual("The Email has wrong format", ex.Message);
            }
        }

        [TestCase("TooLongNewEmailThatThrowsException@longmailservice.longextenson")]
        public void UserModule_ChangeEmail_TooLongEMail_GetException(string newEmail)
        {
            IUserModule userModule = new UserModule(unitOfWork, 0);

            try
            {
                userModule.ChangeEmail(newEmail);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessLogicException), ex.GetType());
                Assert.AreEqual("Email length must be less then 32", ex.Message);
            }
        }

        [Test]
        public void UserModule_ChangeEmail_EmailIsTheSame_GetException()
        {
            int userID = 0;
            IUserModule userModule = new UserModule(unitOfWork, userID);

            try
            {
                var oldEmail = unitOfWork.Users.Get(userID).Email;
                userModule.ChangeEmail(oldEmail);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessLogicException), ex.GetType());
                Assert.AreEqual("New email has to be different to the current", ex.Message);
            }
        }

        [TestCase("123111newPass")]
        public void UserModule_ChangePassword_ChangesAppliedSuccesfully(string password)
        {
            int userID = 0;
            IUserModule userModule = new UserModule(unitOfWork, userID);

            userModule.ChangePassword(password);
            var passwordAfterChanges = unitOfWork.Users.Get(userID).Password;

            Assert.AreEqual(password, passwordAfterChanges);
        }

        [TestCase("TooLongPasswordSoItThrowsBusinessLogicExceptionSoBeReady")]
        public void UserModule_ChangePassword_TooLongEMail_GetException(string password)
        {
            IUserModule userModule = new UserModule(unitOfWork, 0);

            try
            {
                userModule.ChangePassword(password);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessLogicException), ex.GetType());
                Assert.AreEqual("New Password length must be less then 32", ex.Message);
            }
        }

        [Test]
        public void UserModule_ChangePassword_TheSamePassword_GetException()
        {
            int userID = 0;
            IUserModule userModule = new UserModule(unitOfWork, userID);

            try
            {
                var oldPassword = unitOfWork.Users.Get(userID).Password;
                userModule.ChangePassword(oldPassword);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessLogicException), ex.GetType());
                Assert.AreEqual("New password has to be different to the current", ex.Message);
            }
        }
    }
}
