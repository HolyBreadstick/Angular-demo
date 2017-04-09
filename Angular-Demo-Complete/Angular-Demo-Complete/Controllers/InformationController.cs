using Angular_Demo_Complete.Entities;
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
                totalPath = DownloadVideo(videoInfos);
            }
            else if (wantVideo == false)
            {
                totalPath = DownloadAudio(videoInfos);
            }
            else {
                return null;
            }

            return new
            {
                Link = "http://localhost:50569/Content/" + Path.GetFileName(totalPath),
                FileName = Path.GetFileName(totalPath)
            };




            

        }

        private String DownloadAudio(IEnumerable<VideoInfo> videoInfos)
        {
            /*
             * We want the first extractable video with the highest audio quality.
             */
            VideoInfo video = videoInfos
                .Where(info => info.CanExtractAudio)
                .OrderByDescending(info => info.AudioBitrate)
                .First();

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

            var audioDownloader = new AudioDownloader(video,
                Path.Combine(@"C:\Users\baile\Source\Repos\Angular-demo\Angular-Demo-Complete\Angular-Demo-Complete\Content\",
                RemoveIllegalPathCharacters(video.Title) + video.AudioExtension));

            // Register the progress events. We treat the download progress as 85% of the progress
            // and the extraction progress only as 15% of the progress, because the download will
            // take much longer than the audio extraction.
            audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
            audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

            /*
             * Execute the audio downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            audioDownloader.Execute();

            return Path.Combine(@"C:\Users\baile\Source\Repos\Angular-demo\Angular-Demo-Complete\Angular-Demo-Complete\Content\",
                RemoveIllegalPathCharacters(video.Title) + video.AudioExtension);
        }

        private String DownloadVideo(IEnumerable<VideoInfo> videoInfos)
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
            var videoDownloader = new VideoDownloader(video,
                Path.Combine(@"C:\Users\baile\Source\Repos\Angular-demo\Angular-Demo-Complete\Angular-Demo-Complete\Content\",
                RemoveIllegalPathCharacters(video.Title) + ".mp3"));


            // Register the ProgressChanged event and print the current progress
            videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

            /*
             * Execute the video downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            videoDownloader.Execute();

            return Path.Combine(@"C:\Users\baile\Source\Repos\Angular-demo\Angular-Demo-Complete\Angular-Demo-Complete\Content\",
                RemoveIllegalPathCharacters(video.Title) + ".mp3");
        }

        private string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

    }
}
