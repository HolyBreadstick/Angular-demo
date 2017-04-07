namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YoutubeMultiLinkFix3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YoutubeLinks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        BaseLink = c.String(),
                        Owner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Songs", t => t.Owner_ID)
                .Index(t => t.Owner_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.YoutubeLinks", "Owner_ID", "dbo.Songs");
            DropIndex("dbo.YoutubeLinks", new[] { "Owner_ID" });
            DropTable("dbo.YoutubeLinks");
        }
    }
}
