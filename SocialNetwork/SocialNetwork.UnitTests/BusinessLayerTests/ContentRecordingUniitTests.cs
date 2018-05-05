using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.UnitTests.BusinessLayerTests
{
    using SocialNetwork.BLL.BusinessLogic.Exceptions;
    using SocialNetwork.BLL.BusinessLogic.ContentManagement;
    using SocialNetwork.DAL.Infastructure;
    using System.IO;
    using SocialNetwork.BLL.DataProvider;

    [TestFixture]
    [Category("ContentManager Integrated tests")]
    [Ignore("Tests use an outer depends so has to be used only when necessary")]
    public class ContentFileManagerUnitTests
    {
        private string fakeDialogFilePath = @"F:\Social Network\TestRecordsRepositoty\_FakeDialog.xml";
        private string fakeContentPath = @"D:\Screenshots\WoWScrnShot_070417_060208.jpg";
        private string projectPath = @"F:\Social Network\TestRecordsRepositoty\";        
        private FileStream stream;

        private IUnitOfWorkFactory factory;
        private IUnitOfWork unitOfWork;        

        [SetUp]
        public void SetUp()
        {
            factory = new FakeUnitOfWorkFactory(projectPath);
            unitOfWork = factory.Create();
        }

        [TearDown]
        public void TeadDown()
        {
            if (stream != null)
                stream.Close();
        }

        [TestCase("Dialog1", 0)]
        [TestCase("Guysi", 4)]        
        public void ContentFileManager_CreateDialog_Successfully(string name, int masterID)
        {
            ContentFileManager fileManager = new ContentFileManager(unitOfWork);

            string fullPath = fileManager.Create(name, masterID);

            string dayTimeNow = RemoveChars(string.Format("{0:u}", DateTime.Now), ' ', ':', '_', '-');
            string expected = string.Concat(unitOfWork.MainContentDirectory, name, "_masterID", masterID, "_hc", dayTimeNow, ".xml");

            Assert.AreEqual(expected, fullPath);
        }

        [Test]
        public void ContentFileManager_CreateDialog_WrongPath_GetException()
        {            
            try
            {
                ContentFileManager fileManager = new ContentFileManager(new FakeUnitOfWorkFactory("W:\\WrongDirectory").Create());

                fileManager.Create("name", 0);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(BusinessWrongRootException), ex.GetType());
                Assert.AreEqual("This directory is not exist", ex.Message);
            }
        }

        [TestCase("hello")]
        public void ContentFileManager_WriteToDialog_WriteText_Pass(string text)
        {
            ContentFileManager fileManager = new ContentFileManager(unitOfWork);

            fileManager.WriteToDialog(fakeDialogFilePath, 1, text, null);

            Assert.Pass();
        }

        [Test]
        public void ContentFileManager_WriteToDialog_WriteContent_Pass()
        {
            ContentFileManager fileManager = new ContentFileManager(unitOfWork);
            stream = new FileStream(fakeContentPath, FileMode.Open);

            fileManager.WriteToDialog(fakeDialogFilePath, 1, "hello", new[] { stream });
        }

        private string RemoveChars(string input, params char[] chars)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (!chars.Contains(input[i]))
                    sb.Append(input[i]);
            }
            return sb.ToString();
        }
    }
}
