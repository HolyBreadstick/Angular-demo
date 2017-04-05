namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmootherImageLoading : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "imageLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "imageLink");
        }
    }
}
