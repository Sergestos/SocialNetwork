using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SocialNetwork.BLL.BusinessLogic.FSManagement
{
    public static class FSDialogCreator
    {
        public static string Create(string contentDirectory, string name, int masterID)
        {
            string dayTimeNow = (string.Format("{0:u}", DateTime.Now).RemoveChars(' ', ':', '_', '-'));
            string fullpath = string.Concat(contentDirectory, name, "_masterID", masterID, "_hc", dayTimeNow, ".xml");

            XDocument xml = new XDocument();
            XElement rootElement = new XElement("dialog");
            XAttribute attr_masterID = new XAttribute("firstMasterID", masterID);
            rootElement.Add(attr_masterID);

            xml.Add(rootElement);
            xml.Save(fullpath);

            return fullpath;
        }

        private static string RemoveChars(this string input, params char[] chars)
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
