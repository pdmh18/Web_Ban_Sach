namespace Web_Ban_Sach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMaNCCMaPB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "supplierId", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "EditorId", c => c.Int());
            CreateIndex("dbo.Books", "supplierId");
            CreateIndex("dbo.Books", "EditorId");
            AddForeignKey("dbo.Books", "EditorId", "dbo.Editors", "Id");
            AddForeignKey("dbo.Books", "supplierId", "dbo.Suppliers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "supplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Books", "EditorId", "dbo.Editors");
            DropIndex("dbo.Books", new[] { "EditorId" });
            DropIndex("dbo.Books", new[] { "supplierId" });
            DropColumn("dbo.Books", "EditorId");
            DropColumn("dbo.Books", "supplierId");
        }
    }
}
