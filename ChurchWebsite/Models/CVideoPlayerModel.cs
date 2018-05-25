using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChurchWebsite.Models
{
    public class CVideoPlayerModel
    {
        private string m_videoId; 
        public CVideoPlayerModel(string videoId)
        {
            m_videoId = videoId;
        }
        
        public string EmbedLink
        {
            get
            {
                return "https://www.youtube.com/embed/"+m_videoId;
            }
        }
    }
}