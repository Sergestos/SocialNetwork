using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SocialNetwork.BLL.BusinessLogic.ContentManagement
{
    public static class DialogFileSystemCreator
    {
        public static string Create(string contetDirectory, string name, int masterID)
        {
            string fullpath = string.Concat(contetDirectory, name, "__masterID_", masterID, ".xml");

            XDocument xml = new XDocument();
            XElement rootElement = new XElement("dialog");
            XAttribute attr_masterID = new XAttribute("firstMasterID", masterID);
            rootElement.Add(attr_masterID);

            xml.Add(rootElement);
            xml.Save(fullpath);

            return fullpath;
        }
    }
}
