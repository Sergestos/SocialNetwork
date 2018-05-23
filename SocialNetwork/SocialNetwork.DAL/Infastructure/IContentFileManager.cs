using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Infastructure
{
    public interface IContentFileManager
    {
        void CreateDialog(string name, int masterID, out string path);
        void WriteDialog(string dialogPath, int userID, string text, IEnumerable<Stream> streams);

        void UploadFile(Stream file, string FileCategory, out string savedPath);
        Byte[] GetFile(string fullPath);
    }
}
