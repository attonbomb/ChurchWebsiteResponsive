using System.Threading;
using System.Collections.Generic;
using System;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using ChurchWebsite.Models;

namespace ChurchWebsite.Data
{
    public sealed class CYouTubeDataLayer
    {
        private static readonly CYouTubeDataLayer instance = new CYouTubeDataLayer();

        private string m_nextPageToken;
        private List<CYouTubeItemModel> myYouTubeModelList;

        private CYouTubeDataLayer(){
            myYouTubeModelList = new List<CYouTubeItemModel>();
        }

        public static CYouTubeDataLayer Instance
        {
            get 
            {
                return instance; 
            }
        }
        
        public List<CYouTubeItemModel> getUploadedVideoData(bool addNextPage = false){

            if (!addNextPage && myYouTubeModelList.Count > 0)
            {
                return myYouTubeModelList;
            }
            
            var myUploadsPlaylistId = "";
            
            YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyBAgEWYGvhZIIuHD96YS_64dtyRchwWC9k" });

            var channelListRequest = yt.Channels.List("contentDetails");
            channelListRequest.Id = "UCgcapnR8NeRQqy6xZG4I0jA";
            var channelListResult = channelListRequest.Execute();
            foreach (var item in channelListResult.Items)
            {
                myUploadsPlaylistId = item.ContentDetails.RelatedPlaylists.Uploads;
            }

            var playListRequest = yt.PlaylistItems.List("snippet");
            playListRequest.PlaylistId = myUploadsPlaylistId;
            if (m_nextPageToken == null)
            {
                if (addNextPage)
                {
                    return myYouTubeModelList;
                } 
            }
            else
            {
                playListRequest.PageToken = m_nextPageToken;
            }
            var playListResult = playListRequest.Execute();
            m_nextPageToken = playListResult.NextPageToken;
            //get next page token and execute again
            CYouTubeItemModel myVideoModel;
            foreach (var item in playListResult.Items)
            {
                myVideoModel = new CYouTubeItemModel();
                myVideoModel.Title = item.Snippet.Title;
                myVideoModel.Description = item.Snippet.Description;
                myVideoModel.VideoId = item.Snippet.ResourceId.VideoId; 
                //myVideoModel.SmallThumb = item.Snippet.Thumbnails.Standard.Url != null ? item.Snippet.Thumbnails.Standard.Url : null;
                myVideoModel.SmallThumb = item.Snippet.Thumbnails.Default.Url;
                myVideoModel.MediumThumb = item.Snippet.Thumbnails.Medium.Url;
                myVideoModel.LargeThumb = item.Snippet.Thumbnails.High.Url;
                myYouTubeModelList.Add(myVideoModel);
            }
            return myYouTubeModelList;
        }
    }
}