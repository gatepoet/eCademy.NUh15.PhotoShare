namespace eCademy.NUh15.PhotoShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRatings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Rating = c.Int(nullable: false),
                        Photo_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRatings", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRatings", "Photo_Id", "dbo.Photos");
            DropIndex("dbo.UserRatings", new[] { "User_Id" });
            DropIndex("dbo.UserRatings", new[] { "Photo_Id" });
            DropTable("dbo.UserRatings");
        }
    }
}
