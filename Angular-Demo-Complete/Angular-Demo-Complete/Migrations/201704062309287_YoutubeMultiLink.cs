namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YoutubeMultiLink : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Songs", "YoutubeLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Songs", "YoutubeLink", c => c.String());
        }
    }
}
