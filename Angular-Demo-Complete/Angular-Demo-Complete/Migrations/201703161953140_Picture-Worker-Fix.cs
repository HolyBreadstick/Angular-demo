namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PictureWorkerFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "storedPrice", c => c.Double(nullable: false));
            DropColumn("dbo.Songs", "price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Songs", "price", c => c.Double(nullable: false));
            DropColumn("dbo.Songs", "storedPrice");
        }
    }
}
