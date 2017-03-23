namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Angular_Demo_Complete.MusicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

        }

        protected override void Seed(Angular_Demo_Complete.MusicContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            

            //The Weeknd, Starboy
            //art.Add(new Entities.Artist()
            //{
            //    firstName = "The",
            //    lastName = "Weeknd",
            //    birthdate = new DateTime(1990, 2, 16),
            //    Albums = new System.Collections.Generic.List<Entities.Album>() {
            //        new Entities.Album("https://lh3.googleusercontent.com/jHGD4o9ghSK2JJ8-KWAwoLXi9SXAzVAfwKmS72K0gSfFMpAc7May2yc9x9hDKwu9S_6UfBJh7w=w300-rw"){
            //            title = "Starboy",
            //            views = 100000,
            //            Songs = new System.Collections.Generic.List<Entities.Song>(){
            //                new Entities.Song(){
            //                    title="Starboy (feat. Daft Punk)",
            //                    discount = .25,
            //                    onSale = true,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Party Monster",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="False Alarm",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Reminder",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Rockin'",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Secrets",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="True Colors",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Stargirl Interlude (feat. Lana Del Rey)",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Sidewalks (feat. Kendrick Lamar)",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Six Feet Under",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Love to Lay",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="A Lonely Night",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Attention",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Ordinary Life",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Nothing Without You",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="All I Know (feat. Future)",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="Die For You",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                },new Entities.Song(){
            //                    title="I Feel It Coming (feat. Daft Punk)",
            //                    discount = 0,
            //                    onSale = false,
            //                    storedPrice = 1.29
            //                }
            //            }
                        
            //        }
            //    }
            //});


        }
    }
}
