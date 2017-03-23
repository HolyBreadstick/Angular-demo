namespace Angular_Demo_Complete.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CleanupLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Artists", "firstName", c => c.String(maxLength: 999));
            CreateIndex("dbo.Artists", "firstName", unique: true);
            DropColumn("dbo.Artists", "lastName");
            DropColumn("dbo.Artists", "birthdate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Artists", "birthdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Artists", "lastName", c => c.String());
            DropIndex("dbo.Artists", new[] { "firstName" });
            AlterColumn("dbo.Artists", "firstName", c => c.String());
        }
    }
}
