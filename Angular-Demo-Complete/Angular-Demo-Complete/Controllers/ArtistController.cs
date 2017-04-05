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

namespace Angular_Demo_Complete.Controllers
{
    [RoutePrefix("api/Artist")]
    public class ArtistController : ApiController
    {
        private MusicContext db;

        //Set a base URL for the api
        private String baseUrl = "http://ws.audioscrobbler.com/2.0/";
        //Need API key for lastFM
        private String apiKey = "b1f2bc595131a9030dfdd1d2103d447a";
        private WebClient client = new WebClient();

        

        public ArtistController() {
            db = new MusicContext();
        }


        [Route("All")]
        public object GetAllArtistID()
        {
            var info = (from data in db.Albums orderby data.ID select data.ID);
            return info.Skip(new Random().Next(info.Count()-25)).ToList();
        }

        [Route("RefreshAll")]
        public object RefreshAllContent(Boolean Force = false) {
            var timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            //Keeps Artist and their data if the data is not a day old.
            var allArtistNames = (from data in db.Artist where SqlFunctions.DateDiff("day", data.AddedAt, DateTime.UtcNow) > 1 | true==Force select data.firstName).ToList();

            for (int i = 0; i < allArtistNames.Count; i++) {
                if ((from data in db.Artist where data.firstName == allArtistNames[i] select data.firstName).Single() == allArtistNames[i]) {
                    allArtistNames.RemoveAt(i);
                }
            }

            foreach (var art in allArtistNames) {
                RemoveArtist(art);
            }

            var rogueSongs = (from data in db.Songs where data.Owner == null select data);
            db.Songs.RemoveRange(rogueSongs);

            db.Albums.RemoveRange((from data in db.Albums where data.Owner == null select data));



            foreach (var art in allArtistNames) {
                AddArtist(art);
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
                alb.Songs.RemoveRange(0, alb.Songs.Count);
            }
            artist.Albums.RemoveRange(0, artist.Albums.Count);
            db.Artist.Remove(artist);
            db.SaveChanges();


        }
        [Route("Add")]
        public object AddArtist(String Artist)
        {
            var Art = new Entities.Artist();
            if (Artist.Contains(","))
            {
                var allArtist = Artist.Split(',');
                foreach (var art in allArtist)
                {
                    AddArtist(art.Trim());
                }
            }
            else {
                //Need to make call to get the artist data
                var rawData = client.DownloadString(String.Format(baseUrl + "?method=artist.gettopalbums&artist={0}&api_key={1}&format=json", Artist, apiKey));

                var ArtistSearch = JsonConvert.DeserializeObject<ArtistTopAlbums>(rawData);
                
                if (ArtistSearch != null && ArtistSearch.topalbums != null)
                {
                    //Need to verify that artist doesn't already exist
                    var searchInner = (from data in db.Artist where data.firstName.Contains(ArtistSearch.topalbums.attr.artist) select data).ToList();
                    if (searchInner.Count == 0)
                    {
                        Art.firstName = ArtistSearch.topalbums.attr.artist;
                        if (ArtistSearch.topalbums.album.Length != 0)
                        {
                            AddAlbum(Art, ArtistSearch.topalbums.album);
                            db.Artist.Add(Art);
                            db.SaveChanges(); 
                        }
                    } 
                }

                
            }
            return Art;

        }

        [Route("Search")]
        public object SearchArtist(String Artist) {
            return (from search in db.Artist where search.firstName.Contains(Artist) select new {
                ID = search.ID,
                AddedAt = search.AddedAt,
                firstName = search.firstName
            }).Take(10).ToList();
        }

        [Route("Search/Artist")]
        public object SearchArtistCollection(String Artist) {
             var obj = (from data in db.Artist where data.firstName == Artist select new FastArtistSearch() {
                 ID = data.ID,
                 AddedAt = data.AddedAt,
                 firstName = data.firstName,
                 Albums = (from dt in data.Albums select new FastAlbumSearch() {
                     ArtistName = dt.Owner.firstName,
                     ID = dt.ID,
                     imageLink = dt.imageLink,
                     title = dt.title,
                     views = dt.views,
                     Songs = (from ds in dt.Songs select new FastSongSearch() {
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
        public object SearchAlbum(int Album) {
            var data = (from search in db.Albums where search.ID == Album select new FastAlbumSearch() {
                ID = search.ID,
                title = search.title,
                Songs = search.Songs.Select(song=> new FastSongSearch() {
                    ID = song.ID,
                    discount = song.discount,
                    onSale = song.onSale,
                    storedPrice = song.storedPrice,
                    title = song.title
                }).ToList(),
                ArtistName = search.Owner.firstName,
                imageLink = search.imageLink,
                views = search.views
            }).SingleOrDefault();
            

            var increaseNum = (from search in db.Albums where search.ID == Album select search).SingleOrDefault();

            increaseNum.views += 1;

            db.SaveChanges();

            //System.Threading.Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds));

            return data;
        }

        private void AddAlbum(Entities.Artist Artist, Models.ArtistSearch.Album[] Albums) {

            var maxSearch = Albums.Length;

            if (Albums.Length < maxSearch)
                maxSearch = Albums.Length;

            //Populate Albums into the Artist
            for (int i = 0; i < maxSearch; i++) {
                try
                {
                    //Use LastFM api to go get the songs in this album
                    var rawData = client.DownloadString(String.Format(baseUrl + "?method=album.getinfo&api_key={0}&artist={1}&album={2}&format=json", apiKey, Artist.firstName, Albums[i].name));
                    var AlbumSearch = JsonConvert.DeserializeObject<AlbumGetInfo>(rawData);


                    if (AlbumSearch.album != null && !String.IsNullOrEmpty(Albums[i].image[Albums[i].image.Length - 1].text))
                    {
                        var workingAlbum = new Entities.Album(Albums[i].image[Albums[i].image.Length - 1].text)
                        {
                            title = AlbumSearch.album.name,
                            views = int.Parse(AlbumSearch.album.playcount),
                            imageLink = Albums[i].image[Albums[i].image.Length - 1].text
                        };
                        if (AlbumSearch.album.tracks.track.Length != 0)
                        {
                            AddSongs(workingAlbum, AlbumSearch.album.tracks.track);
                            Artist.Albums.Add(workingAlbum);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }

            }

        }

        private void AddSongs(Entities.Album Album, Models.Track[] Songs) {

            Random rn = new Random();

            foreach (var s in Songs)
            {
                var newSong = new Entities.Song() {
                    title = s.name,
                    discount = rn.NextDouble(),
                    onSale = rn.Next(0, 100) < 25,
                    storedPrice = 1.29
                };

                Album.Songs.Add(newSong);
            }

        }

    }
}
