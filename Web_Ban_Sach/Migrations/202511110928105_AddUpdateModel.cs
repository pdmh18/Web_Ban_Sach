namespace Web_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdateModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Editors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EditionNumber = c.Int(nullable: false),
                        Language = c.String(maxLength: 50),
                        Pages = c.Int(nullable: false),
                        Publisher = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        ContactPhone = c.String(nullable: false, maxLength: 200),
                        Address = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "genreId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "genreId");
            AddForeignKey("dbo.Books", "genreId", "dbo.Genres", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "genreId", "dbo.Genres");
            DropIndex("dbo.Books", new[] { "genreId" });
            DropColumn("dbo.Books", "genreId");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Editors");
            DropTable("dbo.Genres");
        }
    }
}
