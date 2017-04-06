namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YoutubeLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "YoutubeLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "YoutubeLink");
        }
    }
}
