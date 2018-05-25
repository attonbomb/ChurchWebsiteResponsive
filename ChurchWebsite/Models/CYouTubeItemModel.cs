using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChurchWebsite.Models
{
    public class CYouTubeItemModel
    {
        public CYouTubeItemModel()
        {

        }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public string VideoId { get; set; }

        public string SmallThumb { get; set; }

        public string MediumThumb { get; set; }

        public string LargeThumb { get; set; }
    }
}
