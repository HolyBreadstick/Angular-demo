using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Angular_Demo_Complete.Models.ArtistSearch;
using Angular_Demo_Complete.Models;
using System.Data.Entity.SqlServer;
using System.IO;
using Angular_Demo_Complete.Helpers;

namespace Angular_Demo_Complete.Controllers
{
    [RoutePrefix("api/Artist")]
    public class ArtistController : ApiController
    {
        private MusicContext db;

        //Set a base URL for the api
        private String baseUrl = "http://ws.audioscrobbler.com/2.0/";
        //Need API key for lastFM
        private String apiKey = "9f0228753db24ea3148fe0934e2d0d27";
        private WebClient client = new WebClient();

        

        public ArtistController() {
            db = new MusicContext();
        }


        [Route("All")]
        public object GetAllArtistID()
        {
            
            var ranArt = (from user in db.Albums orderby Guid.NewGuid() select user.ID).Take(db.Albums.Count() >= 25 ? 25 : db.Albums.Count());
            var info = (from data in db.Albums orderby data.ID where ranArt.Contains(data.ID) & data.Owner != null orderby data.title select data.ID);

            return info;
        }

        [Route("RefreshAll")]
        public object RefreshAllContent(Boolean Force = false) {
            var timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            FolderStructures.DeleteAllFolders();

            //Keeps Artist and their data if the data is not a day old.
            var allArtistNames = (from data in db.Artist where SqlFunctions.DateDiff("day", data.AddedAt, DateTime.UtcNow) > 1 | true==Force select data.firstName).ToList();

            foreach (var name in allArtistNames) {
                db.ArtistBackups.Add(new Entities.BackupArtists() {
                firstName = name});
            }

            db.SaveChanges();

            for (int i = 0; i < allArtistNames.Count; i++) {
                if ((from data in db.Artist where data.Albums.Count == 0 select data.firstName).ToList().Contains(allArtistNames[i])) {
                    RemoveArtist(allArtistNames[i]);
                    allArtistNames.RemoveAt(i);
                    var name = allArtistNames[i];
                    db.ArtistBackups.Remove((from data in db.ArtistBackups where data.firstName == name select data).Single());
                }
            }

            db.SaveChanges();

            foreach (var art in allArtistNames) {
                RemoveArtist(art);
            }

            var rogueLinks = (from data in db.YoutubeLinks where data.Owner == null select data);
            db.YoutubeLinks.RemoveRange(rogueLinks);

            var rogueSongs = (from data in db.Songs where data.Owner == null select data);
            db.Songs.RemoveRange(rogueSongs);

            db.Albums.RemoveRange((from data in db.Albums where data.Owner == null select data));

            db.SaveChanges();

            foreach (var art in (from data in db.ArtistBackups select data.firstName).ToList()) {
                AddArtist(art);
                db.ArtistBackups.Remove((from data in db.ArtistBackups where data.firstName == art select data).Single());
            }

            db.SaveChanges();
            timer.Stop();
            return new {
                ExecutionTime = timer.Elapsed,
                ArtistRefreshed = allArtistNames
            };
        }

        [Route("Remove")]
        public void RemoveArtist(String Artist) {
            var artist = (from data in db.Artist where data.firstName == Artist select data).First();

            foreach (var alb in artist.Albums) {
                foreach (var songs in alb.Songs) {
                    songs.YoutubeLinks.RemoveRange(0, songs.YoutubeLinks.Count);
                }
                alb.Songs.RemoveRange(0, alb.Songs.Count);
            }
            artist.Albums.RemoveRange(0, artist.Albums.Count);
            db.Artist.Remove(artist);
            db.SaveChanges();


        }

        [Route("Add")]
        public object AddArtist(String Artist)
        {
            //Need to verify that artist doesn't already exist
            var searchInner = (from data in db.Artist where data.firstName.Contains(Artist) select data).ToList().Count();

