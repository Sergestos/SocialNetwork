using SocialNetwork.BLL.DataProvider;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialNetwork.BLL.BusinessLogic.ContentManagement
{
    public sealed class ContentFileManager : IContentFileManager
    {
        private IUnitOfWork unitOfWork;

        internal ContentFileManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ContentFileManager(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWork = unitOfWorkFactory.Create();
        }

        public void CreateDialog(string name, int masterID, out string path)
        {
            if (!new DirectoryInfo(unitOfWork.MainContentDirectory).Exists)
                throw new BusinessLogic.Exceptions.BusinessWrongRootException("This directory is not exist");

            string dayTimeNow = RemoveChars(string.Format("{0:u}", DateTime.Now), ' ', ':', '_', '-');
            string fullpath = string.Concat(unitOfWork.MainContentDirectory, name, "_masterID", masterID, "_hc", dayTimeNow, ".xml");

            XDocument xml = new XDocument();
            XElement rootElement = new XElement("dialog");
            XAttribute attr_masterID = new XAttribute("firstMasterID", masterID);
            rootElement.Add(attr_masterID);

            xml.Add(rootElement);
            xml.Save(fullpath);

            var content = new Content() { Category = "Dialog", Path = fullpath };
            unitOfWork.Content.Add(content);

            path = fullpath;
        }

        public void WriteDialog(string dialogFullPath, int userID, string text, IEnumerable<Stream> contentStream)
        {
            XDocument xdoc = XDocument.Load(dialogFullPath);

            XElement message = new XElement("message");
            message.Add(new XAttribute("userID", userID));
            message.Add(new XAttribute("sent_at", GetLongFormattedTime()));
            message.Add(new XElement("text", text));

            List<XElement> contentElement = new List<XElement>();

            if (contentStream != null)
            {
                foreach (var item in contentStream)
                {
                    string contentFullPathName = GetContentName(userID);
                    using (var fileStream = File.Create(contentFullPathName))
                    {
                        item.Seek(0, SeekOrigin.Begin);
                        item.CopyTo(fileStream);
                    }

                    unitOfWork.Content.Add(new Content() { Category = "DialogContent", Path = contentFullPathName, Extension = ".jpg" });

                    contentElement.Add(new XElement("contentID", unitOfWork.Content.Find(x => x.Path == contentFullPathName).First().ID));
                }
            }

            foreach (var item in contentElement)
                message.Add(item);

            xdoc.Root.Add(message);
            xdoc.Save(dialogFullPath);
        }

        public void UploadFile(Stream file, string FileCategory, out string savedPath)
        {
            savedPath = GetContentName();

            using (var fileStream = File.Create(savedPath))
            {
                file.Seek(0, SeekOrigin.Begin);
                file.CopyTo(fileStream);
            }

            string fileExtension = FileCategory == "Avatar" ? ".jpg" :
                                   FileCategory == "Dialog" ? ".xml" :
                                   throw new InvalidDataException("ContentFileManager doesn\'t support this type of files");

            unitOfWork.Content.Add(new Content()
            {
                Category = FileCategory,
                Path = savedPath,
                Extension = fileExtension
            });
        }
        
        public Byte[] GetFile(string fullPath)
        {
            return File.ReadAllBytes(fullPath);
        }

        private string GetLongFormattedTime()
        {
            return string.Format("{0:s}", DateTime.Now).Replace('-', ':');
        }

        private string GetContentName()
        {
            return string.Concat(unitOfWork.MainContentDirectory, "Content",
                "__hc", RemoveChars(GetLongFormattedTime(), ':'));
        }

        private string GetContentName(int userID)
        {
            return string.Concat(unitOfWork.MainContentDirectory, "Content_byUser_", userID,
                "__hc", RemoveChars(GetLongFormattedTime(), ':'));
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