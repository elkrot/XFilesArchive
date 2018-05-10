namespace XFilesArchive.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchiveEntity",
                c => new
                    {
                        ArchiveEntityKey = c.Int(nullable: false, identity: true),
                        ParentEntityKey = c.Int(),
                        DriveId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 250),
                        EntityType = c.Byte(nullable: false),
                        EntityPath = c.String(maxLength: 250),
                        EntityExtension = c.String(maxLength: 20),
                        Description = c.String(),
                        FileSize = c.Long(),
                        EntityInfo = c.Binary(),
                        CreatedDate = c.DateTime(nullable: false),
                        MFileInfo = c.Binary(),
                        Checksum = c.String(),
                    })
                .PrimaryKey(t => t.ArchiveEntityKey)
                .ForeignKey("dbo.Drive", t => t.DriveId)
                .ForeignKey("dbo.ArchiveEntity", t => t.ParentEntityKey)
                .Index(t => t.ParentEntityKey)
                .Index(t => t.DriveId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryKey = c.Int(nullable: false, identity: true),
                        CategoryTitle = c.String(maxLength: 100),
                        ParentCategoryKey = c.Int(),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CategoryKey);
            
            CreateTable(
                "dbo.Drive",
                c => new
                    {
                        DriveId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        HashCode = c.Int(),
                        DriveInfo = c.Binary(nullable: false),
                        CreatedDate = c.DateTime(),
                        DriveCode = c.String(nullable: false, maxLength: 20),
                        IsSecret = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DriveId);
            
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        ImageKey = c.Int(nullable: false, identity: true),
                        Thumbnail = c.Binary(),
                        ImagePath = c.String(maxLength: 255),
                        ThumbnailPath = c.String(maxLength: 255),
                        ImageTitle = c.String(nullable: false, maxLength: 100),
                        HashCode = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImageKey);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagKey = c.Int(nullable: false, identity: true),
                        TagTitle = c.String(maxLength: 50),
                        ModififedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TagKey);
            
            CreateTable(
                "dbo.CategoryToEntity",
                c => new
                    {
                        TargetEntityKey = c.Int(nullable: false),
                        CategoryKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TargetEntityKey, t.CategoryKey })
                .ForeignKey("dbo.ArchiveEntity", t => t.TargetEntityKey, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CategoryKey, cascadeDelete: true)
                .Index(t => t.TargetEntityKey)
                .Index(t => t.CategoryKey);
            
            CreateTable(
                "dbo.ImageToEntity",
                c => new
                    {
                        TargetEntityKey = c.Int(nullable: false),
                        ImageKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TargetEntityKey, t.ImageKey })
                .ForeignKey("dbo.ArchiveEntity", t => t.TargetEntityKey, cascadeDelete: true)
                .ForeignKey("dbo.Image", t => t.ImageKey, cascadeDelete: true)
                .Index(t => t.TargetEntityKey)
                .Index(t => t.ImageKey);
            
            CreateTable(
                "dbo.TagToEntity",
                c => new
                    {
                        TargetEntityKey = c.Int(nullable: false),
                        TagKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TargetEntityKey, t.TagKey })
                .ForeignKey("dbo.ArchiveEntity", t => t.TargetEntityKey, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagKey, cascadeDelete: true)
                .Index(t => t.TargetEntityKey)
                .Index(t => t.TagKey);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagToEntity", "TagKey", "dbo.Tag");
            DropForeignKey("dbo.TagToEntity", "TargetEntityKey", "dbo.ArchiveEntity");
            DropForeignKey("dbo.ArchiveEntity", "ParentEntityKey", "dbo.ArchiveEntity");
            DropForeignKey("dbo.ImageToEntity", "ImageKey", "dbo.Image");
            DropForeignKey("dbo.ImageToEntity", "TargetEntityKey", "dbo.ArchiveEntity");
            DropForeignKey("dbo.ArchiveEntity", "DriveId", "dbo.Drive");
            DropForeignKey("dbo.CategoryToEntity", "CategoryKey", "dbo.Category");
            DropForeignKey("dbo.CategoryToEntity", "TargetEntityKey", "dbo.ArchiveEntity");
            DropIndex("dbo.TagToEntity", new[] { "TagKey" });
            DropIndex("dbo.TagToEntity", new[] { "TargetEntityKey" });
            DropIndex("dbo.ImageToEntity", new[] { "ImageKey" });
            DropIndex("dbo.ImageToEntity", new[] { "TargetEntityKey" });
            DropIndex("dbo.CategoryToEntity", new[] { "CategoryKey" });
            DropIndex("dbo.CategoryToEntity", new[] { "TargetEntityKey" });
            DropIndex("dbo.ArchiveEntity", new[] { "DriveId" });
            DropIndex("dbo.ArchiveEntity", new[] { "ParentEntityKey" });
            DropTable("dbo.TagToEntity");
            DropTable("dbo.ImageToEntity");
            DropTable("dbo.CategoryToEntity");
            DropTable("dbo.Tag");
            DropTable("dbo.Image");
            DropTable("dbo.Drive");
            DropTable("dbo.Category");
            DropTable("dbo.ArchiveEntity");
        }
    }
}
