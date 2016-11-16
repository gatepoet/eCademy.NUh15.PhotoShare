namespace eCademy.NUh15.PhotoShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Timestamp", c => c.DateTime(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "Timestamp");
        }
    }
}
