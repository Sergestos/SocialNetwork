using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.PresentationLayer.Infastructure
{
    public static class ApplicationHelper
    {
        public static string GetMimeType(string extension)
        {
            extension = extension.ToLower();

            string mime = "image/";

            if (extension == ".png")
                return mime + "png";

            if (extension == ".jpg")
                return mime + "jpeg";

            if (extension == mime + "png" || extension == mime + "jpeg")
                return extension;

            throw new ArgumentException($"Extension \'{extension}\' is not avaibale for this operation");
        }
    }
}