using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SocialNetwork.BLL.BusinessLogic.ContentManagement
{
    public sealed class DialogWritter
    {
        private IUnitOfWork unitOfWork;

        public DialogWritter(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void WriteToDialog(string dialogPath, int userID, string text, IEnumerable<FileStream> contentStream)
        {
            XDocument xdoc = XDocument.Load(dialogPath);

            XElement message = new XElement("message");
            message.Add(new XAttribute("userID", userID));
            message.Add(new XAttribute("sent at", DateTime.Now.ToLongTimeString()));
            message.Add(new XElement("text", text));
            
            foreach (var item in contentStream)                
            {
                string contentFullName = unitOfWork.MainContentDirectory
                                       + "__byUser_" + userID + DateTime.Now.Millisecond;
                using (var fileWritter = new StreamWriter(contentFullName))
                {
                    var bytes = new byte[item.Length];
                    item.Read(bytes, 0, (int)item.Length);
                    fileWritter.Write(bytes);
                }

                unitOfWork.ContentPaths.Add(new Content() { Category = "DialogContent", Path = contentFullName });

                message.Add(new XElement("contentID", unitOfWork.ContentPaths.Find(x => x.Path == contentFullName).First()));
            }

            xdoc.Save(dialogPath);
        }        
    }
}