            if (searchInner != 0)
            {
                return null;
            }
            else {
                    var Art = new Entities.Artist();
                    if (Artist.Contains(","))
                    {
                        var allArtist = Artist.Split(',');
                        foreach (var art in allArtist)
                        {
                            AddArtist(art.Trim());
                        }
                    }
                    else
                    {
                        //Need to make call to get the artist data
                        var rawData = client.DownloadString(String.Format(baseUrl + "?method=artist.gettopalbums&artist={0}&api_key={1}&format=json", Artist, apiKey));

                        var ArtistSearch = JsonConvert.DeserializeObject<ArtistTopAlbums>(rawData);

                        if (ArtistSearch != null && ArtistSearch.topalbums != null) {
                           Art.firstName = ArtistSearch.topalbums.attr.artist;
                        //Need to create folder structure for the File System
                            if (ArtistSearch.topalbums.album.Length != 0)
                            {
                            db.Artist.Add(Art);
                            db.SaveChanges();
                            AddAlbum(Art.ID, ArtistSearch.topalbums.album);
                            using (var db = new MusicContext()) {
                                var artist = (from data in db.Artist where data.ID == Art.ID select data).SingleOrDefault();

                                if (artist != null) {
                                    if (artist.Albums.Count != 0)
                                    {
                                        FolderStructures.CreateArtistFolderStructure(Art.ID);
                                    }
                                    else {
                                        RemoveArtist(artist.firstName);
                                    }
                                }
                            }
                        }
                        }
                    }

                return Art;
            }

        }

        [Route("Search")]
        public object SearchArtist(String Artist) {
            if (Artist == "*")
            {
                return (from search in db.Artist
                        select new
                        {
                            ID = search.ID,
                            AddedAt = search.AddedAt,
                            firstName = search.firstName
                        }).ToList();
            }
            else {
                return (from search in db.Artist
                        where search.firstName.Contains(Artist)
                        select new
                        {
                            ID = search.ID,
                            AddedAt = search.AddedAt,
                            firstName = search.firstName
                        }).Take(10).ToList();
            }
        }

        [Route("Search/Artist")]
        public object SearchArtistCollection(String Artist) {
             var obj = (from data in db.Artist where data.firstName == Artist select new FastArtistSearch() {
                 ID = data.ID,
                 AddedAt = data.AddedAt,
                 firstName = data.firstName,
                 Albums = (from dt in data.Albums orderby dt.title select new FastAlbumSearch() {
                     ArtistName = dt.Owner.firstName,
                     ID = dt.ID,
                     imageLink = dt.imageLink,
                     title = dt.title,
                     views = dt.views,
                     Songs = (from ds in dt.Songs orderby ds.title select new FastSongSearch() {
                         discount = ds.discount,
                         ID = ds.ID,
                         onSale = ds.onSale,
                         storedPrice = ds.storedPrice,
                         title = ds.title
                     }).ToList()
                 }).ToList()
             }).SingleOrDefault(); 

            return obj;


        }

        [Route("Album/Search")]
        public object SearchAlbum(int Album, bool includeSongs = true) {

            var data = new FastAlbumSearch();
            if (includeSongs)
            {
                data = (from search in db.Albums
                        where search.ID == Album
                        select new FastAlbumSearch()
                        {
                            ID = search.ID,
                            title = search.title,
                            Songs = search.Songs.Select(song => new FastSongSearch()
                            {
                                ID = song.ID,
                                discount = song.discount,
                                onSale = song.onSale,
                                storedPrice = song.storedPrice,
                                title = song.title,
                                YoutubeLink = (from link in song.YoutubeLinks select new FastYoutubeLink() {
                                    Link = link.Link
                                }).ToList()
                            }).ToList(),
                            ArtistName = search.Owner.firstName,
                            imageLink = search.imageLink,
                            views = search.views
                        }).SingleOrDefault();
            }
            else {
                data = (from search in db.Albums
                        where search.ID == Album
                        select new FastAlbumSearch()
                        {
                            ID = search.ID,
                            title = search.title,
                            Songs = search.Songs.Select(song => new FastSongSearch()
                            {
                                ID = song.ID,
                                discount = song.discount,
                                onSale = song.onSale,
                                storedPrice = song.storedPrice,
                                title = song.title,
                                YoutubeLink = (from link in song.YoutubeLinks
                                               select new FastYoutubeLink()
                                               {
                                                   Link = link.Link
                                               }).ToList()
                            }).ToList(),
                            ArtistName = search.Owner.firstName,
                            imageLink = search.imageLink,
                            views = search.views
                        }).SingleOrDefault();
                if (data != null) {
                    data.Songs.Clear();
                }
            }

            if (data != null && data.Songs != null) {
                for (int i = 0; i < data.Songs.Count; i++)
                {
                    var temp = data.Songs[i];

                    if (temp.YoutubeLink.Count() == 0)
                    {
                        var list = SearchYoutube(data.Songs[i].ID);

                        var tmp = new List<FastYoutubeLink>();

                        foreach (var x in list) {
                            tmp.Add(new FastYoutubeLink() {
                                Link = x.Link
                            });
                        }

                        data.Songs[i].YoutubeLink = tmp;
                        var songUpdate = (from dt in db.Songs where dt.ID == data.Songs[i].ID select dt).SingleOrDefault();
                        songUpdate.YoutubeLinks = tmp;
                    }
                }

                var increaseNum = (from search in db.Albums where search.ID == Album select search).SingleOrDefault();

                increaseNum.views += 1;

                db.SaveChanges();
            }

