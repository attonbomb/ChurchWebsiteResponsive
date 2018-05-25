using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChurchWebsite.Models;
using ChurchWebsite.Data;

namespace ChurchWebsite.Controllers
{
    public class MediaController : Controller
    {
        //
        // GET: /Media/

        public ActionResult Index()
        {
            return RedirectToAction("Sermons");
        }

        public ActionResult Sermons()
        {
            var videos = CYouTubeDataLayer.Instance.getUploadedVideoData();
            return View(videos[0]);
        }

        [HttpGet]
        public ActionResult VideoPlayer(string vidId)
        {
            var myVideoPlayerModel = new CVideoPlayerModel(vidId);
            return PartialView(myVideoPlayerModel);
        }

        [OutputCache(Duration = 1, VaryByParam = "*")]
        public ActionResult _VideosList(bool getMore = false)
        {
            var videos = CYouTubeDataLayer.Instance.getUploadedVideoData(getMore);
            return PartialView("VideoListView", videos);
        }
    }
}
