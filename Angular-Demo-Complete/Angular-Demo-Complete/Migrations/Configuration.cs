namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Angular_Demo_Complete.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<Angular_Demo_Complete.MusicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(Angular_Demo_Complete.MusicContext context)
        {
            var ArtistsWhoNeedFolders = (from data in context.Artist where data.FilePath == null | data.FilePath == "" select data.ID);

            foreach (var art in ArtistsWhoNeedFolders) {
                FolderStructures.CreateArtistFolderStructure(art);
            }
        }
    }
}
