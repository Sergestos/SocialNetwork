using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.UnitTests.BusinessLayerTests
{
    using SocialNetwork.BLL.BusinessLogic.FSManagement;

    [TestFixture]
    [Category("FileSystemWork")]
    public class ContentRecordingUniitTests
    {
        private string projectPath = @"F:\Social Network\TestRecordsRepositoty\";

        [TestCase("Dialog1", 0)]
        [TestCase("Guysi", 4)]        
        public void FileSystemWork_FSDialogCreator_CreateDialog(string name, int masterID)
        {
            string fullPath = string.Empty;

            fullPath = FSDialogCreator.Create(projectPath, name, masterID);

            string dayTimeNow = RemoveChars(string.Format("{0:u}", DateTime.Now),' ', ':', '_', '-');
            string expected = string.Concat(projectPath, name, "_masterID", masterID, "_hc", dayTimeNow, ".xml");

            Assert.AreEqual(expected, fullPath);
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
