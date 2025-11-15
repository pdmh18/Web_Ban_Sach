namespace Web_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "EditorId", "dbo.Editors");
            DropForeignKey("dbo.Books", "genreId", "dbo.Genres");
            DropForeignKey("dbo.Books", "supplierId", "dbo.Suppliers");
            DropIndex("dbo.Books", new[] { "genreId" });
            DropIndex("dbo.Books", new[] { "supplierId" });
            DropIndex("dbo.Books", new[] { "EditorId" });
            AlterColumn("dbo.Books", "genreId", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "supplierId", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "EditorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "genreId");
            CreateIndex("dbo.Books", "supplierId");
            CreateIndex("dbo.Books", "EditorId");
            AddForeignKey("dbo.Books", "EditorId", "dbo.Editors", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Books", "genreId", "dbo.Genres", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Books", "supplierId", "dbo.Suppliers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "supplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Books", "genreId", "dbo.Genres");
            DropForeignKey("dbo.Books", "EditorId", "dbo.Editors");
            DropIndex("dbo.Books", new[] { "EditorId" });
            DropIndex("dbo.Books", new[] { "supplierId" });
            DropIndex("dbo.Books", new[] { "genreId" });
            AlterColumn("dbo.Books", "EditorId", c => c.Int());
            AlterColumn("dbo.Books", "supplierId", c => c.Int());
            AlterColumn("dbo.Books", "genreId", c => c.Int());
            CreateIndex("dbo.Books", "EditorId");
            CreateIndex("dbo.Books", "supplierId");
            CreateIndex("dbo.Books", "genreId");
            AddForeignKey("dbo.Books", "supplierId", "dbo.Suppliers", "Id");
            AddForeignKey("dbo.Books", "genreId", "dbo.Genres", "Id");
            AddForeignKey("dbo.Books", "EditorId", "dbo.Editors", "Id");
        }
    }
}
