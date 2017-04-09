using Angular_Demo_Complete.Entities;
using Angular_Demo_Complete.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using YoutubeExtractor;

namespace Angular_Demo_Complete.Controllers
{


    [RoutePrefix("api/Information")]
    public class InformationController : ApiController
    {
        private MusicContext db;

        public InformationController() {
            db = new MusicContext();
        }

        [Route("Time")]
        public object GetTime() {
            return DateTime.Now;
        }

        [Route("Download")]
        public object DownloadVideoAs(String link, Boolean wantVideo) {
            
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link, false);

            //DownloadAudio(videoInfos);
            var totalPath = "";

            if (wantVideo)
            {
                totalPath = DownloadVideo(videoInfos, link);
            }
            else if (wantVideo == false)
            {
                totalPath = DownloadAudio(videoInfos, link);
            }
            else {
                return null;
            }

            var downloadLink = "http://localhost:50569/" + RemoveIllegalPathCharacters.FormatForBrowser(totalPath);

            return new
            {
                Link = downloadLink,
                FileName = Path.GetFileName(totalPath)
            };




            

        }

        private String DownloadAudio(IEnumerable<VideoInfo> videoInfos, String Link)
        {
            /*
             * We want the first extractable video with the highest audio quality.
             */
            VideoInfo video = null;
            video = videoInfos
                .Where(info => info.CanExtractAudio)
                .OrderByDescending(info => info.AudioBitrate)
                .FirstOrDefault();

            if (video == null) {
                video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
            }
            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the audio downloader.
             * The first argument is the video where the audio should be extracted from.
             * The second argument is the path to save the audio file.
             */
            var endPath = Path.Combine(FolderStructures.FindSongFilePath(YoutubeLinkExtractor.FindVideoID(Link)), String.Format(@"{0}\", "Audio"), String.Format(@"{0}{1}", FolderStructures.GetSongName(YoutubeLinkExtractor.FindVideoID(Link)), ".mp3"));
            var audioDownloader = new VideoDownloader(video, endPath);
            
            /*
             * Execute the audio downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            audioDownloader.Execute();

            return endPath;
        }

        private String DownloadVideo(IEnumerable<VideoInfo> videoInfos, String Link)
        {
            /*
             * Select the first .mp4 video with 360p resolution
             */
            VideoInfo video = videoInfos
                .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            var endPath = Path.Combine(FolderStructures.FindSongFilePath(YoutubeLinkExtractor.FindVideoID(Link)), String.Format(@"{0}\", "Video"), String.Format(@"{0}{1}", FolderStructures.GetSongName(YoutubeLinkExtractor.FindVideoID(Link)), video.VideoExtension));
            var videoDownloader = new VideoDownloader(video, endPath);


            // Register the ProgressChanged event and print the current progress
            videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

            /*
             * Execute the video downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            videoDownloader.Execute();

            return endPath;
        }

        

    }
}
