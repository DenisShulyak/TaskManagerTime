namespace TaskManagerApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTaskManager : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reiterations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        ReiterationId = c.Guid(nullable: false),
                        TypeOperationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reiterations", t => t.ReiterationId, cascadeDelete: true)
                .ForeignKey("dbo.TypeOperations", t => t.TypeOperationId, cascadeDelete: true)
                .Index(t => t.ReiterationId)
                .Index(t => t.TypeOperationId);
            
            CreateTable(
                "dbo.TypeOperations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "TypeOperationId", "dbo.TypeOperations");
            DropForeignKey("dbo.Tasks", "ReiterationId", "dbo.Reiterations");
            DropIndex("dbo.Tasks", new[] { "TypeOperationId" });
            DropIndex("dbo.Tasks", new[] { "ReiterationId" });
            DropTable("dbo.TypeOperations");
            DropTable("dbo.Tasks");
            DropTable("dbo.Reiterations");
        }
    }
}
