using Angular_Demo_Complete.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete
{
    public class MusicContext : DbContext
    {
        public MusicContext() : base("MusicContext") {
            Database.CreateIfNotExists();
        }


        public DbSet<Artist> Artist { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }

        
    }
}