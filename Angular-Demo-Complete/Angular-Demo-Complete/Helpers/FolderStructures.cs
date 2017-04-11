using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Helpers
{
    public class FolderStructures
    {
        public static void CreateArtistFolderStructure(int ID)
        {
            //Need to hold a global path here
#if DEBUG
            const String RootPath = @"C:\Users\baile\Source\Repos\Angular-demo\Angular-Demo-Complete\Angular-Demo-Complete\ArtistData\";

#else
            const String RootPath = @"C:\MusicContent\ArtistData\";
#endif

            using (var db = new MusicContext())
            {
                var artist = (from data in db.Artist where data.ID == ID select data).SingleOrDefault();

                if (artist != null)
                {

                    var ArtistRootPath = RemoveIllegalPathCharacters.RemoveSpaces(Path.Combine(RootPath, String.Format(@"{0}\", RemoveIllegalPathCharacters.RemoveCharacters(artist.firstName))));
                    try
                    {
                        //Create the top level for the artist
                        Directory.CreateDirectory(ArtistRootPath);
                        SaveArtistFolderInDb(ArtistRootPath, ID);
                    }
                    catch (Exception)
                    {

                    }


                    //Create all the album folders for that artist
                    foreach (var album in artist.Albums)
                    {
                        //Form the path
                        var AlbumRootPath = RemoveIllegalPathCharacters.RemoveSpaces(Path.Combine(ArtistRootPath, String.Format(@"{0}\", RemoveIllegalPathCharacters.RemoveCharacters(album.title))));
                        try
                        {
                            Directory.CreateDirectory(AlbumRootPath);
                            SaveAlbumFolderInDb(AlbumRootPath, album.ID);
                        }
                        catch (Exception)
                        {

                        }


                        //Create all the song folder for that artist
                        foreach (var song in album.Songs)
                        {
                            try
                            {
                                //Root song path
                                var SongRootPath = RemoveIllegalPathCharacters.RemoveSpaces(Path.Combine(AlbumRootPath, String.Format(@"{0}\", RemoveIllegalPathCharacters.RemoveCharacters(song.title))));
                                Directory.CreateDirectory(SongRootPath);
                                //Path for audio files
                                var SongSubFolderAudio = RemoveIllegalPathCharacters.RemoveSpaces(Path.Combine(SongRootPath, String.Format(@"{0}\", RemoveIllegalPathCharacters.RemoveCharacters("Audio"))));
                                Directory.CreateDirectory(SongSubFolderAudio);
                                //Path for video files
                                var SongSubFolderVideo = RemoveIllegalPathCharacters.RemoveSpaces(Path.Combine(SongRootPath, String.Format(@"{0}\", RemoveIllegalPathCharacters.RemoveCharacters("Video"))));
                                Directory.CreateDirectory(SongSubFolderVideo);
                                //Update the db so the song has it's path
                                SaveSongFolderInDb(SongRootPath, song.ID);
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }



                }
            }

        }

        public static void SaveAlbumFolderInDb(string albumRootPath, int ID)
        {
            using (var db = new MusicContext())
            {
                var needsFolderInDb = (from data in db.Albums where data.ID == ID select (!(data.FilePath == null)) | (!(data.FilePath == ""))).SingleOrDefault();


                if (needsFolderInDb)
                {
                    var album = (from data in db.Albums where data.ID == ID select data).SingleOrDefault();

                    if (album != null)
                    {
                        album.FilePath = albumRootPath;
                        album.canDownload = true;
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void SaveSongFolderInDb(string songRootPath, int ID)
        {
            using (var db = new MusicContext())
            {
                var needsFolderInDb = (from data in db.Songs where data.ID == ID select (!(data.FilePath == null)) | (!(data.FilePath == ""))).SingleOrDefault();


                if (needsFolderInDb)
                {
                    var song = (from data in db.Songs where data.ID == ID select data).SingleOrDefault();

                    if (song != null)
                    {
                        song.FilePath = songRootPath;
                        song.canDownload = true;
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void SaveArtistFolderInDb(String path, int ID)
        {
            using (var db = new MusicContext())
            {
                var needsFolderInDb = (from data in db.Artist where data.ID == ID select (!(data.FilePath == null)) | (!(data.FilePath == ""))).SingleOrDefault();

                if (needsFolderInDb)
                {
                    var artist = (from data in db.Artist where data.ID == ID select data).SingleOrDefault();

                    if (artist != null)
                    {
                        artist.FilePath = path;
                        artist.canDownload = true;
                        db.SaveChanges();
                    }
                }
            }
        }
        
        public static String FindAlbumFilePath(int ID) {
            using (var db = new MusicContext()) {
                var albumPath = (from data in db.Albums where data.ID == ID select data.FilePath).SingleOrDefault();

                if (albumPath != null)
                {
                    return albumPath;
                }
                else {
                    return null;
                }
            }
        }

        public static String FindSongFilePath(int ID) {
            using (var db = new MusicContext()) {
                var songPath = (from data in db.Songs where data.ID == ID && data.canDownload == true select data.FilePath).SingleOrDefault();
                var test = (from data in db.Songs where data.ID == ID select data).SingleOrDefault();
                if (songPath != null)
                {
                    return songPath;
                }
                else {
                    return null;
                }
            }
        }

        public static String FindArtistFilePath(int ID) {
            using (var db = new MusicContext()) {
                var artistPath = (from data in db.Artist where data.ID == ID select data.FilePath).SingleOrDefault();

                if (artistPath != null)
                {
                    return artistPath;
                }
                else {
                    return null;
                }
            }
        }

        public static String GetSongName(int ID) {
            using (var db = new MusicContext()) {

                var songTitle = (from data in db.Songs where data.ID == ID select data.title).SingleOrDefault();

                if (songTitle != null)
                {
                    return songTitle;
                }
                else {
                    return null;
                }

            }
        }

        public static void DeleteAllFolders() {
#if DEBUG
            const String RootPath = @"C:\Users\baile\Source\Repos\Angular-demo\Angular-Demo-Complete\Angular-Demo-Complete\ArtistData\";

#else
            const String RootPath = @"C:\MusicContent\ArtistData\";
#endif

            System.IO.DirectoryInfo di = new DirectoryInfo(RootPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

        }

    }
}