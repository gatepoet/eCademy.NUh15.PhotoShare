namespace eCademy.NUh15.PhotoShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Image_Id = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.Image_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Image_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Filename = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Photos", "Image_Id", "dbo.Images");
            DropIndex("dbo.Photos", new[] { "User_Id" });
            DropIndex("dbo.Photos", new[] { "Image_Id" });
            DropTable("dbo.Images");
            DropTable("dbo.Photos");
        }
    }
}