            //System.Threading.Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds));
            

            return data;
        }

        [Route("Song/Youtube")]
        public List<Entities.YoutubeLink> SearchYoutube(int Id) {

            var song = (from data in db.Songs where data.ID == Id select data).SingleOrDefault();

            if (song != null)
            {
                if (song.YoutubeLinks.Count() != 0)
                    return new List<Entities.YoutubeLink>();
                else
                {
                    var link = SearchYoutube(song.title, song.Owner.Owner.firstName);

                    foreach (var i in link) {
                        song.YoutubeLinks.Add(new Entities.YoutubeLink() {
                            Link = i
                        });
                    }
                    
                    db.SaveChanges();

                    return song.YoutubeLinks;

                }
            }
            else {
                return new List<Entities.YoutubeLink>();
            }

        }
        
        private List<String> SearchYoutube(String SongName, String ArtistName) {
            try
            {
                var client = new WebClient();
                client.BaseAddress = "https://www.googleapis.com/youtube/v3/search?part=snippet";
                var Params = "&type=video&videoCatergoryId=10&key=AIzaSyBDg51nViqZI8iupXHPg1v2ODyORtIVYF8&q={0}";

                var completeLink = client.BaseAddress + String.Format(Params, SongName + " by " + ArtistName);

                var respone = client.DownloadString(completeLink);

                var result = JsonConvert.DeserializeObject<YoutubeSongSearch>(respone);

                if (result.items != null & result.items.Length != 0)
                {
                    var temp = new List<String>();

                    foreach (var i in result.items.Take(result.items.Length < 10 ? result.items.Length : 10))
                    {
                        temp.Add(i.id.videoId);
                    }

                    return temp;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        private void AddAlbum(int ArtID, Models.ArtistSearch.Album[] Albums) {

            var maxSearch = Albums.Length;

            if (Albums.Length < maxSearch)
                maxSearch = Albums.Length;

            //Populate Albums into the Artist
            for (int i = 0; i < maxSearch; i++) {
                try
                {

                    using (var db = new MusicContext()) {

                        var Artist = (from data in db.Artist where data.ID == ArtID select data).SingleOrDefault();

                        //Use LastFM api to go get the songs in this album
                        var rawData = client.DownloadString(String.Format(baseUrl + "?method=album.getinfo&api_key={0}&artist={1}&album={2}&format=json", apiKey, Artist.firstName, Albums[i].name));
                        var AlbumSearch = JsonConvert.DeserializeObject<AlbumGetInfo>(rawData);


                        if (AlbumSearch.album != null && !String.IsNullOrEmpty(Albums[i].image[Albums[i].image.Length - 1].text) && AlbumSearch.album.tracks.track.Length != 0)
                        {
                            var workingAlbum = new Entities.Album(Albums[i].image[Albums[i].image.Length - 1].text)
                            {
                                title = AlbumSearch.album.name,
                                views = int.Parse(AlbumSearch.album.playcount),
                                imageLink = Albums[i].image[Albums[i].image.Length - 1].text
                            };
                            Artist.Albums.Add(workingAlbum);
                            db.SaveChanges();
                            AddSongs(workingAlbum.ID, AlbumSearch.album.tracks.track);
                        }
                    }
                }
                catch (Exception)
                {

                }

            }

        }

        private void AddSongs(int AlbId, Models.Track[] Songs) {

            using (var db = new MusicContext()) {

                var Album = (from data in db.Albums where data.ID == AlbId select data).SingleOrDefault();
                Random rn = new Random();

                foreach (var s in Songs)
                {
                    var newSong = new Entities.Song()
                    {
                        title = s.name,
                        discount = rn.NextDouble(),
                        onSale = rn.Next(0, 100) < 25,
                        storedPrice = 1.29
                    };

                    //try
                    //{
                    //    var link = SearchYoutube(s.name, s.artist.name);

                    //    foreach (var i in link)
                    //    {
                    //        newSong.YoutubeLinks.Add(new Entities.YoutubeLink()
                    //        {
                    //            Link = i
                    //        });
                    //    }
                    //}
                    //catch (Exception)
                    //{
                        
                    //}
                    
                    Album.Songs.Add(newSong);
                    db.SaveChanges();
                }
            }

        }
        
    }
}
