namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtistAddDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "AddedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artists", "AddedAt");
        }
    }
}
