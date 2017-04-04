﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Angular_Demo_Complete.Models.ArtistSearch;
using Angular_Demo_Complete.Models;

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
        public object GetAllArtist()
        {

            var AllArtist = db.Artist.ToList();

            return AllArtist;

        }

        [Route("RefreshAll")]
        public object RefreshAllContent() {
            var timer = new System.Diagnostics.Stopwatch();

            timer.Start();
            var allArtistNames = (from data in db.Artist select data.firstName).ToList();

            foreach (var art in allArtistNames) {
                RemoveArtist(art);
            }

            //Clear out all rogue songs.
            db.Songs.RemoveRange(db.Songs);

            //Clear rogue albums
            db.Albums.RemoveRange(db.Albums);

            //Clear rogue artist
            db.Artist.RemoveRange(db.Artist);

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
                


                //Need to verify that artist doesn't already exist
                var searchInner = (from data in db.Artist where data.firstName.Contains(ArtistSearch.topalbums.attr.artist) select data).ToList();
                if (searchInner.Count == 0)
                {
                    Art.firstName = ArtistSearch.topalbums.attr.artist;
                    AddAlbum(Art, ArtistSearch.topalbums.album);
                    db.Artist.Add(Art);
                    db.SaveChanges();
                }

                
            }
            return Art;

        }

        [Route("Search")]
        public object SearchArtist(String Artist) {
            var data = (from search in db.Artist where search.firstName.Contains(Artist) select search).Take(10).ToList();

            

            return data;
        }

        [Route("Album/Search")]
        public object SearchAlbum(int Album) {
            var data = (from search in db.Albums where search.ID == Album select search).SingleOrDefault();

            data.views += 1;

            db.SaveChanges();

            //System.Threading.Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds));

            return data;
        }

        private void AddAlbum(Entities.Artist Artist, Models.ArtistSearch.Album[] Albums) {

            var maxSearch = 10;

            if (Albums.Length < maxSearch)
                maxSearch = Albums.Length;

            //Populate Albums into the Artist
            for (int i = 0; i < maxSearch; i++) {

                

                //Use LastFM api to go get the songs in this album
                var rawData = client.DownloadString(String.Format(baseUrl + "?method=album.getinfo&api_key={0}&artist={1}&album={2}&format=json",apiKey, Artist.firstName, Albums[i].name));
                var AlbumSearch = JsonConvert.DeserializeObject<AlbumGetInfo>(rawData);


                if (AlbumSearch.album != null)
                {
                    var workingAlbum = new Entities.Album(Albums[i].image[Albums[i].image.Length - 1].text)
                    {
                        title = AlbumSearch.album.name,
                        views = int.Parse(AlbumSearch.album.playcount)
                    };
                    if (AlbumSearch.album.tracks.track.Length != 0)
                    {
                        AddSongs(workingAlbum, AlbumSearch.album.tracks.track);
                        Artist.Albums.Add(workingAlbum);  
                    }
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
