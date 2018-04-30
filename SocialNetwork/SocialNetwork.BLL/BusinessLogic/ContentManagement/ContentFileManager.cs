using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialNetwork.BLL.BusinessLogic.ContentManagement
{
    public sealed class ContentFileManager
    {
        private IUnitOfWork unitOfWork;

        public ContentFileManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public string Create(string name, int masterID)
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

            return fullpath;
        }

        public void WriteToDialog(string dialogFullPath, int userID, string text, IEnumerable<FileStream> contentStream)
        {
            XDocument xdoc = XDocument.Load(dialogFullPath);

            XElement message = new XElement("message");
            message.Add(new XAttribute("userID", userID));
            message.Add(new XAttribute("sent_at", GetLongFormattedTime()));
            message.Add(new XElement("text", text));

            if (contentStream != null)
            {
                foreach (var item in contentStream)
                {
                    string contentFullName = GetContentName(userID);
                    using (var fileWritter = new StreamWriter(contentFullName))
                    {
                        var bytes = new byte[item.Length];
                        item.Read(bytes, 0, (int)item.Length);
                        fileWritter.Write(bytes);
                    }

                    unitOfWork.ContentPaths.Add(new Content() { Category = "DialogContent", Path = contentFullName });

                    message.Add(new XElement("contentID", unitOfWork.ContentPaths.Find(x => x.Path == contentFullName).First().ID));
                }
            }

            xdoc.Root.Add(message);
            xdoc.Save(dialogFullPath);
        }

        private string GetLongFormattedTime()
        {
            return string.Format("{0:s}", DateTime.Now).Replace('-', ':');
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