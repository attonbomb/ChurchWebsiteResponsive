using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ChurchWebsite.Models
{
    public class ImageLibraryModel
    {
        public int displayMode { get; set; }
        public List<CImage> displayImages { get; set; }
    }
}